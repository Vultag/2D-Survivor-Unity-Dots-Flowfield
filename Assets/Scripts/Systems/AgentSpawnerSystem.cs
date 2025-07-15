using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;



[UpdateInGroup(typeof(SimulationSystemGroup))]
//[UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
//[UpdateBefore(typeof(TreeInsersionSystem))]
public partial struct AgentSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    public void OnUpdate(ref SystemState state)
    {

        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var spawner in SystemAPI.Query<RefRW<AgentSpawnerData>>())
        {

            if (spawner.ValueRO.active == true)
            {

                spawner.ValueRW.RespawnTimer -= SystemAPI.Time.DeltaTime;

                if (spawner.ValueRO.RespawnTimer < 0)
                {

                    var agent = ecb.Instantiate(spawner.ValueRO.AgentPrefab);

                    var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

                    var new_pos = new float3(FlowfieldGridStorage.gridCenter.xy,0)  + new float3(direction.x * 40, direction.y * 40,0);

                    //new_pos = float3.zero;

                    ecb.SetComponent<LocalTransform>(agent, new LocalTransform { Position = new_pos, Rotation = Quaternion.identity, Scale = 1f });

                    var newShapeData = state.EntityManager.GetComponentData<ShapeData>(spawner.ValueRO.AgentPrefab);
                    newShapeData.Position = new_pos.xy;
                    newShapeData.PreviousPosition = new_pos.xy;
                    newShapeData.Rotation = 0;
                    ecb.SetComponent<ShapeData>(agent, newShapeData);

                    spawner.ValueRW.RespawnTimer = spawner.ValueRO.SpawnRate;
                }
            }

        }

    }


}