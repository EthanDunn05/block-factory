using System.Threading.Tasks;
using BlockFactory.scripts.block_library;
using Godot;
using Godot.Collections;

namespace BlockFactory.scripts.loading;

public partial class LoadingScene : Node
{
	[Export] private FactoryBlockLibrary library;
	[Export] private ShaderMaterial blockMaterial;
	[Export] private TextureRect tex;

	private Task loadingTask;

	public override void _Ready()
	{
		// Load async to avoid stopping the main thread
		loadingTask = Task.Run(Load);
	}

	public override void _Process(double delta)
	{
		if (!loadingTask.IsCompleted) return;
		// tex.Texture = BlockAtlas.Atlas;
		SceneManager.Instance.GotoScene("res://main.tscn");
	}

	private async void Load()
	{
		FactoryData.Initialize(library, blockMaterial);
		SetModelData();
	}
	
	private void SetModelData()
	{
		foreach (var type in library.Types)
		{
			if (type.BaseModel is FactoryModel model)
			{
				model.SetData();
			}

			var variants = FactoryBlockLibrary.WrapVariantData(type._VariantModelsData);

			if (variants.Length > 0)
			{
				foreach (var variant in variants)
				{
					if (variant.Model is FactoryModel fm)
					{
						fm.SetData();
					} 
				}
			}
		}
	}
}
