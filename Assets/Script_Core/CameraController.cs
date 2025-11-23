using UnityEngine;

namespace SimAirport.Core {
    /// <summary>
    /// Simple top-down / 3D camera controller:
    /// - WASD / Arrow keys to pan
    /// - Mouse scroll wheel to zoom in/out (changes height)
    /// - Q / E to rotate around Y axis
    /// </summary>
    public class CameraController : MonoBehaviour {
        [Header("Pan")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float fastMoveMultiplier = 2f; // hold Left Shift to move faster

        [Header("Zoom (Y-axis)")]
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float minHeight = 5f;
        [SerializeField] private float maxHeight = 30f;

        [Header("Rotate (Q/E)")]
        [SerializeField] private float rotationSpeed = 90f; // degrees per second

        private void Update() {
            HandleRotate();
            HandlePan();
            HandleZoom();
        }

        private void HandlePan() {
            // WASD / Arrow keys use Unity's Horizontal/Vertical axes by default
            float h = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
            float v = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

            if (Mathf.Approximately(h, 0f) && Mathf.Approximately(v, 0f))
                return;

            // Move in the camera's XZ plane (ignore Y in the direction vectors)
            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = transform.right;
            right.y = 0f;
            right.Normalize();

            Vector3 direction = (forward * v + right * h).normalized;

            float speed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                speed *= fastMoveMultiplier;
            }

            transform.position += direction * speed * Time.deltaTime;
        }

        private void HandleZoom() {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) < 0.001f)
                return;

            Vector3 pos = transform.position;
            pos.y = Mathf.Clamp(pos.y - scroll * zoomSpeed, minHeight, maxHeight);
            transform.position = pos;
        }

        private void HandleRotate() {
            float rotateDir = 0f;

            if (Input.GetKey(KeyCode.Q)) {
                rotateDir -= 1f; // rotate left
            }
            if (Input.GetKey(KeyCode.E)) {
                rotateDir += 1f; // rotate right
            }

            if (Mathf.Approximately(rotateDir, 0f))
                return;

            // Rotate around global Y so you orbit the world, not tilt
            transform.Rotate(Vector3.up, rotateDir * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
