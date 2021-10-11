using UnityEngine;
using Unity.Entities;

public class FollowCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entities = entityManager.GetAllEntities();
        if(EntityController.Instance.player.entity != null)
        {
            if (entityManager.HasComponent<PlayerData>(EntityController.Instance.player.entity))
            {
                var data = entityManager.GetComponentData<PlayerData>(EntityController.Instance.player.entity);
                transform.position = data.Position;
                transform.rotation = data.Rotation;
            }
        }
    }
}
