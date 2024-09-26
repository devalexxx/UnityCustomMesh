using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Sphere : MonoBehaviour
{
    private MeshFilter filter;

    [SerializeField]
    private float radius = 1.0f;

    [SerializeField]
    private uint nmeridians = 3;
    
    [SerializeField]
    private uint nparallels = 2;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();

        if (nmeridians < 3)
            nmeridians = 3;

        if (nparallels < 2)
            nparallels = 2;
    }

    void Start()
    {
        Mesh mesh = filter.mesh;

        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        {
            int vFaces = (int)(nparallels + 1);

            float theta = Mathf.PI * 2 / nmeridians;
            float phi   = Mathf.PI / vFaces;

            // Sphere body
            {
                // Predefined first line of vertices
                vertices.Add(new(
                    radius * Mathf.Sin(phi) * Mathf.Cos(0.0f),
                    radius * Mathf.Cos(phi),
                    radius * Mathf.Sin(phi) * Mathf.Sin(0.0f)
                ));
                for (int meridian = (int)(nmeridians - 1); meridian > 0; meridian--)
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

                    int firstTopPointIndex = (vFace - 1) * (int)nmeridians;
                    int topPointIndex      = firstTopPointIndex;

                    Vector3 firstBotPoint = new(
                        radius * Mathf.Sin(botVAngle) * Mathf.Cos(0.0f),
                        radius * Mathf.Cos(botVAngle),
                        radius * Mathf.Sin(botVAngle) * Mathf.Sin(0.0f)
                    );
                    vertices.Add(firstBotPoint);
                    int firstBotPointIndex = vertices.Count - 1;
                    int botPointIndex      = firstBotPointIndex;


                    for (int meridian = (int)(nmeridians - 1); meridian > 0; meridian--)
                    {
                        float hAngle = meridian * theta;

                        Vector3 nextBotPoint = new(
                            radius * Mathf.Sin(botVAngle) * Mathf.Cos(hAngle),
                            radius * Mathf.Cos(botVAngle),
                            radius * Mathf.Sin(botVAngle) * Mathf.Sin(hAngle)
                        );
                        vertices.Add(nextBotPoint);

                        int nextTopPointIndex = vFace * (int)nmeridians - meridian;
                        int nextBotPointIndex = vertices.Count - 1;

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
            }

            int northPoleIndex = vertices.Count;
            int southPoleIndex = vertices.Count + 1;

            // South pole
            {
                int startBotIndex   = vertices.Count - (int)nmeridians;
                int currentBotIndex = startBotIndex;
                for (int i = vertices.Count - 1; i > startBotIndex; --i)
                {
                    triangles.AddRange(new int[] { southPoleIndex, currentBotIndex, i });
                    currentBotIndex = i;
                }
                triangles.AddRange(new int[] { southPoleIndex, currentBotIndex, startBotIndex });
            }

            // North pole
            {
                int startTopIndex   = 0;
                int currentTopIndex = startTopIndex;
                for (int i = startTopIndex + 1; i < nmeridians; i++)
                {
                    triangles.AddRange(new int[] { northPoleIndex, currentTopIndex, i });
                    currentTopIndex = i;
                } 
                triangles.AddRange(new int[] { northPoleIndex, currentTopIndex, startTopIndex });
            }

        }

        vertices.Add(new Vector3(0.0f,  radius, 0.0f));
        vertices.Add(new Vector3(0.0f, -radius, 0.0f));

        mesh.vertices  = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

}
