using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    // public GameClass gameClass;
    // public Weapon weapon;

    new public string name;
    public Color color;
    public GameObject icon;
    public UnitStats stats;
    public List<Action> abilities;
    public bool isAI;

    public UnityAction onDie;

    public Bar healthBar;
    public Bar actionBar;

    private bool gizmo;

    private void Awake()
    {
        abilities = new List<Action>();
    }

    public void Die()
    {
        onDie?.Invoke();
        Destroy(icon);
        Destroy(gameObject);
    }

    public void SwitchGizmo()
    {
        gizmo = !gizmo;
    }

    void Update()
    {
        healthBar.SetSize((float)stats.health / stats.maxHealth);
        actionBar.SetSize((float)stats.actionPoints / stats.maxActionPoints);

        healthBar.SetValue($"{stats.health}/{stats.maxHealth}");
        actionBar.SetValue($"{stats.actionPoints}/{stats.maxActionPoints}");

        healthBar.SwitchValue(gizmo);
        actionBar.SwitchValue(gizmo);
    }
}
