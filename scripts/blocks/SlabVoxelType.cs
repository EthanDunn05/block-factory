using BlockFactory.scripts.block_library;
using BlockFactory.scripts.blocks.attributes;
using BlockFactory.scripts.player;
using Godot;

namespace BlockFactory.scripts.blocks;

public partial class SlabVoxelType : BaseVoxelType
{
    public override void OnPlaced(Player player, Vector3I pos, Vector3 placementNormal, FactoryTerrain terrain)
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

        var oldState = FactoryData.BlockLibrary.GetVoxelDataFromId(terrain.TerrainTool.GetVoxelId(pos));
        var newValue = FactoryData.BlockLibrary.GetSingleAttributeId(oldState.Name, direction);
        
        terrain.TerrainTool.SetVoxel(pos, newValue);
    }
}