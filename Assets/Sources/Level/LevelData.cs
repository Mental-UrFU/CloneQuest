public class LevelData
{
    public bool Passed => _starCount >= 0;
    public int Stars => Passed ? _starCount : 0;

    private readonly int _starCount = -1;

    public LevelData(int starCount = -1) { _starCount = starCount; }

    public static string Serialize(LevelData levelData) => $"{levelData._starCount}";
    public static LevelData Deserialize(string levelData) => new(int.Parse(levelData));
}
