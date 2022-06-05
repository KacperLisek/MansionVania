using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Static class, keeps track of player stats to reload on death.
public static class PlayerStats
{
    public static int checkPoint { get; set; } = 0;

    public static int currentLevel { get; set; } = 1;
    public static bool dashUnlocked { get; set; } = false;

    public static bool jumpUnlocked { get; set; } = false;
}
