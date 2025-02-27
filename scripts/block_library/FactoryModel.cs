using Godot;

namespace BlockFactory.scripts.block_library;

[GlobalClass]
public partial class FactoryModel : VoxelBlockyModelMesh
{
    [Export] private Texture2D texture;
    
    public void SetData()
    {
        SetMeshCollisionEnabled(0, false);

        if (GetMaterialOverride(0) != null) return;
        var material = (ShaderMaterial) FactoryData.BlockMaterial.Duplicate();
        material.SetShaderParameter("texture_albedo", texture);
        material.SetShaderParameter("albedo_texture_size", texture.GetSize());
        SetMaterialOverride(0, material);
    }
}