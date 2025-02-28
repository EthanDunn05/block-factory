using Godot;

namespace BlockFactory.scripts.player;

public partial class Player : Node3D
{
	[Export] public FactoryTerrain Terrain;
	public FactoryTerrainTool TerrainTool;

	private bool spawned = false;

	public override void _Ready()
	{
		TerrainTool = new FactoryTerrainTool(Terrain);
	}

	public override void _PhysicsProcess(double delta)
	{
		
	}
}
