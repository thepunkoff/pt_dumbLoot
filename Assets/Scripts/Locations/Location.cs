using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Location : MonoBehaviour, IPointerDownHandler
{
    private GameManager gameManager;
    private PrefabManager prefabManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        prefabManager = FindObjectOfType<PrefabManager>();
    }

    public LocationType type;
    new public string name;
    public string descriprion;
    public bool isDiscovered = false;
    public bool isLooted = false;
    public bool ranOut = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (type == LocationType.Exit)
        {
            gameManager.VisitLocation(this);
            return;
        }

        var popup = Instantiate(prefabManager.locationInfoPopup, prefabManager.popupCanvas.transform);

        popup.GetComponentInChildren<Text>().text = isDiscovered ? $"{name}\n\n{descriprion}" : "This location is not yet discovered";

        popup.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Text>().text = isDiscovered ? "Visit" : "Discover";
        popup.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => { gameManager.VisitLocation(this); Destroy(popup.gameObject); });
        popup.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => Destroy(popup.gameObject));
    }

    public void Discover()
    {
        isDiscovered = true;
        GetComponent<Image>().color = GetColor();
    }

    public void TryDrink(Player player)
    {
        if (!ranOut)
        {
            player.Drink();
            ranOut = true;
        }
        else
        {
            gameManager.WriteToUser("Water ran out.");
        }
    }

    public void TryEat(Player player)
    {
        if (!ranOut)
        {
            player.Eat();
            ranOut = true;
        }
        else
        {
            gameManager.WriteToUser("Food ran out.");
        }
    }

    public void TryLoot(Player player)
    {
        if (!isLooted)
        {
            GiveRandomLoot(player);
            isLooted = true;
        }
        else
        {
            gameManager.WriteToUser("No more loot here.");
        }
    }

    public Color GetColor()
    {
        switch (type)
        {
            case LocationType.Drink:
                return new Color(0.274f, 0.505f, 0.745f);
            case LocationType.Eat:
                return new Color(0.862f, 0.623f, 0.396f);
            case LocationType.Loot:
                return new Color(0.776f, 0.749f, 0.525f);
            case LocationType.Fight:
                return new Color(0.811f, 0.368f, 0.466f);
            case LocationType.Empty:
                return new Color(0.874f, 0.870f, 0.847f);
            case LocationType.Exit:
                return new Color(0.427f, 0.752f, 0.427f);
            case LocationType.Rest:
                return new Color(0.129f, 0.729f, 0.533f);
            default:
                throw new Exception("Unknown location type.");
        }
    }

    private void GiveRandomLoot(Player player)
    {
        var maxFood = 2;
        var foodChance = 0.2f;

        var maxWater = 2;
        var waterChance = 0.2f;

        var maxAmmo = 2;
        var ammoChance = 0.1f;

        var maxMedicine = 2;
        var medicineChance = 0.05f;

        var rng = new System.Random();

        var foodCount = 0;
        var waterCount = 0;
        var ammoCount = 0;
        var medicineCount = 0;

        for (int f = 0; f < maxFood; f++)
            if (rng.NextDouble() < foodChance) foodCount++;
        for (int f = 0; f < maxWater; f++)
            if (rng.NextDouble() < waterChance) waterCount++;
        for (int f = 0; f < maxAmmo; f++)
            if (rng.NextDouble() < ammoChance) ammoCount++;
        for (int f = 0; f < maxMedicine; f++)
            if (rng.NextDouble() < medicineChance) medicineCount++;

        player.Give(ItemType.Food, foodCount);
        player.Give(ItemType.Food, waterCount);
        player.Give(ItemType.Food, ammoCount);
        player.Give(ItemType.Food, medicineCount);

        if (foodCount == 0 && waterCount == 0 && ammoCount == 0 && medicineCount == 0)
        {
            gameManager.WriteToUser("Nothing here.");
        }
    }
}
