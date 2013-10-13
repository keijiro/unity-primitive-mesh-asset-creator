using UnityEngine;
using UnityEditor;

public static class PrimitiveMeshAssetCreator
{
    [MenuItem("Assets/Create/Primitive Meshes")]
    static void CreatePrimitiveMeshes ()
    {
        var dirName = "Primitive Meshes";
        var dirPath = "Assets/" + dirName;

        if (!System.IO.Directory.Exists (dirPath)) {
            AssetDatabase.CreateFolder ("Assets", dirName);
        }

        CreateOrReplaceAsset (CreateXYQuadMesh (), dirPath + "/XYQuad.asset");
        CreateOrReplaceAsset (CreateXYQuadWireMesh (), dirPath + "/XYQuadWire.asset");
        CreateOrReplaceAsset (CreateXYQuadWireSolidMesh (), dirPath + "/XYQuadWireSolid.asset");
    }

    static void CreateOrReplaceAsset (Mesh mesh, string path)
    {
        var meshAsset = AssetDatabase.LoadAssetAtPath (path, typeof(Mesh)) as Mesh;
        if (meshAsset == null) {
            meshAsset = new Mesh ();
            EditorUtility.CopySerialized (mesh, meshAsset);
            AssetDatabase.CreateAsset (mesh, path);
        } else {
            EditorUtility.CopySerialized (mesh, meshAsset);
            AssetDatabase.SaveAssets ();
        }
    }

    static Mesh CreateXYQuadMesh ()
    {
        var mesh = new Mesh ();

        var vertices = new Vector3 [8];
        vertices [0] = new Vector3 (-1, +1, 0);
        vertices [1] = new Vector3 (+1, +1, 0);
        vertices [2] = new Vector3 (+1, -1, 0);
        vertices [3] = new Vector3 (-1, -1, 0);
        vertices [4] = new Vector3 (+1, +1, 0);
        vertices [5] = new Vector3 (-1, +1, 0);
        vertices [6] = new Vector3 (-1, -1, 0);
        vertices [7] = new Vector3 (+1, -1, 0);
        mesh.vertices = vertices;

        var uvs = new Vector2 [8];
        uvs [0] = uvs [4] = new Vector2 (-1, -1);
        uvs [1] = uvs [5] = new Vector2 (+1, -1);
        uvs [2] = uvs [6] = new Vector2 (+1, +1);
        uvs [3] = uvs [7] = new Vector2 (-1, +1);
        mesh.uv = uvs;

        var indices = new int[8];
        for (var i = 0; i < 8; i++) {
            indices [i] = i;
        }
        mesh.SetIndices (indices, MeshTopology.Quads, 0);

        mesh.RecalculateNormals ();
        return mesh;
    }

    static Mesh CreateXYQuadWireMesh ()
    {
        var mesh = new Mesh ();

        var vertices = new Vector3 [4];
        vertices [0] = new Vector3 (-1, +1, 0);
        vertices [1] = new Vector3 (+1, +1, 0);
        vertices [2] = new Vector3 (+1, -1, 0);
        vertices [3] = new Vector3 (-1, -1, 0);
        mesh.vertices = vertices;
        
        var uvs = new Vector2 [4];
        uvs [0] = new Vector2 (-1, -1);
        uvs [1] = new Vector2 (+1, -1);
        uvs [2] = new Vector2 (+1, +1);
        uvs [3] = new Vector2 (-1, +1);
        mesh.uv = uvs;
        
        var indices = new int[5];
        for (var i = 0; i < 5; i++) {
            indices [i] = i & 3;
        }
        mesh.SetIndices (indices, MeshTopology.LineStrip, 0);
        
        mesh.RecalculateNormals ();
        return mesh;
    }

    static Mesh CreateXYQuadWireSolidMesh ()
    {
        var mesh = new Mesh ();
        mesh.subMeshCount = 2;
        
        var vertices = new Vector3 [8];
        vertices [0] = new Vector3 (-1, +1, 0);
        vertices [1] = new Vector3 (+1, +1, 0);
        vertices [2] = new Vector3 (+1, -1, 0);
        vertices [3] = new Vector3 (-1, -1, 0);
        vertices [4] = new Vector3 (+1, +1, 0);
        vertices [5] = new Vector3 (-1, +1, 0);
        vertices [6] = new Vector3 (-1, -1, 0);
        vertices [7] = new Vector3 (+1, -1, 0);
        mesh.vertices = vertices;
        
        var uvs = new Vector2 [8];
        uvs [0] = uvs [4] = new Vector2 (-1, -1);
        uvs [1] = uvs [5] = new Vector2 (+1, -1);
        uvs [2] = uvs [6] = new Vector2 (+1, +1);
        uvs [3] = uvs [7] = new Vector2 (-1, +1);
        mesh.uv = uvs;
        
        var indices = new int[8];
        for (var i = 0; i < 8; i++) {
            indices [i] = i;
        }
        mesh.SetIndices (indices, MeshTopology.Quads, 0);

        indices = new int[5];
        for (var i = 0; i < 5; i++) {
            indices [i] = i & 3;
        }
        mesh.SetIndices (indices, MeshTopology.LineStrip, 1);

        mesh.RecalculateNormals ();
        return mesh;
    }
}