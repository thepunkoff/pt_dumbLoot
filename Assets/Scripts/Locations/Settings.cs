using UnityEngine;

public class Settings : MonoBehaviour
{
    public float hungerPerMove = -0.15f;
    public float thirstPerMove = -0.2f;
    public float hungerThreshold = 0.3f;
    public float thirstThreshold = 0.3f;
    public float hungerPerEat = 0.3f;
    public float thirstPerDrink = 0.3f;
    public float healthPerMoveWhenThirsty = -0.15f;
    public float healthPerMoveWhenHungry = -0.15f;
    public float healthPerFightNoAmmo = -0.25f;
    public float healthPerFightAmmo = -0.1f;
    public int ammoPerFight = -1;
}
