using Unity.Entities;
using UnityEngine;

public struct CentralizedInputData : IComponentData
{
    public Vector2 playerMouvements;
}
/// <summary>
/// Gather inputs in mono world and pass to ecs world
/// </summary>
public class InputsManager : MonoBehaviour
{
    public static PlayerInputs playerInputs;

    private CentralizedInputData CentralizedInputData;

    private EntityQuery CentralizedInputDataQuery;

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.PlayerMap.Enable();

        //inputs = new Inputs();
        CentralizedInputData = new CentralizedInputData();

        var world = World.DefaultGameObjectInjectionWorld;
        var em = world.EntityManager;
        CentralizedInputDataQuery = em.CreateEntityQuery(typeof(CentralizedInputData));
        em.CreateEntity(em.CreateArchetype(typeof(CentralizedInputData)));
    }

    void Update()
    {
        CentralizedInputData.playerMouvements = playerInputs.PlayerMap.Mouvements.ReadValue<Vector2>();

        // Push to ECS
        var world = World.DefaultGameObjectInjectionWorld;
        var em = world.EntityManager;

        em.SetComponentData(CentralizedInputDataQuery.GetSingletonEntity(), CentralizedInputData);
        /// clear for next frame
        CentralizedInputData = new CentralizedInputData();
    }
}
