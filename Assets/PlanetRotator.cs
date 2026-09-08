
using UnityEngine;
 
public class PlanetRotator : MonoBehaviour
{
    public float rotateSpeed = 3f; // degrees per second on Y axis
                                   // speed 3 = full rotation every ~2 minutes
 
    void Update()
    {
        // Slight X tilt (0.2) gives it a natural axial rotation feel
        // Y rotation is the main visible spin
        transform.Rotate(0.2f, rotateSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
 
