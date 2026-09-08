using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    public float matchTime = 60f;

    private int totalUFOs;
    private int ufosDestroyed;
    private float timeRemaining;
    private int score;
    private bool gameOver = false;

    public int Score => score;
    public float TimeRemaining => timeRemaining;
    public int UFOsRemaining => totalUFOs - ufosDestroyed;
    public bool IsGameOver => gameOver;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Level Settings")]
    public int currentLevel = 1;
    public int level1UFOCount = 4;
    public int level2UFOCount = 7;
    public float level1MatchTime = 60f;
    public float level2MatchTime = 70f;

    [Header("Retry Settings")]
    public int maxRetries = 2;
    private int retryCount = 0;

    public int RetriesRemaining => maxRetries - retryCount;

    [Header("Level Visuals")]
    public Material level2Skybox;
    public Material level1Skybox;

    [Header("Win/Lose Sounds")]
    public AudioSource uiAudioSource;
    public AudioClip winSound;
    public AudioClip loseSound;

    public LosePanelUI loseMessageUI; // the script sitting on LosePanel with ShowLoseMessage()

    public UFOSpawner ufoSpawner;

    void WinGame()
    {
        if (currentLevel == 1)
        {
            SceneManager.LoadScene("Level2"); // CHANGED - load info screen instead of switching in-place
        }
        else
        {
            gameOver = true;
            winPanel.SetActive(true);
            uiAudioSource.PlayOneShot(winSound);
        }
    }

    void LoseGame()
    {
        gameOver = true;
        uiAudioSource.PlayOneShot(loseSound); // NEW - plays either way, retry or demotion

        if (retryCount < maxRetries)
        {
            retryCount++;
            losePanel.SetActive(true); // shows "Try Again" state, retries remaining
            loseMessageUI.ShowLoseMessage(RetriesRemaining, false);
        }
        else
        {
            // Failed 3rd time total - demote to Level 1
            currentLevel = 1;
            retryCount = 0;
            losePanel.SetActive(true); // shows "Back to Level 1" state
            loseMessageUI.ShowLoseMessage(RetriesRemaining, true);
        }
    }

    public void RestartLevel()
    {
        gameOver = false;
        losePanel.SetActive(false);
        ufosDestroyed = 0;
        score = 0;

        if (currentLevel == 1)
        {
            timeRemaining = level1MatchTime;
            ufoSpawner.numberToSpawn = level1UFOCount;
            RenderSettings.skybox = level1Skybox;; // or reference your original Level 1 skybox material here
        }
        else
        {
            timeRemaining = level2MatchTime;
            ufoSpawner.numberToSpawn = level2UFOCount;
            RenderSettings.skybox = level2Skybox;
        }

        DynamicGI.UpdateEnvironment();

        // Destroy any surviving UFOs from the failed attempt before respawning
        foreach (var ufo in FindObjectsByType<UFOController>(FindObjectsSortMode.None))
        {
            Destroy(ufo.gameObject);
        }

        ufoSpawner.SpawnAllUFOs();
    }   

    void Start()
    {
        int startingLevel = PlayerPrefs.GetInt("StartingLevel", 1); // defaults to 1 if not set
        currentLevel = startingLevel;

        if (currentLevel == 1)
        {
            timeRemaining = level1MatchTime;
            ufoSpawner.numberToSpawn = level1UFOCount;
            RenderSettings.skybox = level1Skybox;
        }
        else
        {
            timeRemaining = level2MatchTime;
            ufoSpawner.numberToSpawn = level2UFOCount;
            RenderSettings.skybox = level2Skybox;
        }

        DynamicGI.UpdateEnvironment();
        ufoSpawner.SpawnAllUFOs();
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (gameOver) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            LoseGame();
        }
    }

    public void SetTotalUFOs(int count)
    {
        totalUFOs = count;
    }

    public void RegisterUFODestroyed()
    {
        if (gameOver) return;

        ufosDestroyed++;
        score += 100; // adjust point value as you like

        if (ufosDestroyed >= totalUFOs)
        {
            gameOver = true; // stop ship/UFO/weapon input immediately, but don't transition yet
            StartCoroutine(WinGameDelayed());
        }
    }

    System.Collections.IEnumerator WinGameDelayed()
    {
        yield return new WaitForSeconds(2f); // gives time for laser + explosion sound to finish
        WinGame();
    }

    
}