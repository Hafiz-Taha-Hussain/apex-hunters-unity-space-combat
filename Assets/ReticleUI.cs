using UnityEngine;
using UnityEngine.UI;

public class ReticleUI : MonoBehaviour
{
    public WeaponLockSystem weaponLockSystem;
    public Image fillImage;

    public Color trackingColor = Color.red;
    public Color lockedColor = Color.green;

    void Update()
    {
        float progress = weaponLockSystem.LockProgress;
        fillImage.fillAmount = progress;
        fillImage.color = weaponLockSystem.IsLocked ? lockedColor : trackingColor;
        
    }
}