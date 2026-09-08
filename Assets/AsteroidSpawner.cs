using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] asteroidPrefabs; // supports multiple asteroid variants
    public int numberToSpawn;
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(100f, 40f, 100f);

    [Header("Random Scale/Rotation")]
    public float minScale;
    public float maxScale;

    void Start()
    {
        SpawnAllAsteroids();
    }

    void SpawnAllAsteroids()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 randomPos = spawnAreaCenter + new Vector3(
                Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
                Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
            );

            // Pick a random prefab if you have multiple variants
            GameObject prefabToSpawn = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

            GameObject asteroid = Instantiate(prefabToSpawn, randomPos, Random.rotation);

            // Random uniform scale for size variety
            float randomScale = Random.Range(minScale, maxScale);
            asteroid.transform.localScale = Vector3.one * randomScale;

            Rigidbody rb = asteroid.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.mass = randomScale * 2f; // bigger asteroids are harder to push - feels right physically
            rb.linearDamping = 0.5f;
            rb.angularDamping = 0.5f;

            asteroid.AddComponent<SphereCollider>(); // or BoxCollider if your asteroid shape is more box-like

            asteroid.AddComponent<AsteroidDrift>();
            
        }
    }
}