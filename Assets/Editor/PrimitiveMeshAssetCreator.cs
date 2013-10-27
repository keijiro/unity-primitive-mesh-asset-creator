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
        CreateOrReplaceAsset (CreateIcosahedronMesh (), dirPath + "/Icosahedron.asset");
        CreateOrReplaceAsset (CreateIcosahedronWireMesh (), dirPath + "/IcosahedronWire.asset");
        CreateOrReplaceAsset (CreateIcosahedronWireSolidMesh (), dirPath + "/IcosahedronWireSolid.asset");
    }

    static void CreateOrReplaceAsset (Mesh mesh, string path)
    {
        var meshAsset = AssetDatabase.LoadAssetAtPath (path, typeof(Mesh)) as Mesh;
        if (meshAsset == null) {
            meshAsset = new Mesh ();
            EditorUtility.CopySerialized (mesh, meshAsset);
            AssetDatabase.CreateAsset (meshAsset, path);
        } else {
            EditorUtility.CopySerialized (mesh, meshAsset);
            AssetDatabase.SaveAssets ();
        }
    }

    static Mesh CreateXYQuadMesh ()
    {
        var mesh = new Mesh ();

        var vertices = new Vector3 [8];
        vertices [0] = vertices [5] = new Vector3 (-1, +1, 0);
        vertices [1] = vertices [4] = new Vector3 (+1, +1, 0);
        vertices [2] = vertices [7] = new Vector3 (-1, -1, 0);
        vertices [3] = vertices [6] = new Vector3 (+1, -1, 0);
        mesh.vertices = vertices;

        var uvs = new Vector2 [8];
        uvs [0] = uvs [5] = new Vector2 (0, 0);
        uvs [1] = uvs [4] = new Vector2 (1, 0);
        uvs [2] = uvs [7] = new Vector2 (0, 1);
        uvs [3] = uvs [6] = new Vector2 (1, 1);
        mesh.uv = uvs;

        var indices = new int[] {0, 1, 2, 3, 2, 1, 4, 5, 6, 7, 6, 5};
        mesh.SetIndices (indices, MeshTopology.Triangles, 0);

        mesh.RecalculateNormals ();
        return mesh;
    }

    static Mesh CreateXYQuadWireMesh ()
    {
        var mesh = new Mesh ();

        var vertices = new Vector3 [4];
        vertices [0] = new Vector3 (-1, +1, 0);
        vertices [1] = new Vector3 (+1, +1, 0);
        vertices [2] = new Vector3 (-1, -1, 0);
        vertices [3] = new Vector3 (+1, -1, 0);
        mesh.vertices = vertices;
        
        var uvs = new Vector2 [4];
        uvs [0] = new Vector2 (0, 0);
        uvs [1] = new Vector2 (1, 0);
        uvs [2] = new Vector2 (0, 1);
        uvs [3] = new Vector2 (1, 1);
        mesh.uv = uvs;
        
        var indices = new int[] {0, 1, 3, 2, 0};
        mesh.SetIndices (indices, MeshTopology.LineStrip, 0);
        
        mesh.RecalculateNormals ();
        return mesh;
    }

    static Mesh CreateXYQuadWireSolidMesh ()
    {
        var mesh = new Mesh ();
        mesh.subMeshCount = 2;
        
        var vertices = new Vector3 [8];
        vertices [0] = vertices [5] = new Vector3 (-1, +1, 0);
        vertices [1] = vertices [4] = new Vector3 (+1, +1, 0);
        vertices [2] = vertices [7] = new Vector3 (-1, -1, 0);
        vertices [3] = vertices [6] = new Vector3 (+1, -1, 0);
        mesh.vertices = vertices;
        
        var uvs = new Vector2 [8];
        uvs [0] = uvs [5] = new Vector2 (0, 0);
        uvs [1] = uvs [4] = new Vector2 (1, 0);
        uvs [2] = uvs [7] = new Vector2 (0, 1);
        uvs [3] = uvs [6] = new Vector2 (1, 1);
        mesh.uv = uvs;
        
        var indices = new int[] {0, 1, 2, 3, 2, 1, 4, 5, 6, 7, 6, 5};
        mesh.SetIndices (indices, MeshTopology.Triangles, 0);

        indices = new int[] {0, 1, 3, 2, 0};
        mesh.SetIndices (indices, MeshTopology.LineStrip, 1);

        mesh.RecalculateNormals ();
        return mesh;
    }

    static Mesh CreateIcosahedronMesh ()
    {
        var mesh = new Mesh ();

        var t = (1.0f + Mathf.Sqrt (5.0f)) / 2.0f;

        var vertices = new Vector3[] {
            new Vector3 (-1, +t, 0).normalized,
            new Vector3 (+1, +t, 0).normalized,
            new Vector3 (-1, -t, 0).normalized,
            new Vector3 (+1, -t, 0).normalized,
        //
            new Vector3 (0, -1, +t).normalized,
            new Vector3 (0, +1, +t).normalized,
            new Vector3 (0, -1, -t).normalized,
            new Vector3 (0, +1, -t).normalized,
        //
            new Vector3 (+t, 0, -1).normalized,
            new Vector3 (+t, 0, +1).normalized,
            new Vector3 (-t, 0, -1).normalized,
            new Vector3 (-t, 0, +1).normalized
        };

        var triangles = new int[] {
            0, 11, 5,
            0, 5, 1,
            0, 1, 7,
            0, 7, 10,
            0, 10, 11,
        //
            1, 5, 9,
            5, 11, 4,
            11, 10, 2,
            10, 7, 6,
            7, 1, 8,
        //
            3, 9, 4,
            3, 4, 2,
            3, 2, 6,
            3, 6, 8,
            3, 8, 9,
        //
            4, 9, 5,
            2, 4, 11,
            6, 2, 10,
            8, 6, 7,
            9, 8, 1
        };

        // Convert to split vertices.
        var vertices2 = new Vector3[triangles.Length];
        for (var i = 0; i < triangles.Length; i++) {
            vertices2 [i] = vertices [triangles [i]];
        }

        var triangles2 = new int[triangles.Length];
        for (var i = 0; i < triangles.Length; i++) {
            triangles2 [i] = i;
        }

        mesh.vertices = vertices2;
        mesh.triangles = triangles2;
        mesh.uv = new Vector2[triangles.Length];

        mesh.Optimize ();
        mesh.RecalculateNormals ();
        return mesh;
    }

    static Mesh CreateIcosahedronWireMesh ()
    {
        var mesh = new Mesh ();
        
        var t = (1.0f + Mathf.Sqrt (5.0f)) / 2.0f;
        
        mesh.vertices = new Vector3[] {
            new Vector3 (-1, +t, 0).normalized,
            new Vector3 (+1, +t, 0).normalized,
            new Vector3 (-1, -t, 0).normalized,
            new Vector3 (+1, -t, 0).normalized,
        //
            new Vector3 (0, -1, +t).normalized,
            new Vector3 (0, +1, +t).normalized,
            new Vector3 (0, -1, -t).normalized,
            new Vector3 (0, +1, -t).normalized,
        //
            new Vector3 (+t, 0, -1).normalized,
            new Vector3 (+t, 0, +1).normalized,
            new Vector3 (-t, 0, -1).normalized,
            new Vector3 (-t, 0, +1).normalized
        };

        mesh.SetIndices (new int[] {
            0, 1,
            0, 5,
            0, 7,
            0, 10,
            0, 11,
            1, 5,
            1, 7,
            1, 8,
            1, 9,
            2, 3,
            2, 4,
            2, 6,
            2, 10,
            2, 11,
            3, 4,
            3, 6,
            3, 8,
            3, 9,
            4, 5,
            4, 9,
            4, 11,
            5, 9,
            5, 11,
            6, 7,
            6, 8,
            6, 10,
            7, 8, 
            7, 10,
            8, 9,
            10, 11
        }, MeshTopology.Lines, 0);

        mesh.uv = new Vector2[12];
        
        mesh.Optimize ();
        mesh.RecalculateNormals ();
        return mesh;
    }

    static Mesh CreateIcosahedronWireSolidMesh ()
    {
        var mesh = new Mesh ();
        mesh.subMeshCount = 2;

        var t = (1.0f + Mathf.Sqrt (5.0f)) / 2.0f;
        
        var vertices = new Vector3[] {
            new Vector3 (-1, +t, 0).normalized,
            new Vector3 (+1, +t, 0).normalized,
            new Vector3 (-1, -t, 0).normalized,
            new Vector3 (+1, -t, 0).normalized,
        //
            new Vector3 (0, -1, +t).normalized,
            new Vector3 (0, +1, +t).normalized,
            new Vector3 (0, -1, -t).normalized,
            new Vector3 (0, +1, -t).normalized,
        //
            new Vector3 (+t, 0, -1).normalized,
            new Vector3 (+t, 0, +1).normalized,
            new Vector3 (-t, 0, -1).normalized,
            new Vector3 (-t, 0, +1).normalized
        };
        
        var triangles = new int[] {
            0, 11, 5,
            0, 5, 1,
            0, 1, 7,
            0, 7, 10,
            0, 10, 11,
        //
            1, 5, 9,
            5, 11, 4,
            11, 10, 2,
            10, 7, 6,
            7, 1, 8,
        //
            3, 9, 4,
            3, 4, 2,
            3, 2, 6,
            3, 6, 8,
            3, 8, 9,
        //
            4, 9, 5,
            2, 4, 11,
            6, 2, 10,
            8, 6, 7,
            9, 8, 1
        };

        var lines = new int[] {
            0, 1,
            0, 5,
            0, 7,
            0, 10,
            0, 11,
            1, 5,
            1, 7,
            1, 8,
            1, 9,
            2, 3,
            2, 4,
            2, 6,
            2, 10,
            2, 11,
            3, 4,
            3, 6,
            3, 8,
            3, 9,
            4, 5,
            4, 9,
            4, 11,
            5, 9,
            5, 11,
            6, 7,
            6, 8,
            6, 10,
            7, 8, 
            7, 10,
            8, 9,
            10, 11
        };

        // Convert to split vertices.
        var vertices2 = new Vector3[triangles.Length];
        for (var i = 0; i < triangles.Length; i++) {
            vertices2 [i] = vertices [triangles [i]];
        }
        
        var triangles2 = new int[triangles.Length];
        for (var i = 0; i < triangles.Length; i++) {
            triangles2 [i] = i;
        }
        
        mesh.vertices = vertices2;
        mesh.SetIndices (triangles2, MeshTopology.Triangles, 0);
        mesh.uv = new Vector2[triangles.Length];

        // Calculate the normal vectore before setting the lines.
        mesh.RecalculateNormals ();

        // Look up the proper vertex index from the triangle array.
        for (var i = 0; i < lines.Length; i++) {
            var vi = lines [i];
            for (var tvi = 0; tvi < triangles.Length; tvi++) {
                if (triangles [tvi] == vi) {
                    lines [i] = tvi;
                    break;
                }
            }
        }

        mesh.SetIndices (lines, MeshTopology.Lines, 1);

        mesh.Optimize ();
        return mesh;
    }
}
