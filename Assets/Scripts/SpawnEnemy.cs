using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Tooltip("The enemy prefab to spawn.")]
    public GameObject enemyPrefab;

    [Tooltip("The positions where the enemies will be spawned.")]
    public Transform[] spawnPoints;

    [Tooltip("The number of enemies to spawn.")]
    public int numberOfEnemiesToSpawn = 5;

    private bool playerEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!playerEntered && other.CompareTag("Player"))
        {
            playerEntered = true;
            SpawnEnemies();
            Destroy(gameObject);
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            int spawnIndex = i % spawnPoints.Length; // Loop through spawn points if numberOfEnemiesToSpawn > spawnPoints.Length
            Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        }
    }
}
