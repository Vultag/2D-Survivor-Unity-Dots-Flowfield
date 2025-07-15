using Unity.Entities;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

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

    private float currentZoom;
    private float zoomTarget;
    private float zoomDelta;

    [SerializeField]
    Vector2 MinMaxZoom;
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


        zoomTarget = Camera.main.orthographicSize;
        currentZoom = zoomTarget;

        playerInputs.PlayerMap.Zoom.performed += OnZoomTick;
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

        ///ZOOM
        if (zoomTarget != currentZoom)
        {
            float smoothing = 3f;
            Camera.main.orthographicSize = Mathf.Lerp(currentZoom, zoomTarget, 1f - Mathf.Exp(-smoothing * zoomDelta));
            zoomDelta += Time.deltaTime;
        }

    }
    private void OnZoomTick(CallbackContext context)
    {
        //Debug.Log(playerInputs.PlayerMap.Zoom.ReadValue<float>());
        zoomDelta = 0;
        currentZoom = Camera.main.orthographicSize;
        zoomTarget = Mathf.Clamp(Mathf.Max(zoomTarget - playerInputs.PlayerMap.Zoom.ReadValue<float>() * 1f, 0), MinMaxZoom.x, MinMaxZoom.y);

    }

}
