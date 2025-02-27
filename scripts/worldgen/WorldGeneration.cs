using System;
using System.Threading.Tasks;
using BlockFactory.scripts.block_library;
using Godot;

namespace BlockFactory.scripts.worldgen;

[GlobalClass]
public partial class WorldGeneration : VoxelGeneratorScript
{
	public const int Seed = 1337;

	[Export] private VoxelGeneratorGraph landShapeGraph;
	[Export] private int lowestBlock;
	[Export] private int highestBlock;

	private FactoryBlockLibrary lib;

	public WorldGeneration()
	{
		lib = FactoryData.BlockLibrary;
		
		
	}

	public override void _GenerateBlock(VoxelBuffer outBuffer, Vector3I blockPos, int lod)
	{
		outBuffer.Fill(lib.GetDefaultId("air"));
		if (blockPos.Y < 0)
		{
			outBuffer.Fill(lib.GetDefaultId("water"));
		}

		// if (blockPos.Y > highestBlock) return;
		// if (blockPos.Y < lowestBlock)
		// {
		// 	outBuffer.Fill(lib.GetDefaultId("stone"));
		// 	return;
		// }
		
		var depthBuffer = LandShape(outBuffer, blockPos);
		Paint(depthBuffer, outBuffer, blockPos);
	}
	

	private VoxelBuffer LandShape(VoxelBuffer outBuffer, Vector3I blockPos)
	{
		var depthBuffer = new VoxelBuffer();
		
		// Generate with a 1 voxel edge for painting
		depthBuffer.Create(outBuffer.GetSize().X + 2, outBuffer.GetSize().Y + 2,outBuffer.GetSize().Z + 2);
		landShapeGraph.GenerateBlock(depthBuffer, blockPos - Vector3I.One, 0);
		
		return depthBuffer;
	}

	private void Paint(VoxelBuffer depthBuffer, VoxelBuffer outBuffer, Vector3I blockPos)
	{
		// Optimization
		if (depthBuffer.IsUniform(0))
		{
			var depth = (short) depthBuffer.GetVoxel(0, 0, 0);
			if (depth > 0) outBuffer.Fill(lib.GetDefaultId("stone"));

			return;
		}

		var rng = new RandomNumberGenerator();
		rng.Seed = MakeRngSeed(blockPos);
		
		for (var x = 0; x < outBuffer.GetSize().X; x++)
		for (var z = 0; z < outBuffer.GetSize().Z; z++)
		{
			var height = (short) depthBuffer.GetVoxel(x + 1, 1, z + 1);
			var heightY = height + blockPos.Y;
			var heightRight = (short) depthBuffer.GetVoxel(x + 2, 1, z + 1);
			var heightUp = (short) depthBuffer.GetVoxel(x + 1, 1, z + 2);

			// Find the slope of the terrain
			var rightVec = new Vector3(1, heightRight - height, 0).Normalized();
			var upVec = new Vector3(0, heightUp - height, 1).Normalized();
			var normal = rightVec.Cross(upVec);
			var slope = Vector3.Up.Dot(normal);
			slope = 1f - Mathf.Abs(slope);
			
			var heightInBlock = ClampInBufferY(height, outBuffer);
			
			outBuffer.FillArea(lib.GetDefaultId("stone"),
				new Vector3I(x, 0, z),
				new Vector3I(x + 1, heightInBlock, z + 1));

			// Add dirt
			if (slope < 0.9f && heightY < 48 + rng.RandiRange(-2, 2))
			{
				var dirtStart = heightInBlock;
				var dirtEnd = ClampInBufferY(height - rng.RandiRange(2, 3), outBuffer);
				outBuffer.FillArea(lib.GetDefaultId("dirt"),
					new Vector3I(x, dirtStart, z),
					new Vector3I(x + 1, dirtEnd, z + 1));

				// Don't put grass underwater
				// Or on mountains
				if (heightY >= 1)
				{
					var grassStart = heightInBlock;
					var grassEnd = ClampInBufferY(height - 1, outBuffer);
					
					outBuffer.FillArea(lib.GetDefaultId("grass"),
						new Vector3I(x, grassStart, z),
						new Vector3I(x + 1, grassEnd, z + 1));
					
					// Random short grass
					if (rng.Randf() < 0.25f)
					{
						var decoStart = ClampInBufferY(height + 1, outBuffer);
						var decoEnd = ClampInBufferY(height, outBuffer);

						var type = "short_grass";
						if (rng.Randf() < 0.25f) type = "tall_grass";
						if (rng.Randf() < 0.1f) type = "flower"; 
						outBuffer.FillArea(lib.GetDefaultId(type),
							new Vector3I(x, decoStart, z),
							new Vector3I(x + 1, decoEnd, z + 1));
					}
				}
			}
			
			// Sand
			if (heightY <= 0)
			{
				var sandStart = heightInBlock;
				var sandEnd = ClampInBufferY(height - 6, outBuffer);
				outBuffer.FillArea(lib.GetDefaultId("sand"),
					new Vector3I(x, sandStart, z),
					new Vector3I(x + 1, sandEnd, z + 1));
			}
		}
	}

	private int ClampInBufferY(int y, VoxelBuffer buffer)
	{
		return Mathf.Clamp(y, 0, buffer.GetSize().Y);
	}

	private ulong MakeRngSeed(Vector3I blockPos)
	{
		return (ulong) Math.Pow(blockPos.X, 31 * blockPos.Z) * Seed;
	}
}
