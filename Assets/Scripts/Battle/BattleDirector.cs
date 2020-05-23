using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleDirector : StateMachine
{
    private BattlePrefabManager prefabManager;
    private UnitBuilder unitBuilder;

    private LinkedList<Unit> orderList;

    private LinkedListNode<Unit> currentTurnUnit;
    private List<Action> currentActions = new List<Action>();

    private void Awake()
    {
        prefabManager = FindObjectOfType<BattlePrefabManager>();
        unitBuilder = FindObjectOfType<UnitBuilder>();

        preStateStarted += (o, e) =>
        {
            if (!orderList.Any(x => x.isAI))
                EndBattle(true);

            UpdateActions(e);
            UpdateTargets(e);
        };

        turnEnded += (o, e) => StartNextTurn();
    }

    private void Start()
    {
        var units = InstantiateUnits();
        orderList = RollInitiative(units);

        prefabManager.endTurn.onClick.AddListener(() => StartCoroutine(State.EndTurn()));
        prefabManager.gizmos.onClick.AddListener(() => SwitchGizmos());

        StartNextTurn();
    }

    private void SwitchGizmos()
    {
        foreach (var unit in orderList)
        {
            unit.SwitchGizmo();
        }
    }

    private List<Unit> InstantiateUnits()
    {
        var ground = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
        ground.name = "Ground";

        var templates = new[]
        {
            new UnitTemplate("Rick", -4, Color.blue, isAI: false, ground)
                .SetStats(UnitStats.CreateDefault())
                .AddSkill<Punch>()
                .AddSkill<MoveLeft>()
                .AddSkill<MoveRight>(),
            
            new UnitTemplate("Morty", 3, Color.red, isAI: true, ground)
                .SetStats(UnitStats.CreateDefault().AddMaxHealth(UnityEngine.Random.Range(-40, 41)))
                .AddSkill<Punch>()
                .AddSkill<MoveLeft>()
                .AddSkill<MoveRight>()
                .SetBotProgram<CowardProgram>(1)
                .SetBotProgram<HealProgram>(2)
                .SetBotProgram<FollowProgram>(3)
                .SetBotProgram<PunchProgram>(4),

            new UnitTemplate("Penis", -3, Color.cyan, isAI: false, ground)
                .SetStats(UnitStats.CreateDefault())
                .AddSkill<Punch>()
                .AddSkill<MoveLeft>()
                .AddSkill<MoveRight>(),

            new UnitTemplate("Vagina", 4, Color.magenta, isAI: true, ground)
                .SetStats(UnitStats.CreateDefault().AddMaxHealth(UnityEngine.Random.Range(-40, 41)))
                .AddSkill<Punch>()
                .AddSkill<MoveLeft>()
                .AddSkill<MoveRight>()
                .SetBotProgram<CowardProgram>(1)
                .SetBotProgram<HealProgram>(2)
                .SetBotProgram<FollowProgram>(3)
                .SetBotProgram<PunchProgram>(4)
    };

        var units = new List<Unit>();

        foreach (var t in templates)
        {
            t.SetOnIconClickedCallback((unit) => {
                if (!ReferenceEquals(unit, currentTurnUnit.Value))
                    StartCoroutine(State.SelectUnit(unit));
            });

            t.SetOnDieCallback((unit) => orderList.Remove(unit));

            units.Add(unitBuilder.Build(t));
        }

        return units;
    }

    private LinkedList<Unit> RollInitiative(List<Unit> units)
    {
        var list = new List<Unit>(units);

        var order = new LinkedList<Unit>();

        while (list.Count > 0)
        {
            var index = UnityEngine.Random.Range(0, list.Count);

            order.AddFirst(list[index]);

            list.RemoveAt(index);
        }

        int i = 0;
        foreach (var u in order)
        {
            i++;
            u.icon.GetComponentInChildren<Text>().text += $" ({i})";
        }

        return order;
    }

    private void StartNextTurn()
    {
        if (!orderList.Any(x => !x.isAI))
        {
            EndBattle(false);
            return;
        }

        if (!orderList.Any(x => x.isAI))
        {
            EndBattle(true);
            return;
        }

        if (currentTurnUnit == null)
        {
            currentTurnUnit = orderList.First;
        }
        else
        {
            currentTurnUnit.Value.icon.transform.localScale = new Vector3(1f, 1f, 1f);

            currentTurnUnit = currentTurnUnit.Next == null
                ? orderList.First
                : currentTurnUnit.Next;
        }

        currentTurnUnit.Value.stats.actionPoints = currentTurnUnit.Value.stats.maxActionPoints;
        currentTurnUnit.Value.icon.transform.localScale = Vector3.Scale(currentTurnUnit.Value.icon.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f));

        for (int i = 0; i < prefabManager.actionsPanel.transform.childCount; i++)
            Destroy(prefabManager.actionsPanel.transform.GetChild(i).gameObject);

        for (int i = 0; i < prefabManager.unitsPanel.transform.childCount; i++)
            prefabManager.unitsPanel.transform.GetChild(i).GetComponentInChildren<Button>().interactable = !currentTurnUnit.Value.isAI;

        for (int i = 0; i < prefabManager.enemiesPanel.transform.childCount; i++)
            prefabManager.enemiesPanel.transform.GetChild(i).GetComponentInChildren<Button>().interactable = !currentTurnUnit.Value.isAI;

        prefabManager.endTurn.interactable = !currentTurnUnit.Value.isAI;

        currentActions.Clear();
        foreach (var action in currentTurnUnit.Value.abilities)
        {
            var fieldAction = action.Clone();

            var uiObject = Instantiate(prefabManager.icon, prefabManager.actionsPanel.transform);
            uiObject.GetComponentInChildren<Text>().text = fieldAction.actionName;

            uiObject.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(State.SelectAction(fieldAction)));

            fieldAction.UIObject = uiObject;

            currentActions.Add(fieldAction);
        }

        Debug.Log($"It's {currentTurnUnit.Value.name}'s turn!");

        if (currentTurnUnit.Value.isAI)
            SetState(new EnemyTurnState(
                currentTurnUnit.Value,
                new BattleContext(
                    allUnits: new ReadOnlyCollection<Unit>(orderList.ToList()),
                    currentActions: new ReadOnlyCollection<Action>(currentActions),
                    this),
                this));
        else
            SetState(new WaitingForActionState(this));
    }

    private void UpdateTargets(State state)
    {
        if (state is TargetSkillSelectedState tss)
        {
            if (tss.GetSelectedUnit() != null)
                return;

            if (tss.GetSelectedAction() is TargetAction)
            {
                foreach (var enemy in orderList.Where(x => x.isAI))
                {
                    enemy.icon.GetComponentInChildren<Button>().interactable =
                        ((TargetAction)tss.GetSelectedAction()).IsAvailableWithTarget(enemy);
                }
            }

            if (tss.GetSelectedAction() is SelfAction)
            {
                foreach (var enemy in orderList.Where(x => x.isAI))
                {
                    enemy.icon.GetComponentInChildren<Button>().interactable = true;
                }
            }
        }

        if (state is WaitingForActionState)
        {
            foreach (var enemy in orderList.Where(x => x.isAI))
            {
                enemy.icon.GetComponentInChildren<Button>().interactable = true;
            }
        }
    }
    private void UpdateActions(State state)
    {
        if (state is WaitingForActionState)
        {
            foreach (var action in currentActions)
            {
                // Обнулим цель
                if (action is TargetAction ta)
                {
                    if (ta.HasTarget())
                        ta.SetTarget(null);
                }

                var available = action.IsAvailableForUser(orderList);

                action.UIObject.GetComponent<Button>().interactable = available;
                action.UIObject.GetComponent<Button>().colors = ColorBlock.defaultColorBlock;
            }
        }

        if (state is TargetSkillSelectedState tss)
        {
            if (tss.GetSelectedUnit() == null && tss.GetSelectedAction() is TargetAction ta)
            {
                if (!ta.HasTarget())
                {
                    var colors = tss.GetSelectedAction().UIObject.GetComponent<Button>().colors;
                    colors.normalColor = Color.grey;
                    colors.pressedColor = Color.grey;
                    colors.selectedColor = Color.grey;
                    colors.highlightedColor = Color.grey;
                    tss.GetSelectedAction().UIObject.GetComponent<Button>().colors = colors;
                }
            }
        }

        if (state is TargetUnitSelectedState tus)
        {
            if (tus.GetSelectedAction() == null)
            {
                foreach (var action in currentActions)
                {
                    if (action is TargetAction ta)
                        action.UIObject.GetComponentInChildren<Button>().interactable = ta.IsAvailableWithTarget(tus.GetSelectedUnit());
                }
            }
        }
    }

    private IEnumerator EnemyTurn()
    {
        

        while (EnoughActionPoints(currentTurnUnit.Value))
        {
            
        }

        StartNextTurn();
        yield break;
    }

    private void EndBattle(bool win)
    {
        var popup = Instantiate(win ? prefabManager.youWinPopup : prefabManager.youLosePopup, prefabManager.canvas.transform);
        popup.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene("Battle"));
    }

    private bool EnoughActionPoints(Unit unit)
    {
        return currentActions.Any(x => x.actionPoints <= unit.stats.actionPoints);
    }
}
