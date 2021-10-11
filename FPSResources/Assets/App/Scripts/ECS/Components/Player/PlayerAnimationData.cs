using UnityEngine;
using Unity.Entities;
using Unity.Animation;

public struct PlayerAnimationData : IComponentData
{
    public BlobAssetReference<Clip> Clip;
}
