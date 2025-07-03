
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System;


[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class TargetSystem : SystemBase
{

    private EntityQuery CentralizedInputDataQuery;

    protected override void OnCreate()
    {
        CentralizedInputDataQuery = EntityManager.CreateEntityQuery(typeof(CentralizedInputData));
    }
    protected override void OnStartRunning()
    {

        //physics
        foreach (var (shape, trans) in SystemAPI.Query<RefRW<ShapeData>, RefRO<LocalTransform>>())
        {

            shape.ValueRW.Position = new Vector2(trans.ValueRO.Position.x, trans.ValueRO.Position.y);

        }

    }


    protected override void OnUpdate()
    {
        foreach (var ( player_phy, trans) in SystemAPI.Query<RefRW<PhyBodyData>, RefRO<LocalTransform>>().WithAll<TargetData>())
        {

            var inputs = EntityManager.GetComponentData<CentralizedInputData>(CentralizedInputDataQuery.GetSingletonEntity());

            var moveDirection = inputs.playerMouvements;

            player_phy.ValueRW.Force += moveDirection * 0.0015f;

        }


    }


}