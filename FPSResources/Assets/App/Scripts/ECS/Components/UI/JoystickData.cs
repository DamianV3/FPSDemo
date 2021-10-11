using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct JoystickData : IComponentData
{
    public float2 Value;
}
