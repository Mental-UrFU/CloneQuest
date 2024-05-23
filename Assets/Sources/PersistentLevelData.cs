using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This class is designed to store data across entire game.
/// </summary>
public static class PersistentLevelData
{
    public static Dictionary<int, int> LevelStars = new Dictionary<int, int>();
    public static int CurrentLevel = 0;
}
