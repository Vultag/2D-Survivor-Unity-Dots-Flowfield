using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct TargetData : IComponentData
{

}
public class TargetAuthoring : MonoBehaviour
{

    class TargetBaker : Baker<TargetAuthoring>
    {
        public override void Bake(TargetAuthoring authoring)
        {

            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new TargetData());
        }
    }
}