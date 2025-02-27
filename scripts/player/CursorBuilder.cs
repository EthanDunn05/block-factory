using Godot;
using Godot.Collections;

namespace BlockFactory.scripts.player;

public class CursorBuilder
{
    private static readonly Vector3[] VertexPositions =
    [
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(1, 0, 1),
        new Vector3(0, 0, 1),
        new Vector3(0, 1, 0),
        new Vector3(1, 1, 0),
        new Vector3(1, 1, 1),
        new Vector3(0, 1, 1),
    ];

    private static readonly int[] Indices =
    [
        0, 1,
        1, 2,
        2, 3,
        3, 0,
        
        4, 5,
        5, 6,
        6, 7,
        7, 4,
        
        0, 4,
        1, 5,
        2, 6,
        3, 7,
    ];
    
    public static ArrayMesh BuildWireframe(Array<Aabb> boundingBoxes)
    {
        var mesh = new ArrayMesh();
        foreach (var aabb in boundingBoxes)
        {
            var arrays = new Array();
            arrays.Resize((int) Mesh.ArrayType.Max);

            arrays[(int) Mesh.ArrayType.Vertex] = MakeVertexPositions(aabb);
            arrays[(int) Mesh.ArrayType.Index] = Indices;
            
            mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Lines, arrays);
        }

        return mesh;
    }
    
    private static Vector3[] MakeVertexPositions(Aabb boundingBox)
    {
        var modifiedPositions = new Vector3[VertexPositions.Length];
        
        for (var i = 0; i < VertexPositions.Length; i++)
        {
            var orig = VertexPositions[i];
            modifiedPositions[i] = orig * boundingBox.Size + boundingBox.Position;
        }

        return modifiedPositions;
    }
}