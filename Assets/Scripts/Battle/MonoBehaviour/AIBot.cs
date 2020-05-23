using System.Collections.Generic;
using UnityEngine;

public class AIBot : MonoBehaviour
{
    public List<(BotProgram Program, int Priority)> programs = new List<(BotProgram Program, int Priority)>();
}