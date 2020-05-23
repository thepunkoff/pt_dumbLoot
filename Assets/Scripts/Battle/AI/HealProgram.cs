using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class HealProgram : BotProgram
{
    public override void Init(Unit unit)
    {
        base.Init(unit);
    }

    public override IEnumerator Step(BattleContext context)
    {
        if ((float)unit.stats.health / unit.stats.maxHealth <= 0.2f && new System.Random().NextDouble() <= 0.3f)
        {
            var hp = (int)(unit.stats.maxHealth * 0.1f);
            unit.stats.health += hp;
            Debug.Log($"{unit.name} healed {hp} health points!");
        }

        yield break;
    }
}