using Godot;

namespace BlockFactory.scripts.player;

public partial class Player : Node3D
{
	[Export] public VoxelTerrain Terrain;
	public FactoryVoxelTool VoxelTool;

	private bool spawned = false;

	public override void _Ready()
	{
		VoxelTool = new FactoryVoxelTool(Terrain.GetVoxelTool());
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Terrain == null) return;
		
		// if (!spawned && VoxelTool.IsAreaEditable(new Aabb(GlobalPosition - new Vector3(5, 5, 5), new Vector3(10, 10, 10))))
		// {
		// 	var spawnStart = new Vector3(GlobalPosition.X, 512, GlobalPosition.Y);
		// 	var cast = VoxelTool.Raycast(spawnStart, Vector3.Down, 512f);
		// 	if (cast != null)
		// 	{
		// 		GlobalPosition = cast.PreviousPosition;
		// 		spawned = true;
		// 	}
		// }
	}
}
