using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitTemplate
{
    public string name;
    public int x;
    public Color color;
    public bool isAI;
    public GameObject ground;
    public List<Action> abilities = new List<Action>();
    public UnitStats stats;
    public Action<Unit> onIconClicked;
    public Action<Unit> onDie;
    public BotProgram botProgram;

    public UnitTemplate(string name, int x, Color color, bool isAI, GameObject ground)
    {
        this.name = name;
        this.x = x;
        this.color = color;
        this.isAI = isAI;
        this.ground = ground;
    }

    public UnitTemplate SetStats(UnitStats stats)
    {
        this.stats = stats;
        return this;
    }

    public UnitTemplate AddSkill<T>() where T : Action
    {
        abilities.Add(ScriptableObject.CreateInstance<T>());
        return this;
    }

    public UnitTemplate SetOnDieCallback(Action<Unit> onDie)
    {
        this.onDie = onDie;
        return this;
    }

    public UnitTemplate SetOnIconClickedCallback(Action<Unit> onIconClicked)
    {
        this.onIconClicked = onIconClicked;
        return this;
    }

    public UnitTemplate SetBotProgram<TProgram>() where TProgram : BotProgram
    {
        if (!isAI)
        {
            Debug.LogWarning("Trying to set AI program to a human controlled unit");
            return this;
        }

        botProgram = ScriptableObject.CreateInstance<TProgram>();
        return this;
    }
}