using UnityEngine;
using TMPro;

public class LosePanelUI : MonoBehaviour
{
    public TMP_Text loseMessageText;

    public void ShowLoseMessage(int retriesRemaining, bool demoted)
    {
        loseMessageText.text = demoted
            ? "Failed 3 times — back to Level 1!"
            : $"Retries remaining: {retriesRemaining}";
    }
}
