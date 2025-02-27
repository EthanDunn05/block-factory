using System;
using BlockFactory.scripts.block_library;
using Godot;

namespace BlockFactory.scripts.worldgen;

[GlobalClass]
public partial class WorldGeneration : VoxelGeneratorScript
{
	public const int Seed = 1337;

	[Export] private VoxelGeneratorGraph landShapeGraph;

	private FactoryBlockLibrary lib;

	public WorldGeneration()
	{
		lib = FactoryData.BlockLibrary;
		
		
	}

	public override void _GenerateBlock(VoxelBuffer outBuffer, Vector3I blockPos, int lod)
	{
		outBuffer.Fill(lib.GetDefaultId("air"));
		
		
		
		LandShape(outBuffer, blockPos);
	}
	

	private void LandShape(VoxelBuffer outBuffer, Vector3I blockPos)
	{
		var graphBuffer = new VoxelBuffer();
		graphBuffer.Create(outBuffer.GetSize().X, outBuffer.GetSize().Y,outBuffer.GetSize().Z);
		
		landShapeGraph.GenerateBlock(graphBuffer, blockPos, 0);
		outBuffer.CopyChannelFrom(graphBuffer, 0);
		outBuffer.RemapValues(0, new int[]
		{
			(int) lib.GetDefaultId("air"),
			(int) lib.GetDefaultId("stone"),
			(int) lib.GetDefaultId("dirt"),
			(int) lib.GetDefaultId("grass"),
		});
	}

	private void Decorate(VoxelBuffer outBuffer, Vector3I blockPos)
	{
		if (outBuffer.IsUniform(0)) return;

		var voxelTool = new FactoryVoxelTool(outBuffer.GetVoxelTool());

		var rng = new RandomNumberGenerator();
		rng.Seed = MakeRngSeed(blockPos);

		for (var x = 0; x < outBuffer.GetSize().X; x++)
		for (var z = 0; z < outBuffer.GetSize().Z; z++)
		{
			if (rng.Randf() > 0.5f) continue;
			
			var globalPos = new Vector2(x + blockPos.X, z + blockPos.Z);
			var castPos = new Vector3(globalPos.X, blockPos.Y + outBuffer.GetSize().Y, globalPos.Y);
			var result = voxelTool.Raycast(castPos, Vector3.Down, outBuffer.GetSize().Y);

			if (result == null) continue;
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
