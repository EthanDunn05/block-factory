using Godot;

namespace BlockFactory.scripts.blocks;

/// <summary>
/// The base for complex block behavior. This should only be used if you want complex behaviors.
/// </summary>
public abstract partial class BaseFactoryBlock : VoxelBlockyType
{
    public abstract void OnPlace(FactoryVoxelTool vt, ulong id, Vector3I position);
}