using BlockFactory.scripts.block_library;
using BlockFactory.scripts.blocks.attributes;
using BlockFactory.scripts.blocks.interfaces;
using BlockFactory.scripts.player;
using Godot;

namespace BlockFactory.scripts.blocks;

public partial class SlabVoxelType : BaseVoxelType, ICustomPlacementId
{
    public ulong GetId(Player player, Vector3I pos, Vector3 placementNormal, FactoryTerrain terrain)
    {
        var direction = SlabAttribute.Bottom;

        if (placementNormal == Vector3.Up)
        {
            direction = SlabAttribute.Bottom;
        }
        else if (placementNormal == Vector3.Down)
        {
            direction = SlabAttribute.Top;
        }
        else if (placementNormal == Vector3.Forward)
        {
            direction = SlabAttribute.South;
        }
        else if (placementNormal == Vector3.Back)
        {
            direction = SlabAttribute.North;
        }
        else if (placementNormal == Vector3.Left)
        {
            direction = SlabAttribute.East;
        }
        else if (placementNormal == Vector3.Right)
        {
            direction = SlabAttribute.West;
        }

        return FactoryData.BlockLibrary.GetSingleAttributeId(UniqueName, direction);
    }
}