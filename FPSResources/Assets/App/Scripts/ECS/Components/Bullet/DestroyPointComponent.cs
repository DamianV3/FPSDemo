using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct DestroyPointComponent : IComponentData
{
    public float3 Value;
}
