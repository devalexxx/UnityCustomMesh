using System.Collections.Generic;
using UnityEngine;

public class TuncatedCone : MonoBehaviour
{
    private MeshFilter filter;

    [SerializeField]
    private float radius = 1.0f;

    [SerializeField]
    private float height = 3.0f;

    [SerializeField]
    private float cuttingHeight = 2.0f;

    [SerializeField]
    private uint nMeridians = 16;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();

        if (nMeridians < 3)
        {
            Debug.LogWarning("TruncatedCone::nMeridians must be greater than 3");
            nMeridians = 3;
        }

        if (height < 0)
        {
            Debug.LogWarning("TruncatedCone::height must be positive");
            height = Mathf.Abs(height);
        }

        if (radius < 0)
        {
            Debug.LogWarning("TruncatedCone::radius must be positive");
            radius = Mathf.Abs(radius);
        }

        if (cuttingHeight < 0)
        {
            Debug.LogWarning("TruncatedCone::cuttingHeight must be positive");
            cuttingHeight = Mathf.Abs(cuttingHeight);
        }   

        if (cuttingHeight > height)
        {
            Debug.LogWarning("TruncatedCone::cuttingHeight must be between 0 and TruncatedCone::height");
            cuttingHeight = 0.9f * height;
        }
    }

    void Start()
    {
        Mesh mesh = filter.mesh;

        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        // Vertical faces
        {
            float theta = 2 * Mathf.PI / nMeridians;

            float topRadius = radius - (radius * (cuttingHeight / height));

            vertices.Add(new(
                radius * Mathf.Cos(0.0f),
                0,
                radius * Mathf.Sin(0.0f)
            ));
            vertices.Add(new(
                topRadius * Mathf.Cos(0.0f),
                cuttingHeight,
                topRadius * Mathf.Sin(0.0f)
            ));

            int firstBotPointIndex = 0;
            int firstTopPointIndex = 1;

            int botPointIndex = firstBotPointIndex;
            int topPointIndex = firstTopPointIndex;

            for (int i = (int)(nMeridians - 1); i > 0; i--)
            {
                float angle = i * theta;

                vertices.Add(new(
                    radius * Mathf.Cos(angle),
                    0,
                    radius * Mathf.Sin(angle)
                ));
                vertices.Add(new(
                    topRadius * Mathf.Cos(angle),
                    cuttingHeight,
                    topRadius * Mathf.Sin(angle)
                ));

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

        // Preshot both top and bottom center index to form triangles without interfere with the loop
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

        vertices.Add(new Vector3(0.0f, cuttingHeight, 0.0f));
        vertices.Add(new Vector3(0.0f, 0.0f, 0.0f));

        mesh.vertices  = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }
}
