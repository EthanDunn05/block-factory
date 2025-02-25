using Godot;
using Godot.Collections;

namespace BlockFactory.scripts.block_library;

[GlobalClass]
public partial class SingleTextureMeshBlock : FactoryBlockType
{
    [Export] private Texture2D texture;
    [Export] private ArrayMesh mesh;
    [Export] private Array<Aabb> collision = [new Aabb(Vector3.Zero, Vector3.One)];
    public override Texture2D[]? Textures => [texture];
    
    public override void ApplyModel()
    {
        var model = new VoxelBlockyModelMesh()
        {
            Mesh = mesh,
        };
        model.CollisionAabbs = collision;
        BaseModel = model;

        var material = (ShaderMaterial) BlockAtlas.BlockMaterial.Duplicate();
        material.SetShaderParameter("texture_albedo", texture);
        material.SetShaderParameter("albedo_texture_size", texture.GetSize());
        BaseModel.SetMaterialOverride(0, material);
    }
}