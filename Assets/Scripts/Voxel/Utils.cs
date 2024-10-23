using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereRepr
{
    public float radius;
    public Vector3 center;

    public SphereRepr(float r, Vector3 c)
    {
        radius = r;
        center = c;
    }
}

public enum OperatorType
{
    Union, Intersection
}

public static class Utils
{

    public static void CreateCube(AABB aabb, Transform transform)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position    = aabb.GetCenter();
        cube.transform.localScale *= aabb.GetSize().x;
        cube.transform.SetParent(transform);
    }

    public static void Subdivide(SphereRepr repr, AABB aabb, uint depth, Transform transform)
    {
        switch (aabb.Intersect(repr))
        {
            case IntersectionType.Intersect:
                if (depth > 0)
                {
                    foreach (var aabb2 in aabb.Split())
                    {
                        Subdivide(repr, aabb2, depth - 1, transform);
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

    public static void Subdivide(SphereRepr[] reprs, AABB aabb, uint depth, Transform transform, OperatorType op)
    {
        List<IntersectionType> its = new();
        foreach (var repr in reprs)
        {   
            its.Add(aabb.Intersect(repr));
        }

        if (!its.Contains(IntersectionType.Intersect))
        {
            switch (op)
            {
                case OperatorType.Intersection:
                    if (its.All(it => it == IntersectionType.Inside))
                        CreateCube(aabb, transform);
                    break;
                case OperatorType.Union:
                    if (its.Contains(IntersectionType.Inside))
                        CreateCube(aabb, transform);
                    break;
            }
        }
        else if (depth > 0)
        {
            foreach (var aabb2 in aabb.Split())
            {
                Subdivide(reprs, aabb2, depth - 1, transform, op);
            }
        }
        else
        {
            if (op == OperatorType.Intersection && !its.Contains(IntersectionType.Outside))
                CreateCube(aabb, transform);
            else if (op == OperatorType.Union)
                CreateCube(aabb, transform);
        }
    }

}
