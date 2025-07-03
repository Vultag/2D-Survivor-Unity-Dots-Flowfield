using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
partial struct AgentSystem : ISystem
{
    private EntityQuery targetEntityQuery;


    public void OnCreate(ref SystemState state)
    {
        targetEntityQuery = state.EntityManager.CreateEntityQuery(typeof(TargetData));
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var PlayerPosition = state.EntityManager.GetComponentData<LocalTransform>(targetEntityQuery.GetSingletonEntity()).Position;

        foreach (var (agent_phy, trans) in SystemAPI.Query<RefRW<PhyBodyData>, RefRO<LocalTransform>>().WithAll<AgentData>())
        {
            FlowfieldCellData agentCell = FlowfieldGridStorage.GetCellFromPosition(trans.ValueRO.Position);
            Vector2 moveDirection = agentCell.InLineOfSight? new Vector2(PlayerPosition.x- trans.ValueRO.Position.x,PlayerPosition.y- trans.ValueRO.Position.y).normalized : agentCell.Direction;
            agent_phy.ValueRW.Force += moveDirection * 0.0005f;

        }
    }

}
