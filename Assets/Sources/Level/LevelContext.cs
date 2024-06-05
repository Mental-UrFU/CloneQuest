using System.Collections.ObjectModel;

public class LevelContext
{
    public string Id => ListIds[Index];
    public bool IsLast => Index >= ListIds.Count - 1;
    public int Index { get; private set; }
    public ReadOnlyCollection<string> ListIds { get; private set; }
    public string FromId { get; private set; }

    public LevelContext(int loadIndex, ReadOnlyCollection<string> ids, string fromId)
    {
        Index = loadIndex;
        ListIds = ids;
        FromId = fromId;
    }

    public LevelContext Next => new(IsLast ? ListIds.Count : Index + 1, ListIds, FromId);
}
