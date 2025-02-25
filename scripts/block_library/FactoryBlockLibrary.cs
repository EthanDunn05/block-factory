using Godot;

namespace BlockFactory.scripts.block_library;

[GlobalClass]
[Tool]
public partial class FactoryBlockLibrary : VoxelBlockyTypeLibrary
{
    /// <summary>
    /// A wrapper around <see cref="VoxelBlockyTypeLibrary.GetModelIndexDefault"/> for world generation.
    /// </summary>
    public ulong GetDefaultId(StringName name)
    {
        return (ulong) GetModelIndexDefault(name);
    }
}