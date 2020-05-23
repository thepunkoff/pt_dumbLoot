using UnityEngine;

public class UnitStats
{
    public int maxActionPoints;
    public int actionPoints;
    public int maxHealth;
    public int health;
    public int attack;
    public int defense;
    public int speed;

    public static UnitStats CreateDefault()
    {
        var stats = new UnitStats
        {
            maxActionPoints = 100,
            maxHealth = 100,
            attack = 20,
            defense = 0,
            speed = 1
        };

        stats.actionPoints = stats.maxActionPoints;
        stats.health = stats.maxHealth;

        return stats;
    }

    public UnitStats AddMaxHealth(int health)
    {
        maxHealth += health;
        this.health = maxHealth;
        return this;
    }
}