using UnityEngine;
using UnityEngine.InputSystem;
using SimAirport.Core;

namespace SimAirport.Building
{
    public class BuildModeController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GridManager gridManager;
        [SerializeField] private BuildableDefinition gateBasicDefinition;

        [Header("Debug Passenger Hook")]
        [Tooltip("Temporary: old GatePoint Transform that passengers currently walk to.")]
        [SerializeField] private Transform debugGatePoint;

        [Header("State")]
        [SerializeField] private bool isBuildModeActive = false;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("[BuildModeController] No camera tagged MainCamera found.");
            }
        }

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            // Toggle build mode with B
            if (keyboard.bKey.wasPressedThisFrame)
            {
                isBuildModeActive = !isBuildModeActive;
                Debug.Log($"[BuildModeController] Build mode: {isBuildModeActive}");
            }

            if (!isBuildModeActive) return;

            var mouse = Mouse.current;
            if (mouse == null) return;

            // Place a gate on left click
            if (mouse.leftButton.wasPressedThisFrame)
            {
                HandleBuildClick();
            }
        }

        private void HandleBuildClick()
        {
            if (mainCamera == null || gridManager == null || gateBasicDefinition == null)
            {
                Debug.LogWarning("[BuildModeController] Missing references (camera/gridManager/gateBasicDefinition).");
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 500f))
            {
                Vector3 hitPoint = hitInfo.point;
                Debug.Log($"[BuildModeController] Raycast hit '{hitInfo.collider.name}' at {hitPoint}");

                if (gridManager.TryGetGridPosition(hitPoint, out int x, out int y))
                {
                    if (!gridManager.IsCellFree(x, y))
                    {
                        Debug.Log($"[BuildModeController] Cell {x},{y} is already occupied.");
                        return;
                    }

                    Vector3 spawnPos = gridManager.GetWorldPosition(x, y);
                    GameObject instance = Instantiate(gateBasicDefinition.prefab, spawnPos, Quaternion.identity);

                    gridManager.SetOccupant(x, y, instance);
                    Debug.Log($"[BuildModeController] Placed GateBasic at {x},{y}");

                    // 🔗 NEW: update the old debug GatePoint so passengers walk to this gate
                    if (debugGatePoint != null)
                    {
                        // Prefer the EntryPoint child if it exists
                        Transform target = instance.transform;
                        Transform entryPoint = instance.transform.Find("EntryPoint");
                        if (entryPoint != null)
                        {
                            target = entryPoint;
                        }

                        debugGatePoint.position = target.position;
                        Debug.Log($"[BuildModeController] Updated debug gate point to {debugGatePoint.position}");
                    }
                }
                else
                {
                    Vector3 local = hitPoint - gridManager.originPosition;
                    int gx = Mathf.FloorToInt(local.x / gridManager.cellSize);
                    int gy = Mathf.FloorToInt(local.z / gridManager.cellSize);

                    Debug.Log(
                        $"[BuildModeController] Outside grid. " +
                        $"Hit: {hitPoint}, Origin: {gridManager.originPosition}, " +
                        $"Local: {local}, Cell guess: ({gx},{gy}), " +
                        $"Grid size: {gridManager.width}x{gridManager.height}"
                    );
                }
            }
        }
    }
}