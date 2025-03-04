using BlockFactory.scripts.player;
using BlockFactory.scripts.serialization;
using Godot;

namespace BlockFactory.scripts;

public partial class FactoryTerrain : VoxelTerrain
{
	[Export] public Player Player;	
	
	public FactoryTerrainTool TerrainTool;
	private bool savePressed;

	private FactoryStream factoryStream;
	
	public override void _OnAreaEdited(Vector3I areaOrigin, Vector3I areaSize)
	{
		base._OnAreaEdited(areaOrigin, areaSize);
	}

	public override void _Ready()
	{
		TerrainTool = new FactoryTerrainTool(this);

		factoryStream.LoadPlayer(Player);
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsPhysicalKeyPressed(Key.T) && !savePressed)
		{
			SaveModifiedBlocks();
			savePressed = true;
		}
		else
		{
			savePressed = false;
		}
	}

	public override void _EnterTree()
	{
		factoryStream = new FactoryStream();
		Stream = factoryStream;
	}

	public override void _ExitTree()
	{
		SaveModifiedBlocks();
		factoryStream.SavePlayer(Player);
	}
}
