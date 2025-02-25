using Godot;

namespace BlockFactory.scripts;


public partial class SceneManager : Node
{
    public static SceneManager Instance;
    public Node CurrentScene;

    public override void _Ready()
    {
        Instance = this;
        
        var root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
    }
    
    public void GotoScene(string path)
    {
        CallDeferred(nameof(DeferredGotoScene), path);
    }
    
    
    public void DeferredGotoScene(string path)
    {
        CurrentScene.Free();
        var nextScene = (PackedScene)GD.Load(path);
        CurrentScene = nextScene.Instantiate();
        GetTree().GetRoot().AddChild(CurrentScene);
        GetTree().SetCurrentScene(CurrentScene);
    }
}