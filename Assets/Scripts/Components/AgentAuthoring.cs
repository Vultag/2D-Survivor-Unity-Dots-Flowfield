using Unity.Entities;
using UnityEngine;

/// tag for now
public struct AgentData : IComponentData
{
    public float lifetime;
}
public class AgentAuthoring : MonoBehaviour
{

    class AgentBaker : Baker<AgentAuthoring>
    {
        public override void Bake(AgentAuthoring authoring)
        {

            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new AgentData
            {
                lifetime = 25f
            });
        }
    }
}