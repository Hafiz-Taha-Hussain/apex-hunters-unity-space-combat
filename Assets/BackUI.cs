using UnityEngine;
using UnityEngine.SceneManagement;

public class BackUI : MonoBehaviour
{
    public void OnBackClicked()
{
    SceneManager.LoadScene("MainMenu");
}
}
