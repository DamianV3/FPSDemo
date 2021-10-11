using UnityEngine;
using Unity.Entities;

public class InputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //var deltaTime = Time.DeltaTime;
        //Entities.ForEach((InputData data, ref UserInputData inputData) =>
        //    {
        //        data.ButtonShot.onClick = (e)=>{
        //            inputData.shot = true;
        //        };
        //    }).ScheduleParallel();
    }
}
