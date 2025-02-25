using System;
using Godot;

namespace BlockFactory.scripts.worldgen;

[GlobalClass]
[Tool]
public partial class WorldGeneration : VoxelGeneratorScript
{
    [Export] private block_library.FactoryBlockLibrary lib;

    [Export] private FastNoise2 noiseFunc;

    [Export] private int seaLevel;
    [Export] private int heightmapMin;
    [Export] private int heightmapMax;
    
    private int HeightRange => heightmapMax - heightmapMin;

    public override void _GenerateBlock(VoxelBuffer buffer, Vector3I origin, int lod)
    {
        var blockSize = buffer.GetSize().X;

        var chunkPos = new Vector3(
            origin.X >> 4,
            origin.Y >> 4,
            origin.Z >> 4
        );

        if (origin.Y + blockSize * 2 < heightmapMin)
        {
            buffer.Fill(lib.GetDefaultId("dirt"));
        }
        else if (origin.Y > heightmapMax)
        {
            buffer.Fill(lib.GetDefaultId("air"));
        }
        // noise height
        else
        {
            for (var x = 0; x < blockSize; x++)
            for (var z = 0; z < blockSize; z++)
            {
                var height = GetHeight(new Vector2(origin.X + x, origin.Z + z));
                var relativeHeight = height - origin.Y;

                if (relativeHeight > blockSize)
                {
                    buffer.FillArea(lib.GetDefaultId("dirt"),
                        new Vector3I(x, 0, z),
                        new Vector3I(x+1, blockSize, z+1)
                    );
                }
                else if (relativeHeight > 0)
                {
                    buffer.FillArea(lib.GetDefaultId("dirt"),
                        new Vector3I(x, 0, z),
                        new Vector3I(x+1, (int) relativeHeight, z+1)
                    );
                   buffer.SetVoxel(lib.GetDefaultId("grass"), x, (int) (relativeHeight - 1), z);
                    
                    // if ((relativeHeight - (int) relativeHeight) > 0.5f)
                    // {
                    //     buffer.SetVoxel(lib.GetDefaultId("dirt_slab"), x, (int) (relativeHeight), z);
                    // }
                }

                if (height < 0 && origin.Y < HeightRange / 2f)
                {
                    var startHeight = Mathf.Min(0f, relativeHeight);
                    
                    buffer.FillArea(lib.GetDefaultId("water"),
                        new Vector3I(x, (int) startHeight, z),
                        new Vector3I(x + 1, blockSize, z + 1));
                }
            }
        }
    }

    private float GetHeight(Vector2 pos)
    {
        
        var noise = Mathf.Remap(noiseFunc.GetNoise2DSingle(pos), -1f, 1f, 0f, 1f);
        return noise * HeightRange;
    }

    public ulong GetChunkSeed(Vector3 chunkPos)
    {
        return (ulong) Mathf.Pow(chunkPos.X, 31d * chunkPos.Z);
    }
}