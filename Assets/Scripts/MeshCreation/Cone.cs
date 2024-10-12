using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    private MeshFilter filter;

    [SerializeField]
    private float radius = 1.0f;

    [SerializeField]
    private float height = 3.0f;

    [SerializeField]
    private uint nMeridians = 16;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();

        if (nMeridians < 3)
        {
            Debug.LogWarning("Cone::nMeridians must be greater than 3");
            nMeridians = 3;
        }

        if (height < 0)
        {
            Debug.LogWarning("Cone::height must be positive");
            height = Mathf.Abs(height);
        }

        if (radius < 0)
        {
            Debug.LogWarning("Cone::radius must be positive");
            radius = Mathf.Abs(radius);
        }
    }

    void Start()
    {
        Mesh mesh = filter.mesh;

        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        vertices.Add(new Vector3(0.0f, 0.0f, 0.0f));
        vertices.Add(new Vector3(0.0f, height, 0.0f));

        int centerBotIndex = 0;
        int centerTopIndex = 1;

        // Body
        {
            float theta = 2 * Mathf.PI / nMeridians;

            vertices.Add(new(
                radius * Mathf.Cos(0.0f),
                0.0f,
                radius * Mathf.Sin(0.0f)
            ));
            int firstBotPointIndex = vertices.Count - 1;
            int botPointIndex      = firstBotPointIndex;

            for (int i = (int)(nMeridians - 1); i > 0; i--)
            {
                float angle = i * theta;

                vertices.Add(new(
                    radius * Mathf.Cos(angle),
                    0.0f,
                    radius * Mathf.Sin(angle)
                ));

                int nextBotPointIndex = vertices.Count - 1;

                triangles.AddRange(new int[] {
                    centerTopIndex, botPointIndex, nextBotPointIndex,
                });

                botPointIndex = nextBotPointIndex;
            }

            triangles.AddRange(new int[] {
                centerTopIndex, botPointIndex, firstBotPointIndex,
            });
        }

        // Bottom face
        {
            int startBotIndex   = 2;
            int currentBotIndex = startBotIndex;
            // From last element - 1 to 0
            for (int i = vertices.Count - 1; i >= startBotIndex + 1; --i)
            {
                triangles.AddRange(new int[] { centerBotIndex, currentBotIndex, i });
                currentBotIndex = i;
            }
            // Because it's a circle, first vertex is also the last
            triangles.AddRange(new int[] { centerBotIndex, currentBotIndex, startBotIndex });
        }

        Debug.Assert(vertices.Count == nMeridians + 2, "Unexpected amount of vertices for this mesh");
        Debug.Assert(triangles.Count / 3 == nMeridians * 2, "Unexpected amount of triangles for this mesh");

        mesh.vertices  = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }
}
