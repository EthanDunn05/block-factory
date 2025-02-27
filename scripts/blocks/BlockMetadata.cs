using Godot;
using Godot.Collections;

namespace BlockFactory.scripts.blocks;

public partial class BlockMetadata : GodotObject
{
    internal Dictionary<string, Variant> dataDict = new();
}

public static class BlockMetadataExtension
{
    public static void SetData<[MustBeVariant] T>(this BlockMetadata self, string key, T data)
    {
        self.dataDict[key] = Variant.From(data);
    }

    public static T GetData<[MustBeVariant] T>(this BlockMetadata self, string key)
    {
        return self.dataDict[key].As<T>();
    }
}