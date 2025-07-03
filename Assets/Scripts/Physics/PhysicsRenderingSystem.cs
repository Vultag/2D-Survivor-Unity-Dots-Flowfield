using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public partial struct PhysicsRenderingSystem : ISystem
{

    private Vector2 gravity;

    public void OnCreate(ref SystemState state)
    {
        gravity = new Vector2(0f, -9.81f);

    }


    public void OnUpdate(ref SystemState state)
    {

        foreach (var (shape, trans) in SystemAPI.Query<RefRW<ShapeData>, RefRW<LocalTransform>>().WithAny<PhyBodyData>())
        {
            float fixedDeltaTime = 1f / 60f; // Assuming Fixed Timestep = (60Hz physics update)
            float alpha = ((float)SystemAPI.Time.ElapsedTime% fixedDeltaTime) / fixedDeltaTime;
            alpha = math.saturate(alpha); // Ensure alpha is clamped between 0-1
            //Debug.Log(alpha);
            ////trans.ValueRW.Position = new float3(math.lerp(shape.ValueRO.PreviousPosition, shape.ValueRO.Position, alpha), trans.ValueRO.Position.z);
            ////trans.ValueRW.Rotation = Quaternion.Euler(0,0, math.lerp(shape.ValueRO.PreviousRotation, shape.ValueRO.Rotation, alpha));
            trans.ValueRW.Position = new float3(shape.ValueRO.Position, trans.ValueRO.Position.z);
            trans.ValueRW.Rotation = Quaternion.Euler(0,0, shape.ValueRO.Rotation);

        }

    }


}
