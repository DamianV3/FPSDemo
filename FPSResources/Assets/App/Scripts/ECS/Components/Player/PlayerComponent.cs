using Unity.Entities;
using Unity.Mathematics;

// Component data for a Player entity.
[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    public int Score;
    public float3 Position;
    public quaternion Rotation;
    public float xRotation;
    public float yRotation;
    public int raycastLayer;
}