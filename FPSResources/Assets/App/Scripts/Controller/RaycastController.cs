using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Unity.Mathematics;

public class RaycastController : Singleton<RaycastController>
{
    public bool Cast(float3 start, float3 dir, out RaycastHit hit, int layer, float distance = 1000f)
    {
        Ray ray = new Ray(start, dir);
        return Physics.Raycast(ray, out hit, distance, layer);
    }
}
