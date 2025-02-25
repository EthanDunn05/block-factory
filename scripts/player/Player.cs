using Godot;

namespace BlockFactory.scripts.player;

public partial class Player : CharacterBody3D
{
	[Export] public VoxelTerrain Terrain;
	public VoxelTool VoxelTool;

	public override void _Ready()
	{
		VoxelTool = Terrain.GetVoxelTool();
		VoxelTool.Channel = VoxelBuffer.ChannelId.ChannelType;
	}
}
