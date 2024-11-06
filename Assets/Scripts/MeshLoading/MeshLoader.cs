using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class MeshLoader
{
    public class Mesh
    {
        public Vector3[] vertices;
        public Vector3[] normals;
        public int[]     triangles;
    }

    public static void SaveAsObj(string path, Mesh mesh)
    {
        using (var fileStream   = File.OpenWrite(path))
        using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8)) {    
            streamWriter.WriteLine("# " + Path.GetFileName(path));
            streamWriter.WriteLine();
            streamWriter.WriteLine("o " + Path.GetFileNameWithoutExtension(path));
            streamWriter.WriteLine();

            foreach (var v in mesh.vertices)
            {
                streamWriter.WriteLine("v " + v.x + " " + v.y + " " + v.z);
            }

            streamWriter.WriteLine();

            foreach (var v in mesh.normals)
            {
                streamWriter.WriteLine("vn " + v.x + " " + v.y + " " + v.z);
            }

            streamWriter.WriteLine();

            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                streamWriter.WriteLine("f "
                    + (mesh.triangles[i + 0] + 1) + "//" + (mesh.triangles[i + 0] + 1) + " "
                    + (mesh.triangles[i + 1] + 1) + "//" + (mesh.triangles[i + 1] + 1) + " "
                    + (mesh.triangles[i + 2] + 1) + "//" + (mesh.triangles[i + 2] + 1)
                );
            }

        }
    }

    public static Mesh LoadOFF(string path)
    {
        List<Vector3> vertices  = new();
        List<int>     triangles = new();

        Vector3 vSum = new(0.0f, 0.0f, 0.0f);
        float mMax = float.MinValue;
        int nVertices, nFaces;

        const int bufferSize = 128;
        using (var fileStream   = File.OpenRead(path))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize)) {
            string   line;
            string[] sLine;

            line = streamReader.ReadLine();
            Debug.Assert(line == "OFF");

            line = streamReader.ReadLine();
            sLine = line.Split();
            nVertices = int.Parse(sLine[0]);
            nFaces    = int.Parse(sLine[1]);

            for (int i = 0; i < nVertices; ++i)
            {
                sLine = streamReader.ReadLine().Split(" ");
                var x = float.Parse(sLine[0]);
                var y = float.Parse(sLine[1]);
                var z = float.Parse(sLine[2]);
                Vector3 v = new Vector3(x, y, z);

                vSum += v;
                vertices.Add(v);
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

        Vector3 center = vSum / nVertices;

        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] -= center;
            mMax = Mathf.Max(mMax, vertices[i].magnitude);
        }

        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] /= mMax;
        }

        Vector3[] normals = new Vector3[vertices.Count];

        for (int i = 0; i < triangles.Count; i += 3)
        {
            int idx1 = triangles[i + 0];
            int idx2 = triangles[i + 1];
            int idx3 = triangles[i + 2];
            Vector3 normal = Vector3.Cross(vertices[idx1] - vertices[idx2], vertices[idx1] - vertices[idx3]).normalized;

            if (normals[idx1].magnitude > 0.5)
                normals[idx1] = ((normals[idx1] + normal) / 2).normalized;
            else
                normals[idx1] = normal;

            if (normals[idx2].magnitude > 0.5)
                normals[idx2] = ((normals[idx2] + normal) / 2).normalized;
            else
                normals[idx2] = normal;
            
            if (normals[idx3].magnitude > 0.5)
                normals[idx3] = ((normals[idx3] + normal) / 2).normalized;
            else
                normals[idx3] = normal;
        }

        return new Mesh()
        {
            vertices  = vertices.ToArray(),
            normals   = normals,
            triangles = triangles.ToArray()
        };
    }
}
