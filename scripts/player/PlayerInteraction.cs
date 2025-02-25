using BlockFactory.scripts.block_library;
using Godot;

namespace BlockFactory.scripts.player;

public partial class PlayerInteraction : Node
{
    private Player player;

    [Export] private Camera3D camera;
    
    public override void _Ready()
    {
        player = GetParent<Player>();
    }
    
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("place"))
        {
            var cast = player.VoxelTool.Raycast(camera.GlobalPosition, camera.GlobalBasis * Vector3.Forward);

            if (cast != null)
            {
                var placePos = cast.PreviousPosition;
                player.VoxelTool.SetVoxel(placePos, BlockAtlas.BlockLibrary.GetDefaultId("dirt_slab"));
            }
        }
        
        if (Input.IsActionJustPressed("break"))
        {
            var cast = player.VoxelTool.Raycast(camera.GlobalPosition, camera.GlobalBasis * Vector3.Forward);

            if (cast != null)
            {
                var pos = cast.Position;
                player.VoxelTool.SetVoxel(pos, BlockAtlas.BlockLibrary.GetDefaultId("air"));
            }
        }
    }
}