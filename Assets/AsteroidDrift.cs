using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidDrift : MonoBehaviour
{
    public float minRotSpeed = 5f;
    public float maxRotSpeed = 25f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 spinAxis = Random.onUnitSphere;
        float spinSpeed = Random.Range(minRotSpeed, maxRotSpeed);
        rb.angularVelocity = spinAxis * spinSpeed * Mathf.Deg2Rad;
    }
}