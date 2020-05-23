using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PrefabManager prefabManager;
    private Player player;
    private Settings settings;
    private List<Location> locations;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        prefabManager = FindObjectOfType<PrefabManager>();
        settings = GetComponent<Settings>();

        locations = new List<Location>();
    }

    public void VisitLocation(Location location)
    {
        ClearConsole();
        WriteToUser($"You visited {location.name}.\n{location.descriprion}");

        location.Discover();

        if (player.Health <= 0)
        {
            WriteToUser($"You died of blood loss.");

            GameOver();

            return;
        }

        if (player.Thirst <= settings.thirstThreshold) player.Health += settings.healthPerMoveWhenThirsty;

        if (player.Health <= 0)
        {
            WriteToUser($"You died of thirst.");

            GameOver();

            return;
        }

        if (player.Hunger <= settings.hungerThreshold) player.Health += settings.healthPerMoveWhenHungry;

        if (player.Health <= 0)
        {
            WriteToUser($"You died of starvation.");

            GameOver();

            return;
        }

        switch (location.type)
        {
            case LocationType.Drink:
                location.TryDrink(player);
                break;
            case LocationType.Eat:
                location.TryEat(player);
                break;
            case LocationType.Loot:
                location.TryLoot(player);
                break;
            case LocationType.Fight:
                player.Fight();
                break;
            case LocationType.Empty:
                break;
            case LocationType.Exit:
                if (player.rested)
                    EndGame();
                else
                    WriteToUser("You're very tired. Not the best time for travelling. Gotta find some place to rest.");
                return;
            case LocationType.Rest:
                player.Rest();
                break;
            default:
                throw new Exception("Unknown location type.");
        }

        player.Hunger += settings.hungerPerMove;
        player.Thirst += settings.thirstPerMove;
    }

    private void EndGame()
    {
        var popup = Instantiate(prefabManager.endGamePopup, prefabManager.popupCanvas.transform);

        popup.GetComponentInChildren<Text>().text = $"You escaped the city!\n\n" + player.Inventory;
        popup.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    private void GameOver()
    {
        locations.ForEach(x => Destroy(x.gameObject));
        var popup = Instantiate(prefabManager.gameOverPopup, prefabManager.popupCanvas.transform);
        popup.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    private IEnumerator Start()
    {
        yield return null;
        CreateLocations();
    }

    private void CreateLocations()
    {
        var locationsCount = 20;
        var emptyCount = locationsCount / 2;

        var rng = new System.Random();
        for (int i = 0; i < locationsCount; i++)
        {
            Location location;
            while (true)
            {
                LocationType type;
                if (i < emptyCount)
                {
                    type = LocationType.Empty;
                }
                else
                {
                    type = i < emptyCount + (int)LocationType.TypesCount
                        ? (LocationType)(i % (int)LocationType.TypesCount) 
                        : (LocationType)rng.Next(0, (int)LocationType.TypesCount - 2); // Exit, Rest - last, spawn them 1 time, ignore at rng
                }

                location = SpawnLocation(type);
                location.name = $"location {i}";

                if (locations.Any(x => x.GetComponent<BoxCollider2D>().bounds.Intersects(location.GetComponent<BoxCollider2D>().bounds)))
                {
                    Destroy(location.gameObject);
                }
                else
                {
                    break;
                }
            }

            locations.Add(location);
        }
    }

    private Location SpawnLocation(LocationType type)
    {
        var locationRectWidth = prefabManager.locationPrefab.GetComponent<RectTransform>().rect.width;

        var map = GameObject.Find("Map").GetComponent<RectTransform>();

        var x = UnityEngine.Random.Range(-map.rect.width / 2 + locationRectWidth / 2, map.rect.width / 2 - locationRectWidth / 2);
        var y = UnityEngine.Random.Range(-map.rect.height / 2 + locationRectWidth / 2, map.rect.height / 2 - locationRectWidth / 2);
        var position = prefabManager.map.transform.position + new Vector3(x, y, 0);
        var location = Instantiate(prefabManager.locationPrefab, position, Quaternion.identity, prefabManager.map);

        location.type = type;
        location.gameObject.name = location.type.ToString();
        location.descriprion = $"This is a location of type {location.type}";
        location.GetComponent<Image>().color = location.type == LocationType.Exit ? location.GetColor() : Color.white;

        return location;
    }

    public void WriteToUser(string message)
    {
        var text = prefabManager.console.GetComponentInChildren<Text>().text;

        if (text != string.Empty)
        {
            text += "\n\n";
        }

        text += message;

        Regex.Replace(text, @"^\s*", string.Empty);

        prefabManager.console.GetComponentInChildren<Text>().text = text;
    }

    private void ClearConsole()
    {
        prefabManager.console.GetComponentInChildren<Text>().text = string.Empty;
    }
}