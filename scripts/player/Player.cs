using Godot;

namespace BlockFactory.scripts.player;

public partial class Player : Node3D
{
	[Export] public VoxelTerrain Terrain;
	public VoxelToolTerrain VoxelTool;

	private bool spawned = false;

	public override void _Ready()
	{
		VoxelTool = (VoxelToolTerrain) Terrain.GetVoxelTool();
	}

	public override void _PhysicsProcess(double delta)
	{
		
	}
}
