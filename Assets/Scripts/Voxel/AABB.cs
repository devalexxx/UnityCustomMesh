using System.Collections.Generic;
using UnityEngine;

public enum IntersectionType
{
    Intersect, Inside, Outside
}


public class AABB
{
    Vector3 min;
    Vector3 max;
    Vector3 center;
    Vector3 size;

    public AABB(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
        this.center = (min + max) / 2;
        this.size   = max - min;
    }

    public Vector3 GetCenter()
    {
        return center;
    }

    public Vector3 GetSize()
    {
        return size;
    }

    public AABB[] Split()
    {
        List<AABB> list = new();

        Vector3 l = max - center;
        for (int k = 0; k < 2; ++k)
        {
            for (int j = 0; j < 2; ++j)
            {
                for (int i = 0; i < 2; ++i)
                {
                    Vector3 nMin = new(min.x + i * l.x, min.y + j * l.y, min.z + k * l.z);
                    list.Add(new(nMin, new(nMin.x + l.x, nMin.y + l.y, nMin.z + l.z)));
                }   
            }   
        }
        return list.ToArray();
    }

    public IntersectionType Intersect(Vector3 center, float r)
    {
        float r2 = r * r;
        float dmax = 0;
        float dmin = 0;
        for(int i = 0; i < 3; i++ ) {
            float a = (center[i] - min[i]) * (center[i] - min[i]);
            float b = (center[i] - max[i]) * (center[i] - max[i]);
            dmax += Mathf.Max(a, b);
            if( center[i] < min[i]) 
                dmin += a; 
            else if( center[i] > max[i]) 
                dmin += b;
        }
        if( dmin <= r2 && dmax >= r2 ) 
            return IntersectionType.Intersect;
        else if (dmin < r2 && dmax < r2)
            return IntersectionType.Inside;
        else
            return IntersectionType.Outside;
    }
}
