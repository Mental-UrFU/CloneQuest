using System.Collections.Generic;

/// <summary>
/// This class is designed to store data across entire game.
/// </summary>
public static class PersistentLevelData // TODO REMOVE
{
    public static Dictionary<int, int> LevelStars = new Dictionary<int, int>();
    public static int CurrentLevel = 0;
}
