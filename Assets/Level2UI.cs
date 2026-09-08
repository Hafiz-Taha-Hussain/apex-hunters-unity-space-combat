using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2UI : MonoBehaviour
{
    public void OnStartGameClicked()
    {
        PlayerPrefs.SetInt("StartingLevel", 2);
        SceneManager.LoadScene("Game");
    }
}
