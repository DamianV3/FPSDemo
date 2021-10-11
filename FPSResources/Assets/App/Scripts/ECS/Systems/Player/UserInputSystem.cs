using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class UserInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var movement = UIGameController.Instance.Movement;
        var rotate = UIGameController.Instance.Rotate;
        var shot = UIGameController.Instance.IsShot;
        Entities
            .WithName("UserInput")
            .ForEach((Entity e, ref UserInputData inputData) =>
            {
                inputData.id = e.Index;
                inputData.Movement = new float2(movement.x, movement.y);
                inputData.Direction = new float2(rotate.x, rotate.y);
                inputData.shot = shot;
            }).ScheduleParallel();
    }
}
