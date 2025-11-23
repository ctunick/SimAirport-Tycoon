using UnityEngine;

namespace SimAirport.Core
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Settings")]
        public int width = 50;
        public int height = 50;
        public float cellSize = 1f;
        public Vector3 originPosition = Vector3.zero;

        // Tracks which cells are occupied by a building
        private GameObject[,] gridOccupants;

        private void Awake()
        {
            gridOccupants = new GameObject[width, height];
        }

        public bool IsInsideGrid(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        public bool IsCellFree(int x, int y)
        {
            if (!IsInsideGrid(x, y)) return false;
            return gridOccupants[x, y] == null;
        }

        public void SetOccupant(int x, int y, GameObject occupant)
        {
            if (!IsInsideGrid(x, y)) return;
            gridOccupants[x, y] = occupant;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            // Center of the cell
            return originPosition + new Vector3(
                (x + 0.5f) * cellSize,
                0f,
                (y + 0.5f) * cellSize
            );
        }

        public bool TryGetGridPosition(Vector3 worldPosition, out int x, out int y)
        {
            Vector3 local = worldPosition - originPosition;

            x = Mathf.FloorToInt(local.x / cellSize);
            y = Mathf.FloorToInt(local.z / cellSize);

            if (!IsInsideGrid(x, y))
            {
                return false;
            }

            return true;
        }

        private void OnDrawGizmosSelected()
        {
            // Simple visual debug of the grid
            Gizmos.color = Color.gray;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 center = originPosition + new Vector3(
                        (x + 0.5f) * cellSize,
                        0f,
                        (y + 0.5f) * cellSize
                    );
                    Vector3 size = new Vector3(cellSize, 0.01f, cellSize);
                    Gizmos.DrawWireCube(center, size);
                }
            }
        }
    }
}