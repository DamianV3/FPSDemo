using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float MovementSpeedMetersPerSecond = 5.0f;
    public LayerMask shotLayer;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Here we add all of the components needed by the player
        dstManager.AddComponents(entity, new ComponentTypes(
            typeof(UserInputData),
            typeof(MovementSpeed),
            typeof(PlayerData)));
        // Set the movement speed value from the authoring component
        dstManager.SetComponentData(entity, new MovementSpeed { MetersPerSecond = MovementSpeedMetersPerSecond });
        dstManager.SetComponentData(entity, new PlayerData { raycastLayer = shotLayer, Position = this.transform.position, Rotation = this.transform.rotation });
        
        var controller = EntityController.Instance;
        if(controller)
        {
            controller.player.obj = this.gameObject;
            controller.player.entity = entity;
        }
    }
}
