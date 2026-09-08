using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text ufosRemainingText;

    void Update()
    {
        scoreText.text = $"Score: {GameManager.Instance.Score}";
        timerText.text = $"Time: {Mathf.CeilToInt(GameManager.Instance.TimeRemaining)}";
        ufosRemainingText.text = $"UFOs: {GameManager.Instance.UFOsRemaining}";
    }
    
}