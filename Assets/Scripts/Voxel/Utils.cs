using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    public static void CreateCube(AABB aabb, Transform transform)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position    = aabb.GetCenter();
        cube.transform.localScale *= aabb.GetSize().x;
        cube.transform.SetParent(transform);
    }

    public static void Subdivide(AABB aabb, uint depth, Transform transform)
    {
        switch (aabb.Intersect(new(0, 0, 0), 1))
        {
            case IntersectionType.Intersect:
                if (depth > 0)
                {
                    foreach (var aabb2 in aabb.Split())
                    {
                        Subdivide(aabb2, depth - 1, transform);
                    }
                }
                break;
            case IntersectionType.Inside:
                CreateCube(aabb, transform);
                break;
            case IntersectionType.Outside:
                break;
        }
    }

}
