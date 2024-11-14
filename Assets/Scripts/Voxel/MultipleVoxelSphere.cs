using UnityEngine;

public class MultipleVoxelSphere : MonoBehaviour
{
    [SerializeField]
    private uint _resolution;

    [SerializeField]
    private OperatorType _op;

    [SerializeField]
    private SphereRepr[] _spheres;

    private void Start()
    {   
        AABB aabb = new(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Utils.Subdivide(_spheres, aabb, _resolution, transform, _op);
    }
}
