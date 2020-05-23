using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabManager : MonoBehaviour
{
    public Location locationPrefab;
    public Canvas canvas;
    public Canvas popupCanvas;
    public RectTransform map;
    public GameObject console;
    public GameObject gameOverPopup;
    public GameObject locationInfoPopup;
    public GameObject endGamePopup;

    public Slider health;
    public Slider hunger;
    public Slider thirst;
}
