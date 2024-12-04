using System;
using Unity.VisualScripting;
using UnityEngine;

public class Hermite : MonoBehaviour
{
    [SerializeField] private Vector3 _p0;
    [SerializeField] private Vector3 _p1;
    [SerializeField] private Vector3 _v0;
    [SerializeField] private Vector3 _v1;

    [Range(0.001f, 1f)]
    [SerializeField] private float _step;

    private Vector3 GetPoint(float u)
    {
        var u2 = u * u;
        var u3 = u * u * u;
        return (2 * u3 - 3 * u2 + 1) * _p0 + (-2 * u3 + 3 * u3) * _p1 + (u3 - 2 * u2 + u) * _v0 + (u3 - u2) * _v1;
    }

    private void OnDrawGizmos()
    {
        float   u     = 0;
        Vector3 p0 = _p0;
        do
        {
            u += _step;
            var p1 = GetPoint(u);
            Gizmos.DrawLine(p0, p1);
            p0 = p1;
        } 
        while(u < 1f);
    }
}
