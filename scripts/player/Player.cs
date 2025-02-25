using Godot;

namespace BlockFactory.scripts.player;

public partial class Player : Node3D
{
	[Export] public VoxelTerrain Terrain;
	public VoxelTool VoxelTool;

	public override void _Ready()
	{
		VoxelTool = Terrain.GetVoxelTool();
		VoxelTool.Channel = VoxelBuffer.ChannelId.ChannelType;
	}
}
