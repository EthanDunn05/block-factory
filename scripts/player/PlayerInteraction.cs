using System.Linq;
using BlockFactory.scripts.block_library;
using BlockFactory.scripts.blocks;
using BlockFactory.scripts.blocks.attributes;
using Godot;
using Godot.Collections;

namespace BlockFactory.scripts.player;

public partial class PlayerInteraction : Node
{
    private Player player;

    [Export] private Camera3D camera;
    [Export] private Material cursorMaterial;
    private Cursor cursor;

    private VoxelRaycastResult? pointerResult;

    public override void _Ready()
    {
        player = GetParent<Player>();

        cursor = new Cursor();
        cursor.MaterialOverride = cursorMaterial;
        cursor.SetScale(Vector3.One * 1.001f);
        player.Terrain.AddChild(cursor);
        
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("place") && pointerResult != null)
        {
            var pos = (Vector3I) (pointerResult.Position + pointerResult.Normal);
                
            var id = player.TerrainTool.GetVoxelId(pos);
            var type = FactoryData.BlockLibrary.GetTypeFromId((int) id);

            var clickResult = InteractionResult.Continue;
            if (type is BaseVoxelType clicking)
            {
                clickResult = clicking.OnRightClick(player, pointerResult.Position, pointerResult.Normal, player.Terrain);
            }

            if (clickResult == InteractionResult.Continue)
            {
                player.TerrainTool.SetVoxel(pos, FactoryData.BlockLibrary.GetDefaultId("dirt_slab"));
                var placedType = player.TerrainTool.GetVoxelType(pos);
                
                if (placedType is BaseVoxelType vt)
                {
                    vt.OnPlaced(player, pos, pointerResult.Normal, player.Terrain);
                }
            }
        }

        if (Input.IsActionJustPressed("break") && pointerResult != null)
        {
            var pos = pointerResult.Position;
                
            var id = player.TerrainTool.GetVoxelId(pos);
            var type = FactoryData.BlockLibrary.GetTypeFromId((int) id);

            var clickResult = InteractionResult.Continue;
            if (type is BaseVoxelType clicking)
            {
                clickResult = clicking.OnLeftClick(player, pointerResult.Position, pointerResult.Normal, player.Terrain);
            }
                
            if (clickResult == InteractionResult.Continue)
            {
                if (type is BaseVoxelType vt)
                {
                    vt.OnBroken(player, pos, player.Terrain);
                }

                player.TerrainTool.SetVoxel(pos, FactoryData.BlockLibrary.GetDefaultId("air"));
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        pointerResult = CastPointer(); // Only once per physics update :)
        
        cursor.UpdateHovered(pointerResult, player);
    }

    private VoxelRaycastResult? CastPointer()
    {
        var cast = player.TerrainTool.Raycast(camera.GlobalPosition, camera.GlobalBasis * Vector3.Forward, 5f);
        return cast;
    }
}