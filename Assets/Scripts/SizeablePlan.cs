using System.Collections.Generic;
using UnityEngine;

public class SizeablePlan : MonoBehaviour
{
    private MeshFilter filter;

    [SerializeField]
    private uint nrow = 1;

    [SerializeField]
    private uint ncol = 1;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        // FastBuild();
        OptimizeBuild();
    }

    // Slower to build, but reuse some vertex
    void OptimizeBuild()
    {
        Mesh mesh = filter.mesh;

        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        for (uint row = 0; row < nrow; ++row)
        {
            for (uint col = 0; col < ncol; ++col)
            {
                Vector3 vertex;
                int     idx;
                int[]   defaultTriangles = new int[] { 0, 0, 0, 0, 0, 0 };

                vertex = new Vector3(0.0f + col, 0.0f + row, 0.0f);
                idx    = vertices.FindIndex(0, x => x == vertex);
                if (idx == -1)
                {
                    vertices.Add(vertex);
                    idx = vertices.Count - 1;
                }
                defaultTriangles[2] = idx;
                defaultTriangles[5] = idx;

                vertex = new Vector3(1.0f + col, 0.0f + row, 0.0f);
                idx    = vertices.FindIndex(0, x => x == vertex);
                if (idx == -1)
                {
                    vertices.Add(vertex);
                    idx = vertices.Count - 1;
                }
                defaultTriangles[1] = idx;

                vertex = new Vector3(1.0f + col, 1.0f + row, 0.0f);
                idx    = vertices.FindIndex(0, x => x == vertex);
                if (idx == -1)
                {
                    vertices.Add(vertex);
                    idx = vertices.Count - 1;
                }
                defaultTriangles[0] = idx;
                defaultTriangles[4] = idx;

                vertex = new Vector3(0.0f + col, 1.0f + row, 0.0f);
                idx    = vertices.FindIndex(0, x => x == vertex);
                if (idx == -1)
                {
                    vertices.Add(vertex);
                    idx = vertices.Count - 1;
                }
                defaultTriangles[3] = idx;

                triangles.AddRange(defaultTriangles);
            }   
        }

        mesh.vertices  = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    // Fast to build but create mesh with duplicate vertex
    void FastBuild()
    {
        Mesh mesh = filter.mesh;

        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        for (uint row = 0; row < nrow; ++row)
        {
            for (uint col = 0; col < ncol; ++col)
            {
                vertices.AddRange(new Vector3[] 
                {
                    new(0.0f + col, 0.0f + row, 0.0f),
                    new(1.0f + col, 0.0f + row, 0.0f),
                    new(1.0f + col, 1.0f + row, 0.0f),
                    new(0.0f + col, 1.0f + row, 0.0f),
                });

                int offset = 4 * (int)(row * ncol + col);
                triangles.AddRange(new int[]
                {
                    2 + offset, 1 + offset, 0 + offset,
                    3 + offset, 2 + offset, 0 + offset
                });
            }   
        }

        mesh.vertices  = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }
}
