using System.Linq;
using BlockFactory.scripts.block_library;
using Godot;

namespace BlockFactory.scripts.player;

public partial class Cursor : MeshInstance3D
{
    private VoxelBlockyModel? currentModel;
    
    public void UpdateHovered(VoxelRaycastResult? pointerCast, Player player)
    {
        if (pointerCast != null)
        {
            Position = pointerCast.Position;
            
            // Check out this labyrinth of data
            var hoveredId = player.VoxelTool.GetVoxel(pointerCast.Position);
            var hoveredData = FactoryData.BlockLibrary.GetBlockDataFromId((int) hoveredId);
            var hoveredType = FactoryData.BlockLibrary.GetTypeFromName(hoveredData.Name);
            var variants = FactoryData.BlockLibrary.GetVariants(hoveredData.Name);

            var model = hoveredType.BaseModel;
            if (variants.Length > 0)
            {
                model = variants.First(data => data.Attributes.RecursiveEqual(hoveredData.Attributes)).Model;
            }

            if (currentModel != model)
            {
                Mesh = CursorBuilder.BuildWireframe(model.CollisionAabbs);
                currentModel = model;
            }
            
            Show();
        }
        else
        {
            Hide();
        }
    }
}