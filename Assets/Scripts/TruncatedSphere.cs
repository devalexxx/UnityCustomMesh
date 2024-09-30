using System.Collections.Generic;
using UnityEngine;

public class TruncatedSphere : MonoBehaviour
{
    private MeshFilter filter;

    [SerializeField]
    private float radius = 1.0f;

    [SerializeField]
    private uint nMeridians = 16;
    
    [SerializeField]
    private uint nParallels = 8;

    [SerializeField]
    private uint nCuttingMeridians = 3;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();

        if (nMeridians < 3)
        {
            Debug.LogWarning("TruncatedSphere::nMeridians must be greater than 3");
            nMeridians = 3;
        }

        if (nParallels < 2)
        {
            Debug.LogWarning("TruncatedSphere::nParallels must be greater than 2");
            nParallels = 2;
        }

        if (nCuttingMeridians > nMeridians - 1)
        {
            Debug.LogWarning("TruncatedSphere::nCuttingMeridians must be between 1 and nMeridians - 1");
            nCuttingMeridians = nMeridians - 1;
        }

        if (nCuttingMeridians < 1)
        {
            Debug.LogWarning("TruncatedSphere::nCuttingMeridians must be between 1 and nMeridians - 1");
            nCuttingMeridians = 1;
        }

        if (radius < 0)
        {
            Debug.LogWarning("nCuttingMeridians::radius must be positive");
            radius = Mathf.Abs(radius);
        }
            
    }

    void Start()
    {
        Mesh mesh = filter.mesh;

        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        {
            int vFaces = (int)(nParallels + 1);

            float theta = Mathf.PI * 2 / nMeridians;
            float phi   = Mathf.PI / vFaces;

            // Sphere body
            {
                // Predefined first line of vertices
                vertices.Add(new(
                    radius * Mathf.Sin(phi) * Mathf.Cos(0.0f),
                    radius * Mathf.Cos(phi),
                    radius * Mathf.Sin(phi) * Mathf.Sin(0.0f)
                ));
                for (int meridian = (int)(nMeridians - nCuttingMeridians); meridian >= 0; meridian--)
                {
                    vertices.Add(new(
                        radius * Mathf.Sin(phi) * Mathf.Cos(meridian * theta),
                        radius * Mathf.Cos(phi),
                        radius * Mathf.Sin(phi) * Mathf.Sin(meridian * theta)
                    ));
                }

                for (int vFace = 1; vFace < vFaces - 1; vFace++)
                {
                    float botVAngle = (vFace + 1) * phi;

                    int firstTopPointIndex = (vFace - 1) * (int)(nMeridians + 2 - nCuttingMeridians);
                    int topPointIndex      = firstTopPointIndex;

                    vertices.Add(new(
                        radius * Mathf.Sin(botVAngle) * Mathf.Cos(0.0f),
                        radius * Mathf.Cos(botVAngle),
                        radius * Mathf.Sin(botVAngle) * Mathf.Sin(0.0f)
                    ));
                    int firstBotPointIndex = vertices.Count - 1;
                    int botPointIndex      = firstBotPointIndex;


                    for (int meridian = (int)(nMeridians - nCuttingMeridians); meridian >= 0; meridian--)
                    {
                        float hAngle = meridian * theta;

                        vertices.Add(new(
                            radius * Mathf.Sin(botVAngle) * Mathf.Cos(hAngle),
                            radius * Mathf.Cos(botVAngle),
                            radius * Mathf.Sin(botVAngle) * Mathf.Sin(hAngle)
                        ));

                        int nextTopPointIndex = vFace * (int)(nMeridians + 2 - nCuttingMeridians) - meridian;
                        int nextBotPointIndex = vertices.Count - 1;

                        triangles.AddRange(new int[] {
                            topPointIndex, botPointIndex, nextBotPointIndex,
                            nextTopPointIndex, topPointIndex, nextBotPointIndex
                        });

                        topPointIndex = nextTopPointIndex;
                        botPointIndex = nextBotPointIndex;
                    }   

                    // triangles.AddRange(new int[] {
                    //     topPointIndex, botPointIndex, firstBotPointIndex,
                    //     firstTopPointIndex, topPointIndex, firstBotPointIndex
                    // });
                }
            }

            int northPoleIndex = vertices.Count;
            int southPoleIndex = vertices.Count + 1;
            int centerIndex    = vertices.Count + 2;

            // South pole
            {
                // int startBotIndex   = vertices.Count - (int)(nMeridians + nCuttingMeridians);
                // int currentBotIndex = startBotIndex;
                // for (int i = vertices.Count - 1; i > startBotIndex; --i)
                // {
                //     triangles.AddRange(new int[] { southPoleIndex, currentBotIndex, i });
                //     currentBotIndex = i;
                // }
                // triangles.AddRange(new int[] { southPoleIndex, currentBotIndex, startBotIndex });
            }

            // North pole
            {
                // int startTopIndex   = 0;
                // int currentTopIndex = startTopIndex;
                // for (int i = startTopIndex + 1; i < nMeridians - nCuttingMeridians; i++)
                // {
                //     triangles.AddRange(new int[] { northPoleIndex, currentTopIndex, i });
                //     currentTopIndex = i;
                // } 
                // triangles.AddRange(new int[] { northPoleIndex, currentTopIndex, startTopIndex });
            }

            {

            }

        }

        vertices.Add(new Vector3(0.0f,  radius, 0.0f));
        vertices.Add(new Vector3(0.0f, -radius, 0.0f));
        vertices.Add(new Vector3(0.0f, 0, 0.0f));

        mesh.vertices  = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }
}
