using System;
using UnityEngine;

public class BezierBerstein : MonoBehaviour
{
    [SerializeField] private Vector3 _p0;
    [SerializeField] private Vector3 _p1;
    [SerializeField] private Vector3 _p2;
    [SerializeField] private Vector3 _p3;
    
    [Range(0.1f, 1f)]
    [SerializeField] private float _step;

    private float F(int i)                 => i > 1 ? i * F(i - 1) : 1;
    private float B(int i, int n, float u) => F(n) / (F(i) * F(n - i)) * Mathf.Pow(u, i) * Mathf.Pow(1 - u, n - i);

    private void OnDrawGizmos()
    {
        Vector3[] ps = { _p0, _p1, _p2, _p3 };

        Gizmos.color = Color.blue;
        Gizmos.DrawLineStrip(ps, false);
        Gizmos.color = Color.red;

        int     n  = ps.Length;
        float   u  = 0f;
        Vector3 p0 = _p0;
        do
        {
            u += _step;
            Vector3 p1 = Vector3.zero;
            for (int i = 0; i < n; i++)
            {
                p1 += ps[i] * B(i, n - 1, u);
            }
            
            Gizmos.DrawLine(p0, p1);
            p0 = p1;
        } 
        while(u < 1f);
    }
}
