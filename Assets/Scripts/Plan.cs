using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    private MeshFilter filter;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        Mesh mesh = filter.mesh;

        mesh.vertices = new Vector3[] 
        {
            new(0.0f, 0.0f, 0.0f),
            new(1.0f, 0.0f, 0.0f),
            new(1.0f, 1.0f, 0.0f),
            new(0.0f, 1.0f, 0.0f),
        };

        mesh.triangles = new int[]
        {
            2, 1, 0,
            3, 2, 0
        };
    }

}
