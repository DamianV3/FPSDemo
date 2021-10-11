using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var controller = EntityController.Instance;
        var hit = RaycastController.Instance;

        Entities
            .WithName("MovePlayer")
            .WithoutBurst()
            .ForEach((
                Entity e,
                ref Translation translation,
                ref Rotation rotation,
                ref PlayerData player,
                in LocalToWorld ltw,
                in UserInputData input,
                in MovementSpeed speed) =>
            {
                float3 fwd = ltw.Forward;
                fwd.y = 0;
                float3 right = ltw.Right;
                right.y = 0;
                if (input.Movement.y != 0)
                    player.Position += fwd * (input.Movement.y * speed.MetersPerSecond * deltaTime);
                if(input.Movement.x != 0)
                    player.Position += right * (input.Movement.x * speed.MetersPerSecond * deltaTime);

                translation.Value = player.Position;

                player.xRotation -= input.Direction.y * deltaTime;
                player.xRotation = math.clamp(player.xRotation, -90.0f, 90.0f);
                player.yRotation += input.Direction.x * deltaTime;
                player.Rotation = quaternion.Euler(player.xRotation, player.yRotation, 0);

                rotation.Value = player.Rotation;
                

                if (input.shot && hit != null)
                {
                    var start = player.Position;
                    var end = ltw.Forward;
                    RaycastHit hitO;
                    bool hitTest = hit.Cast(start, end, out hitO, player.raycastLayer);
                    if (hitTest)
                    {
                        
                    }
                }
            }).Run();
    }
}
