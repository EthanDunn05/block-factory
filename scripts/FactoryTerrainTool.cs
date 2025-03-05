using System;
using System.Linq;
using BlockFactory.scripts.block_library;
using BlockFactory.scripts.blocks;
using Godot;
using Godot.Collections;

namespace BlockFactory.scripts;

/// <summary>
/// This is a wrapper around <see cref="VoxelToolTerrain"/> that
/// integrates it into the rest of the game systems.
/// </summary>
public class FactoryTerrainTool
{
    private VoxelToolTerrain vt;
    private FactoryTerrain terrain;

    public FactoryTerrainTool(FactoryTerrain terrain)
    {
        this.terrain = terrain;
        vt = (VoxelToolTerrain) terrain.GetVoxelTool();
    }

    public Vector3I[] GetNeighborPositions(Vector3I pos)
    {
        return
        [
            pos + Vector3I.Up,
            pos + Vector3I.Down,
            pos + Vector3I.Left,
            pos + Vector3I.Right,
            pos + Vector3I.Forward,
            pos + Vector3I.Back
        ];
    }
    
    public void SetVoxelMetadata(Vector3I pos, VoxelMetadata meta)
    {
        vt.SetVoxelMetadata(pos, meta);
    }

    
    public VoxelMetadata GetVoxelMetadata(Vector3I pos)
    {
        return vt.GetVoxelMetadata(pos).As<VoxelMetadata>();
    }


    public void SetVoxel(Vector3I pos, ulong id)
    {
        vt.SetVoxel(pos, id);
        
        foreach (var neighbor in GetNeighborPositions(pos))
        {
            var neighborType = GetVoxelType(neighbor);

            if (neighborType is BaseVoxelType neighborVt)
            {
                neighborVt.NeighborUpdated(neighbor, pos, terrain);
            }
        }
    }

    public int GetVoxelId(Vector3I pos)
    {
        return (int) vt.GetVoxel(pos);
    }
    
    public VoxelBlockyType GetVoxelType(Vector3I pos)
    {
        return FactoryData.BlockLibrary.GetTypeFromId(GetVoxelId(pos));
    }

    public VoxelRaycastResult? Raycast(Vector3 startPos, Vector3 dir, float distance = 5f, uint mask = 4294967295)
    {
        return vt.Raycast(startPos, dir, distance, mask);
    }

    public bool IsAreaEditable(Aabb aabb)
    {
        return vt.IsAreaEditable(aabb);
    }

    public Array<Aabb> GetVoxelCollision(int voxelId)
    {
        var hoveredData = FactoryData.BlockLibrary.GetVoxelDataFromId(voxelId);
        var hoveredType = FactoryData.BlockLibrary.GetTypeFromName(hoveredData.Name);
        var variants = FactoryData.BlockLibrary.GetVariants(hoveredData.Name);

        var model = hoveredType.BaseModel;
        if (variants.Length > 0)
        {
            model = variants.First(data => data.Attributes.RecursiveEqual(hoveredData.Attributes)).Model;
        }

        return model.CollisionAabbs;
    }
}