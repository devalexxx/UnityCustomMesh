using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    private MeshFilter filter;

    [SerializeField]
    private float radius = 1.0f;

    [SerializeField]
    private float height = 3.0f;

    [SerializeField]
    private uint nmeridians = 16;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();

        if (nmeridians < 3)
            nmeridians = 3;

        if (height < 0)
            height = Mathf.Abs(height);
    }

    void Start()
    {
        Mesh mesh = filter.mesh;

        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        // Vertical faces
        {
            float theta = 2 * Mathf.PI / nmeridians;

            Vector3 firstBotPoint = new(
                radius * Mathf.Cos(0.0f),
                -height / 2,
                radius * Mathf.Sin(0.0f)
            );
            Vector3 firstTopPoint = new(
                radius * Mathf.Cos(0.0f),
                height / 2,
                radius * Mathf.Sin(0.0f)
            );

            vertices.Add(firstBotPoint);
            vertices.Add(firstTopPoint);

            int firstBotPointIndex = 0;
            int firstTopPointIndex = 1;

            int botPointIndex = firstBotPointIndex;
            int topPointIndex = firstTopPointIndex;

            for (int i = (int)(nmeridians - 1); i > 0; i--)
            {
                float angle = i * theta;

                Vector3 nextBotPoint = new(
                    radius * Mathf.Cos(angle),
                    -height / 2,
                    radius * Mathf.Sin(angle)
                );
                Vector3 nextTopPoint = new(
                    radius * Mathf.Cos(angle),
                    height / 2,
                    radius * Mathf.Sin(angle)
                );

                vertices.Add(nextBotPoint);
                vertices.Add(nextTopPoint);

                int nextTopPointIndex = vertices.Count - 1;
                int nextBotPointIndex = nextTopPointIndex - 1;

                triangles.AddRange(new int[] {
                    topPointIndex, botPointIndex, nextBotPointIndex,
                    nextTopPointIndex, topPointIndex, nextBotPointIndex
                });

                topPointIndex = nextTopPointIndex;
                botPointIndex = nextBotPointIndex;
            }

            triangles.AddRange(new int[] {
                topPointIndex, botPointIndex, firstBotPointIndex,
                firstTopPointIndex, topPointIndex, firstBotPointIndex
            });
        }

        // Preshot bot top and bottom center index to form triangles without interfere with the loop
        int centerTopIndex = vertices.Count;
        int centerBotIndex = vertices.Count + 1;

        // Bottom face
        {
            int startBotIndex   = 0;
            int currentBotIndex = startBotIndex;
            // From last element - 1 to 0
            for (int i = vertices.Count - 2; i >= startBotIndex + 2; i -= 2)
            {
                triangles.AddRange(new int[] { centerBotIndex, currentBotIndex, i });
                currentBotIndex = i;
            }
            // Because it's a circle, first vertex is also the last
            triangles.AddRange(new int[] { centerBotIndex, currentBotIndex, startBotIndex });
        }

        // Top face
        {
            int startTopIndex   = 1;
            int currentTopIndex = startTopIndex;
            // From 1 to last element
            for (int i = startTopIndex + 2; i < vertices.Count; i += 2)
            {
                triangles.AddRange(new int[] { centerTopIndex, currentTopIndex, i });
                currentTopIndex = i;
            }
            // Because it's a circle, first vertex is also the last
            triangles.AddRange(new int[] { centerTopIndex, currentTopIndex, startTopIndex });
        }

        // Finally add both top and bottom center vertex
        vertices.Add(new Vector3(0.0f,  height / 2.0f, 0.0f));
        vertices.Add(new Vector3(0.0f, -height / 2.0f, 0.0f));

        mesh.vertices  = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

}
