using System.Collections.Generic;
using UnityEngine;

public class MapSegment
{
    public List<GameObject> objects;
    public float startAxis;
    public float endAxis;
    public MapSegment(List<GameObject> objects, float startAxis, float endAxis)
    {
        this.objects = objects;
        this.startAxis = startAxis;
        this.endAxis = endAxis;
    }
}
