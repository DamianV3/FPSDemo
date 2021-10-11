using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
/// <summary>
/// System for destroying Enemy Entities and the Player
/// </summary>
public class DestructionSystem : ComponentSystem
{
    float thresholdDistance = 1f;
    float playerThresholdDistance = 2f;

    protected override void OnUpdate()
    {
        if (GameManager.IsGameOver())
        {
            return;
        }

        var controller = EntityController.Instance;
        var manager = World.EntityManager;

        float3 playerPosition = GameManager.GetPlayerPosition();

        Entities.WithAll<EnemyTag>().ForEach((Entity enemy, ref Translation enemyPos, ref EnemyData health) =>
        {
            if(health.Health <= 0) {
                health.Health = 0;
                controller.RemoveEnemy(enemy);
                PostUpdateCommands.DestroyEntity(enemy);
                GameManager.AddScore(1);
            }
            else if (math.distance(enemyPos.Value, playerPosition) <= playerThresholdDistance)
            {

                // create an explosion at the enemy
                //FXManager.Instance.CreateExplosion(enemyPos.Value);

                // create an explosion at the player (comment this out for stress test)
                //FXManager.Instance.CreateExplosion(playerPosition);

                // end the game (comment this out for stress test)
                GameManager.EndGame();

                // 7 safely remove the enemy Enity using an EntityCommandBuffer
                PostUpdateCommands.DestroyEntity(enemy);
            }
        });

        Entities.WithAll<BulletTag>().ForEach((Entity bullet, ref Translation bulletPos) =>
        {
            if(manager.HasComponent<DestroyPointComponent>(bullet)) {
                var data = manager.GetComponentData<DestroyPointComponent>(bullet);
                if (math.distance(data.Value, bulletPos.Value) <= thresholdDistance)
                {
                    PostUpdateCommands.DestroyEntity(bullet);

                    //FXManager.Instance.CreateExplosion(enemyPosition);
                }
            }
        });
    }
}
