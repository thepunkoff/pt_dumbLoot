using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float hunger;
    [SerializeField] private float thirst;

    public float Hunger { get => hunger; set => hunger = value; }
    public float Thirst { get => thirst; set => thirst = value; }
    public float Health { get => health; set => health = value; }

    private Dictionary<ItemType, int> inventory;

    public bool rested;

    public string Inventory 
    {
        get
        {
            if (inventory.Count == 0)
            {
                return "You have nothing in your pockets.";
            }

            var sb = new StringBuilder();

            sb.AppendLine("That's what you managed to find:");

            foreach (var kvp in inventory)
            {
                sb.AppendLine($"{kvp.Key} - {kvp.Value}x");
            }

            return sb.ToString();
        }
    }

    private GameManager gameManager;
    private PrefabManager prefabManager;
    private Settings settings;


    private void Awake()
    {
        prefabManager = FindObjectOfType<PrefabManager>();
        gameManager = FindObjectOfType<GameManager>();
        settings = GetComponent<Settings>();

        inventory = new Dictionary<ItemType, int>();

        health = 1f;
        hunger = 1f;
        thirst = 1f;
    }

    public void Give(ItemType type, int count)
    {
        if (count == 0)
        {
            return;
        }

        gameManager.WriteToUser($"You gained {count} {type}!");

        if (!inventory.ContainsKey(type))
        {
            inventory.Add(type, count);
        }
        else
        {
            inventory[type] += count;
        }
    }

    public void Drink()
    {
        
        thirst += settings.thirstPerDrink;
    }

    public void Eat()
    {
        hunger += settings.hungerPerEat;
    }

    public void Fight()
    {
        if (inventory.ContainsKey(ItemType.Ammo))
        {
            health += settings.healthPerFightAmmo;
            inventory[ItemType.Ammo] += settings.ammoPerFight;
        }
        else
        {
            health += settings.healthPerFightNoAmmo;
        }
    }

    public void Rest()
    {
        if (!rested)
        {
            rested = true;
            gameManager.WriteToUser($"You successfully rested. Time to get out!");
        }
        else
        {
            gameManager.WriteToUser($"You're not exhausted any more.");
        }
    }

    private void Update()
    {
        prefabManager.health.value = health;
        prefabManager.hunger.value = hunger;
        prefabManager.thirst.value = thirst;

        if (health > 1) health = 1;
        if (hunger > 1) hunger = 1;
        if (thirst > 1) thirst = 1;
        if (health < 0) health = 0;
        if (hunger < 0) hunger = 0;
        if (thirst < 0) thirst = 0;
    }
}
