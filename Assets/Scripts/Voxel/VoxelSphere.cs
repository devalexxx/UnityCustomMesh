using UnityEngine;

public class VoxelSphere : MonoBehaviour
{ 
    [SerializeField]
    private uint resolution;

    private void Start()
    {   
        Utils.Subdivide(new(new Vector3(-1, -1, -1), new Vector3(1, 1, 1)), resolution, transform);
    }
}
