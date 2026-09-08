using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ufoPrefab;
    public Transform player;
    public int numberToSpawn = 5;
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(50f, 20f, 50f);

    // [Header("Player Reference")]
    //  // NEW - drag PlayerShip into this in Inspector

    public void SpawnAllUFOs()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 randomPos = spawnAreaCenter + new Vector3(
                Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
                Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
            );

            GameObject ufoInstance = Instantiate(ufoPrefab, randomPos, Quaternion.identity);

            // NEW - manually wire the player reference since prefabs can't hold scene refs
            UFOController controller = ufoInstance.GetComponent<UFOController>();
            controller.player = player;
        }

        GameManager.Instance.SetTotalUFOs(numberToSpawn);
    }
}