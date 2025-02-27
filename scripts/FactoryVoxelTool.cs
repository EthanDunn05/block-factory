using System;
using BlockFactory.scripts.block_library;
using Godot;
using BaseFactoryBlock = BlockFactory.scripts.blocks.BaseFactoryBlock;
using BlockMetadata = BlockFactory.scripts.blocks.BlockMetadata;

namespace BlockFactory.scripts;

/// <summary>
/// A wrapper around the built-in voxel tool for more specialized functionality.
/// This class should nearly always be used over the default VoxelTool!
/// </summary>
public class FactoryVoxelTool
{
    private VoxelTool voxelTool;

    public FactoryVoxelTool(VoxelTool voxelTool)
    {
        this.voxelTool = voxelTool;
    }
    
    public void FillBox(ulong blockType, Vector3I p0, Vector3I p1)
    {
        voxelTool.Value = blockType;
        voxelTool.DoBox(p0, p1);
    }

    public void SetVoxel(ulong blockId, Vector3I pos)
    {
        var data = FactoryData.BlockLibrary.GetBlockDataFromId((int) blockId);
        var typeData = FactoryData.BlockLibrary.GetTypeFromName(data.Name);
        
        voxelTool.Value = blockId;
        voxelTool.DoPoint(pos);
        
        if (typeData is BaseFactoryBlock fb)
        {
            SetMetadata(pos, new BlockMetadata());
            fb.OnPlace(this, blockId, pos);
        }
    }

    public bool IsAreaEditable(Aabb aabb)
    {
        return voxelTool.IsAreaEditable(aabb);
    }

    public VoxelRaycastResult? Raycast(Vector3 origin, Vector3 direction, float distance)
    {
        return voxelTool.Raycast(origin, direction, distance);
    }

    public ulong GetVoxel(Vector3I voxelPos)
    {
        return voxelTool.GetVoxel(voxelPos);
    }

    private void SetMetadata(Vector3I pos, BlockMetadata metadata)
    {
        // Apparently setting metadata only works in the terrain tool...
        // Tyring to use it in the normal VoxelTool throws an error.
        // WHY?
        if (voxelTool is VoxelToolTerrain vtt)
        {
            vtt.SetVoxelMetadata(pos, Variant.CreateFrom(metadata));
        }
    }
 
    public BlockMetadata GetMetadata(Vector3I pos)
    {
        if (voxelTool is VoxelToolTerrain vtt)
        {
            return voxelTool.GetVoxelMetadata(pos).As<BlockMetadata>();
        }

        throw new Exception("Tried to get metadata without terrain tool");
    }
}