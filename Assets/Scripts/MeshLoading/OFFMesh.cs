using System.IO;
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

        MeshLoader.Mesh loaded = MeshLoader.LoadOFFMesh(path);

        mesh.vertices  = loaded.vertices;
        mesh.normals   = loaded.normals;
        mesh.triangles = loaded.triangles;

        string objPath = Path.Combine(Path.GetFullPath(path), "..", "..", "OBJ", Path.ChangeExtension(Path.GetFileName(path), ".obj"));
        MeshLoader.SaveMesh(ref loaded, objPath);
    }
}
