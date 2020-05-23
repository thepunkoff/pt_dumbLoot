using UnityEngine;
using UnityEngine.UI;

public class UnitBuilder : MonoBehaviour
{
    private BattlePrefabManager prefabManager;

    private void Awake()
    {
        prefabManager = FindObjectOfType<BattlePrefabManager>();
    }

    public Unit Build(UnitTemplate template)
    {
        var fieldObject = Instantiate(prefabManager.dot, new Vector3(template.x, 0, 0), Quaternion.identity, template.ground.transform);
        var unit = fieldObject.AddComponent<Unit>();
        if (template.isAI)
        {
            var aiBot = fieldObject.AddComponent<AIBot>();

            foreach (var program in template.botPrograms)
            {
                aiBot.programs.Add(program);
                program.Program.Init(unit);
            }
        }

        unit.stats = template.stats;
        unit.name = template.name;
        unit.isAI = template.isAI;
        unit.color = template.color;

        unit.icon = Instantiate(prefabManager.icon, template.isAI ? prefabManager.enemiesPanel.transform : prefabManager.unitsPanel.transform);
        var colors = unit.icon.GetComponent<Button>().colors;
        colors.normalColor = template.color;
        colors.pressedColor = template.color;
        colors.selectedColor = template.color;
        colors.highlightedColor = template.color;
        unit.icon.GetComponent<Button>().colors = colors;

        unit.GetComponent<SpriteRenderer>().color = template.color;

        unit.icon.GetComponentInChildren<Text>().text = template.name;

        unit.icon.GetComponent<Button>().onClick.AddListener(() => template.onIconClicked(unit));

        unit.onDie = () => template.onDie(unit);

        unit.healthBar = Instantiate(prefabManager.healthBar, unit.transform);
        unit.healthBar.transform.Translate(0, 0.3f, 0);

        unit.actionBar = Instantiate(prefabManager.actionBar, unit.transform);
        unit.actionBar.transform.Translate(0, 0.2f, 0);

        foreach (var ability in template.abilities)
        {
            ability.Init(unit);
            unit.abilities.Add(ability);
        }

        return unit;
    }
}