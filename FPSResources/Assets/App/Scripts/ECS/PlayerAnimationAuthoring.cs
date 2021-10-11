using Unity.Entities;
using Unity.Transforms;
using Unity.Animation;
using UnityEngine;
using Unity.Animation.Hybrid;

[DisallowMultipleComponent]
public class PlayerAnimationAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public AnimationClip Clip;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (Clip == null)
            return;

        conversionSystem.DeclareAssetDependency(gameObject, Clip);

        // Here we add all of the components needed by the player
        dstManager.AddComponentData(entity, new PlayerAnimationData
        {
            Clip = conversionSystem.BlobAssetStore.GetClip(Clip)
        });

        dstManager.AddComponent<DeltaTime>(entity);
    }
}
