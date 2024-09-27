using System.Collections.Generic;
using UnityEngine;

public class SizeablePlan : MonoBehaviour
{
    private MeshFilter filter;

    [SerializeField]
    private uint nRow = 1;

    [SerializeField]
    private uint nCol = 1;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();

        if (nCol <= 0)
        {
            Debug.LogWarning("SizeablePlan::nCol must be greater than 0");;
            nCol = 1;
        }

        if (nRow <= 0)
        {
            Debug.LogWarning("SizeablePlan::nRow must be greater than 0");;
            nRow = 1;
        }
        
    }

    void Start()
    {
        Mesh mesh = filter.mesh;

        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        int nVertexPerRow = (int)(nCol + 1);
        int nVertexPerCol = (int)(nRow + 1);

        {
            // Predefined first line of vertices
            for (uint col = 0; col < nVertexPerRow; ++col)
            {
                vertices.Add(new(0.0f + col, 0.0f, 0.0f));
            }

            for (int row = 1; row < nVertexPerCol; ++row)
            {
                int topPointIndex = (row - 1) * nVertexPerRow;

                vertices.Add(new(0.0f, 0.0f + row, 0.0f));
                int botPointIndex = vertices.Count - 1;

                for (int col = 1; col < nVertexPerRow; ++col)
                {
                    vertices.Add(new(0.0f + col, 0.0f + row, 0.0f));

                    int nextTopPointIndex = topPointIndex + 1;
                    int nextBotPointIndex = vertices.Count - 1;

                    triangles.AddRange(new int[] {
                        topPointIndex, botPointIndex, nextBotPointIndex,
                        nextTopPointIndex, topPointIndex, nextBotPointIndex
                    });

                    topPointIndex = nextTopPointIndex;
                    botPointIndex = nextBotPointIndex;
                }   
            }

        }

        Debug.Log(vertices.Count);
        Debug.Log(triangles.Count / 3);

        mesh.vertices  = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }
}
