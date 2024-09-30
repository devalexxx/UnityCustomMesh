using System;
using System.Collections.Generic;
using System.IO;
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

        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        const int bufferSize = 128;
        using (var fileStream   = File.OpenRead(path))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize)) {
            string   line;
            string[] sLine;

            line = streamReader.ReadLine();
            Debug.Assert(line == "OFF");

            line = streamReader.ReadLine();
            sLine = line.Split();
            int nVertices = int.Parse(sLine[0]);
            int nFaces    = int.Parse(sLine[1]);

            for (int i = 0; i < nVertices; ++i)
            {
                sLine = streamReader.ReadLine().Split(" ");
                var x = float.Parse(sLine[0]);
                var y = float.Parse(sLine[1]);
                var z = float.Parse(sLine[2]);
                vertices.Add(new Vector3(x, y, z));
            }

            for (int i = 0; i < nFaces; ++i)
            {
                sLine = streamReader.ReadLine().Split(" ");
                var x = int.Parse(sLine[1]);
                var y = int.Parse(sLine[2]);
                var z = int.Parse(sLine[3]);
                triangles.AddRange( new int[] { x, y, z });
            }

        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }
}
