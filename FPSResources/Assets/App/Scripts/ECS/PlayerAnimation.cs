using Unity.Animation;
using Unity.DataFlowGraph;
using Unity.Entities;
using UnityEngine;

#if UNITY_EDITOR
using Unity.Animation.Hybrid;
public class PlayerAnimation : MonoBehaviour, IConvertGameObjectToEntity
{
    public AnimationClip Clip;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (Clip == null)
            return;

        conversionSystem.DeclareAssetDependency(gameObject, Clip);

        dstManager.AddComponentData(entity, new PlayerAnimationData
        {
            Clip = conversionSystem.BlobAssetStore.GetClip(Clip)
        });

        //dstManager.AddComponent<DeltaTime>(entity);
    }
}
#endif





