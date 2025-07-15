using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct AgentSpawnerData : IComponentData
{
    public bool active;
    public float SpawnRate;
    public Entity AgentPrefab;
    public float RespawnTimer;
}

public class AgentSpawnerAuthoring : MonoBehaviour
{

    public bool active;
    public float SpawnRate;
    public GameObject AgentPrefab;

    class AgentSpawnerBaker : Baker<AgentSpawnerAuthoring>
    {
        public override void Bake(AgentSpawnerAuthoring authoring)
        {

            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new AgentSpawnerData
            {

                active = authoring.active,
                SpawnRate = authoring.SpawnRate,
                AgentPrefab = GetEntity(authoring.AgentPrefab, TransformUsageFlags.None),
                RespawnTimer = authoring.SpawnRate

            });
        }
    }
}