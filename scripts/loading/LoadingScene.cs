using System.Threading.Tasks;
using BlockFactory.scripts.block_library;
using Godot;

namespace BlockFactory.scripts.loading;

public partial class LoadingScene : Node
{
	[Export] private FactoryBlockLibrary library;
	[Export] private ShaderMaterial blockMaterial;
	[Export] private TextureRect tex;

	private Task loadingTask;

	public override void _Ready()
	{
		loadingTask = Task.WhenAll([
			Task.Run(() => BlockAtlas.Initialize(library, blockMaterial))
		]);
	}

	public override void _Process(double delta)
	{
		if (!loadingTask.IsCompleted) return;
		// tex.Texture = BlockAtlas.Atlas;
		SceneManager.Instance.GotoScene("res://main.tscn");
	}
}
