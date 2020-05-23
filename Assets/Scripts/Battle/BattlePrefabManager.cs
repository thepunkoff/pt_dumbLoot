using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePrefabManager : MonoBehaviour
{
    public GameObject dot;
    public GameObject icon;
    public Bar healthBar;
    public Bar actionBar;
    public TextMeshPro barValue;

    public Canvas canvas;
    public GameObject unitsPanel;
    public GameObject actionsPanel;
    public GameObject enemiesPanel;
    public Button endTurn;
    public Button gizmos;

    public GameObject youWinPopup;
    public GameObject youLosePopup;
}