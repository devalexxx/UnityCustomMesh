using UnityEngine;

public class DoubleVoxelSphere : MonoBehaviour
{
    [SerializeField]
    private uint _resolution;

    [SerializeField]
    private float _gap;

    [SerializeField]
    private OperatorType _op;

    private void Start()
    {   
        SphereRepr[] reprs = new SphereRepr[] { new (0.5f, new Vector3(0.5f - _gap, 0, 0)), new (0.5f, new Vector3 (0.5f, 0, 0)) };
        AABB         aabb  = new(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));

        Utils.Subdivide(reprs, aabb, _resolution, transform, _op);
    }
}
