using BlockFactory.scripts.player;
using Godot;

namespace BlockFactory.scripts.blocks;

/// <summary>
/// The base for complex voxels.
/// </summary>
/// <remarks>
/// VoxelTypes <b>CANNOT</b> store data in their class. Data <b>MUST</b> be stored in the voxel's metadata.
/// </remarks>
public abstract partial class BaseVoxelType : VoxelBlockyType
{
    public virtual void OnPlaced(Player player, Vector3I pos, Vector3 placementNormal, FactoryTerrain terrain)
    {
    }

    public virtual void OnBroken(Player player, Vector3I pos, FactoryTerrain terrain)
    {
    }

    public virtual void NeighborUpdated(Vector3I thisPos, Vector3I neighborPos, FactoryTerrain terrain)
    {
    }

    public virtual InteractionResult OnRightClick(Player player, Vector3I pos, Vector3 clickNormal,
        FactoryTerrain terrain)
    {
        return InteractionResult.Continue;
    }

    public virtual InteractionResult OnLeftClick(Player player, Vector3I pos, Vector3 clickNormal,
        FactoryTerrain terrain)
    {
        return InteractionResult.Continue;
    }
}