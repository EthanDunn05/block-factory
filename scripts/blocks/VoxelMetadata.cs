using Godot;
using Godot.Collections;

namespace BlockFactory.scripts.blocks;

public partial class VoxelMetadata : GodotObject
{
    private Dictionary<string, Variant> dataDict;

    public void SetData<[MustBeVariant] T>(string key, T data)
    {
        dataDict[key] = Variant.From(data);
    }

    public T GetData<[MustBeVariant] T>(string key)
    {
        return dataDict[key].As<T>();
    }
}