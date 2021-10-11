using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float MovementSpeedMetersPerSecond = 5.0f;
    public int StartHealth = 3;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Here we add all of the components needed by the player
        dstManager.AddComponents(entity, new ComponentTypes(
            typeof(MovementSpeed),
            typeof(EnemyData)));
        // Set the movement speed value from the authoring component
        dstManager.SetComponentData(entity, new MovementSpeed { MetersPerSecond = MovementSpeedMetersPerSecond });
        dstManager.SetComponentData(entity, new EnemyData { Health = StartHealth });

        var controller = EntityController.Instance;
        if (controller)
        {
            controller.AddEnemy(entity, gameObject);
        }
    }
}
