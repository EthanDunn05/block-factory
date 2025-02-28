using BlockFactory.scripts.block_library;
using BlockFactory.scripts.player;
using Godot;

namespace BlockFactory.scripts.blocks;

public partial class PlantVoxelType : BaseVoxelType
{
    public override void OnPlaced(Player player, Vector3I pos, Vector3 placementNormal, FactoryTerrain terrain)
    {
        
    }

    public override void OnBroken(Player player, Vector3I pos, FactoryTerrain terrain)
    {
        
    }

    public override void NeighborUpdated(Vector3I thisPos, Vector3I neighborPos, FactoryTerrain terrain)
    {
        var underPos = thisPos + Vector3I.Down;
        var underBlock = terrain.TerrainTool.GetVoxelId(underPos);
        var underType = FactoryData.BlockLibrary.GetTypeFromId((int) underBlock);

        if (underType != FactoryData.BlockLibrary.GetTypeFromName("grass"))
        {
            terrain.TerrainTool.SetVoxel(thisPos, FactoryData.BlockLibrary.GetDefaultId("air"));
        }
    }
}