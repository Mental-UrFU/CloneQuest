using System;
using System.Collections.Generic;
using System.Linq;
using InstantGamesBridge;

public static class LevelRepository
{
    private const string KEY = "L";

    public static void Get(string levelId, Action<LevelData> callback)
    {
        Bridge.storage.Get($"{KEY}{levelId}", OnStorageGetCompleted);
        void OnStorageGetCompleted(bool success, string data)
        {
            if (!success) { throw new Exception("Can't read storage data"); }
            callback?.Invoke(ConvertStringToLevelData(data));
        }
    }

    public static void Get(IEnumerable<string> levelIds, Action<List<LevelData>> OnComplete)
    {
        Bridge.storage.Get(levelIds.Select((levelId) => $"{KEY}{levelId}").ToList(), OnStorageGetCompleted);
        void OnStorageGetCompleted(bool success, List<string> data)
        {
            if (!success) { throw new Exception("Can't read storage data"); }
            OnComplete?.Invoke(data.Select((levelData) => ConvertStringToLevelData(levelData)).ToList());
        }
    }

    public static void Set(string levelId, LevelData levelData, Action OnComplete)
    {
        Bridge.storage.Set($"{KEY}{levelId}", LevelData.Serialize(levelData), OnStorageSetCompleted);
        void OnStorageSetCompleted(bool success)
        {
            if (!success) { throw new Exception("Can't write storage data"); }
            OnComplete?.Invoke();
        }
    }

    private static LevelData ConvertStringToLevelData(string data)
    {
        try { return LevelData.Deserialize(data); } catch { return new LevelData(); }
    }
}
