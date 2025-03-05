using System.Linq;
using BlockFactory.scripts.block_library;
using BlockFactory.scripts.blocks;
using BlockFactory.scripts.blocks.attributes;
using BlockFactory.scripts.blocks.interfaces;
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
        if (Input.IsActionJustPressed("place"))
        {
            TryPlaceVoxel();
        }

        if (Input.IsActionJustPressed("break"))
        {
            TryBreakVoxel();
        }
    }

    private void TryPlaceVoxel()
    {
        if (pointerResult == null) return;

        var pos = (Vector3I) (pointerResult.Position + pointerResult.Normal);

        var id = player.TerrainTool.GetVoxelId(pos);
        var type = FactoryData.BlockLibrary.GetTypeFromId((int) id);

        if (type is BaseVoxelType clickedVt)
        {
            var res = clickedVt.OnRightClick(player, pointerResult.Position, pointerResult.Normal, player.Terrain);
            if (res == InteractionResult.Stop) return;
        }

        // Place logic
        if (player.TerrainTool.GetVoxelId(pos) != (int) FactoryData.BlockLibrary.GetDefaultId("air"))
        {
            return;
        }

        var placeType = FactoryData.BlockLibrary.GetTypeFromName("dirt_slab");
        var placeId = FactoryData.BlockLibrary.GetDefaultId("dirt_slab");

        if (placeType is ICustomPlacementId customPlacementId)
        {
            placeId = customPlacementId.GetId(player, pos, pointerResult.Normal, player.Terrain);
        }

        var playerBox = player.CollisionBox;
        playerBox.Position += player.Position;

        var voxelBox = player.TerrainTool.GetVoxelCollision((int) placeId);
        for (var i = 0; i < voxelBox.Count; i++)
        {
            var aabb = voxelBox[i];
            aabb.Position += pos;
            voxelBox[i] = aabb;
        }

        if (voxelBox.Any(aabb => aabb.Intersects(playerBox)))
        {
            return;
        }
        
        player.TerrainTool.SetVoxel(pos, placeId);
        var placedType = player.TerrainTool.GetVoxelType(pos);

        if (placedType is BaseVoxelType placedVt)
        {
            placedVt.OnPlaced(player, pos, pointerResult.Normal, player.Terrain);
        }
    }

    private void TryBreakVoxel()
    {
        if (pointerResult == null) return;

        var pos = pointerResult.Position;

        var id = player.TerrainTool.GetVoxelId(pos);
        var type = FactoryData.BlockLibrary.GetTypeFromId((int) id);

        if (type is BaseVoxelType clicking)
        {
            var res = clicking.OnLeftClick(player, pointerResult.Position, pointerResult.Normal, player.Terrain);
            if (res == InteractionResult.Stop) return;
        }

        // Break logic
        if (type is BaseVoxelType vt)
        {
            vt.OnBroken(player, pos, player.Terrain);
        }

        player.TerrainTool.SetVoxel(pos, FactoryData.BlockLibrary.GetDefaultId("air"));
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