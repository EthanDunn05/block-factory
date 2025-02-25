using System.Collections.Generic;
using Godot;

namespace BlockFactory.scripts.block_library;

public abstract partial class FactoryBlockType : VoxelBlockyType
{
    public abstract Texture2D[]? Textures { get; }

    public abstract void ApplyModel();

    protected void MoveUvs(ref ArrayMesh mesh, Vector2I tilePos, Vector2 textureSize)
    {
        var mdt = new MeshDataTool();
        mdt.CreateFromSurface(mesh, 0);
        for (var i = 0; i < mdt.GetVertexCount(); i++)
        {
            var uv = mdt.GetVertexUV(i);
            
            // Scale to tile size
            uv *= textureSize / BlockAtlas.AtlasSize;
            
            // Inset uvs to avoid flickering
            // var insetPos = new Vector2(0.005f, 0.005f);
            // uv *= 1f - 0.0025f;
            
            mdt.SetVertexUV(i, uv + tilePos);
        }

        mesh.ClearSurfaces();
        mdt.CommitToSurface(mesh);
    }
}