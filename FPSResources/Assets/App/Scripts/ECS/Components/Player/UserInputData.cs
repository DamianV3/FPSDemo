using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct UserInputData : IComponentData
{
    public int id;
    public float2 Movement;
    public float2 Direction;
    public bool shot;
    public bool jump;
}
