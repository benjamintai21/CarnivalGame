using UnityEngine;

public class DuckSpawner : MonoBehaviour
{
    public GameObject duckPrefab; // Assign your prefab in Inspector
    public int rows = 4;          // 1 for a line, more for a grid
    public int columns = 1;
    public float spacing = 0.8f;

    public float moveSpeed = 0.2f; // movement speed
    public float moveDistance = 4.0f; // move distance

    void Start()
    {
        
    }

    public void SpawnDucks()
    {
        Vector3 startPos = transform.position;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 spawnPos = startPos + new Vector3(col * spacing, 0, row * spacing);
                Instantiate(duckPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
    void Update()
    {
        // update position and rotation
        transform.position = new Vector3(transform.position.x,
        transform.position.y, moveSpeed * Mathf.Sin(Time.time) * moveDistance);
    }
}
