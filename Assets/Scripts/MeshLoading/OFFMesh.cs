using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class OFFMesh : MonoBehaviour
{    
    private MeshFilter filter;

    [SerializeField]
    private string path;

     void Awake()
    {
        filter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        Mesh mesh = filter.mesh;

        MeshLoader.Mesh loaded = MeshLoader.LoadOFF(path);

        mesh.vertices  = loaded.vertices;
        mesh.normals   = loaded.normals;
        mesh.triangles = loaded.triangles;

       MeshLoader.SaveAsObj(Path.Combine(Path.GetFullPath(path), "..", "..", "OBJ", Path.ChangeExtension(Path.GetFileName(path), ".obj")), loaded);
    }
}
