using Godot;

namespace BlockFactory.scripts;

public partial class FactoryTerrain : VoxelTerrain
{
	public FactoryTerrainTool TerrainTool;
	
	public override void _OnAreaEdited(Vector3I areaOrigin, Vector3I areaSize)
	{
		base._OnAreaEdited(areaOrigin, areaSize);
	}

	public override void _Ready()
	{
		TerrainTool = new FactoryTerrainTool(this);
	}
}
