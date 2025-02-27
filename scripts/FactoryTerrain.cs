using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlockFactory.scripts.block_library;
using BlockFactory.scripts.blocks;
using BlockFactory.scripts.blocks.attributes;
using BlockFactory.scripts.blocks.interfaces;
using BlockFactory.scripts.player;
using Godot;

namespace BlockFactory.scripts;

public partial class FactoryTerrain : VoxelTerrain
{
	[Export] private Player player;
	[Export] private int LodBlocksRadius = 8;
	[Export] private int SimulationBlocksRadius = 8;
	
	private Vector3I prevPlayerBlock;
	private Vector3I playerBlock;

	private FactoryVoxelTool voxelTool;
	private VoxelToolTerrain terrainTool;

	public override void _Ready()
	{
		// Why does this get cast down to VoxelTool inside the method??
		terrainTool = (VoxelToolTerrain) GetVoxelTool();
		voxelTool = new FactoryVoxelTool(GetVoxelTool());
	}

	public override void _OnAreaEdited(Vector3I areaOrigin, Vector3I areaSize)
	{
	}
	
	public override void _Process(double delta)
	{
		
	}
	
	public override void _PhysicsProcess(double delta)
	{
		playerBlock = (Vector3I) (player.GlobalPosition / MeshBlockSize);
		
		var simulationBlocksAabb = new Aabb((playerBlock - Vector3I.One * SimulationBlocksRadius), Vector3.One * SimulationBlocksRadius * 2);
		
		for (var x = simulationBlocksAabb.Position.X; x < simulationBlocksAabb.End.X; x++)
		for (var y = simulationBlocksAabb.Position.Y; y < simulationBlocksAabb.End.Y; y++)
		for (var z = simulationBlocksAabb.Position.Z; z < simulationBlocksAabb.End.Z; z++)
		{
			var blockPos = new Vector3(x, y, z);
			var blockAabb = new Aabb(blockPos * MeshBlockSize, blockPos + Vector3.One * MeshBlockSize);

			if (!voxelTool.IsAreaEditable(blockAabb)) continue;
			
			terrainTool.ForEachVoxelMetadataInArea(blockAabb, Callable.From((Vector3I pos, Variant v) =>
			{
				var metadata = v.As<BlockMetadata>();
				var voxelId = voxelTool.GetVoxel(pos);
				var voxelData = FactoryData.BlockLibrary.GetBlockDataFromId((int) voxelId);
				var voxelType = FactoryData.BlockLibrary.GetTypeFromName(voxelData.Name);

				if (voxelType is not BaseFactoryBlock fb) return;

				if (voxelType is ILodBlock lodBlock)
				{
					var currentState = voxelData.Attributes[LodAttribute.Name];
					var goalState = 0;
					if (pos.DistanceSquaredTo((Vector3I) player.GlobalPosition) > LodBlocksRadius * LodBlocksRadius) goalState = 1;

					if (currentState != goalState)
					{
						voxelData.Attributes[LodAttribute.Name] = goalState;
						var id = FactoryData.BlockLibrary.GetModelWithAttributes(voxelData.Name, voxelData.Attributes);
						voxelTool.SetVoxel(id, pos);
					}
				}
			}));

		}
		
		prevPlayerBlock = playerBlock;
	}
}
