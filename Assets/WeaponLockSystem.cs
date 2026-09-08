using UnityEngine;

public class WeaponLockSystem : MonoBehaviour
{
    [Header("Aim Source")]
    public Transform aimOrigin; // assign your camera here

    [Header("Lock Settings")]
    public float maxLockDistance = 200f;
    public float lockTime = 1.5f;
    public LayerMask ufoLayerMask;

    [Header("Fire")]
    public KeyCode fireKey = KeyCode.Mouse0;

    private Transform currentTarget;
    private float lockTimer;
    public bool IsLocked { get; private set; }
    public float LockProgress => currentTarget != null ? Mathf.Clamp01(lockTimer / lockTime) : 0f;

    [Header("Weapon Effects")]
    public LineRenderer laserLine;
    public Transform gunPoint; // empty child object at ship's nose
    public float laserDuration = 0.05f;

    public AudioSource weaponAudioSource;
    public AudioClip fireSound;
    public AudioClip explosionSound;

    void Update()
    {

        if (GameManager.Instance.IsGameOver) return;

        Transform detectedTarget = null;

        Ray ray = new Ray(aimOrigin.position, aimOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxLockDistance, ufoLayerMask))
        {
            detectedTarget = hit.transform;
        }

        if (detectedTarget != null && detectedTarget == currentTarget)
        {
            // Still tracking the same target, keep building lock
            lockTimer += Time.deltaTime;
        }
        else
        {
            // Target changed or lost - reset
            currentTarget = detectedTarget;
            lockTimer = 0f;
            IsLocked = false;
        }

        if (currentTarget != null && lockTimer >= lockTime)
        {
            IsLocked = true;
        }

        if (IsLocked && Input.GetKeyDown(fireKey))
        {
            Fire();
        }

        // Debug.Log($"Target: {detectedTarget}, LockProgress: {LockProgress}");

    }


    void Fire()
    {
        if (currentTarget != null)
        {

            weaponAudioSource.PlayOneShot(fireSound);
            weaponAudioSource.PlayOneShot(explosionSound);
            
            StartCoroutine(FireLaser(currentTarget.position));

            GameManager.Instance.RegisterUFODestroyed(); 
            Destroy(currentTarget.gameObject);
            currentTarget = null;
            lockTimer = 0f;
            IsLocked = false;
        }
    }

    System.Collections.IEnumerator FireLaser(Vector3 targetPos)
    {
        laserLine.positionCount = 2;
        laserLine.SetPosition(0, gunPoint.position);
        laserLine.SetPosition(1, targetPos);
        laserLine.enabled = true;

        yield return new WaitForSeconds(laserDuration);

        laserLine.enabled = false;
    }
}