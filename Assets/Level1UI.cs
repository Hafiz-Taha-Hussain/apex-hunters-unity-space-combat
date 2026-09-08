using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1UI : MonoBehaviour
{
    public void OnStartGameClicked()
    {
        PlayerPrefs.SetInt("StartingLevel", 1);
        SceneManager.LoadScene("Game");
    }
}
