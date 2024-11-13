using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> _points;

    [SerializeField]
    private Color _baseColor;
    [SerializeField]
    private Color _subdivideColor;

    [SerializeField]
    private int _depth;

    [SerializeField]
    private float _u;
    [SerializeField]
    private float _v;

    [SerializeField]
    private List<Vector3> _sPoints;

    private void OnDrawGizmos()
    {
        Gizmos.color = _baseColor;
        Gizmos.DrawLineStrip(_points.ToArray(), true);
        Gizmos.color = _subdivideColor;
        Gizmos.DrawLineStrip(_sPoints.ToArray(), true);
    }

    public void Subdivide(int depth)
    {
        if (depth > 0)
        {
            List<Vector3> points = new();
            for (int i = 0; i < _sPoints.Count - 1; i++)
            {
                points.Add(_v * _sPoints[i] + _u * _sPoints[i + 1]);
                points.Add(_u * _sPoints[i] + _v * _sPoints[i + 1]);
            }
            points.Add(_v * _sPoints[_sPoints.Count - 1] + _u * _sPoints[0]);
            points.Add(_u * _sPoints[_sPoints.Count - 1] + _v * _sPoints[0]);
            _sPoints = points;
            Subdivide(depth - 1);
        }
    }

    void Start()
    {
        _sPoints = _points;
        Subdivide(_depth);
    }

}
