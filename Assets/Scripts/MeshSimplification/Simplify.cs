using System.Collections.Generic;
using UnityEngine;

public class Simplify : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _filter;

    [SerializeField]
    private string _path;

    [SerializeField]
    private float _epsilon;

    int GetIndex(int x, int y, int z, int n)
    {
        return z * n * n + y * n + x;
    }

    void Start()
    {
        Mesh            mesh   = _filter.mesh;
        MeshLoader.Mesh loaded = MeshLoader.LoadOFF(_path);

        int nAxisPartition = (int)(2.0 / _epsilon);

        Dictionary<Vector3Int, List<int>> dict = new();
        for (int i = 0; i < loaded.vertices.Length; ++i)
        {
            int x = (int)(loaded.vertices[i].x / _epsilon);
            int y = (int)(loaded.vertices[i].y / _epsilon);
            int z = (int)(loaded.vertices[i].z / _epsilon);
            Vector3Int key = new(x, y, z);
            if (!dict.TryGetValue(key, out var value))
            {
                value = new();
                dict.Add(key, value);
            }
            else
            {
                value.Add(i);
            }
        }

        foreach (var item in dict)
        {
            Vector3 edge = new(0, 0, 0);
            for (int i = 0; i < item.Value.Count; i++)
            {
                edge += loaded.vertices[item.Value[i]];
            }
            edge /= item.Value.Count;

            for (int i = 0; i < item.Value.Count; i++)
            {
                loaded.vertices[item.Value[i]] = edge;
            }
        }

        mesh.vertices  = loaded.vertices;
        mesh.normals   = loaded.normals;
        mesh.triangles = loaded.triangles;
    }

}
