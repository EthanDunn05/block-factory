using System.Linq;
using BlockFactory.scripts.block_library;
using BlockFactory.scripts.blocks.attributes;
using Godot;
using Godot.Collections;

namespace BlockFactory.scripts.player;

public partial class PlayerInteraction : Node
{
    private Player player;

    [Export] private Camera3D camera;


    private VoxelRaycastResult? pointerResult;

    public override void _Ready()
    {
        player = GetParent<Player>();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("place"))
        {
            if (pointerResult != null)
            {
                var placePos = pointerResult.PreviousPosition;
                player.VoxelTool.SetVoxel(FactoryData.BlockLibrary.GetSingleAttributeId(
                    "dirt_slab",
                    SlabAttribute.Top
                ), placePos);
            }
        }

        if (Input.IsActionJustPressed("break"))
        {
            if (pointerResult != null)
            {
                var pos = pointerResult.Position;
                player.VoxelTool.SetVoxel(FactoryData.BlockLibrary.GetDefaultId("air"), pos);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        pointerResult = CastPointer(); // Only once per physics update :)
    }

    private VoxelRaycastResult? CastPointer()
    {
        var cast = player.VoxelTool.Raycast(camera.GlobalPosition, camera.GlobalBasis * Vector3.Forward, 5f);
        return cast;
    }
}