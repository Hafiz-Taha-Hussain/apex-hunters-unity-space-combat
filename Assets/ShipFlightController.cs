using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipFlightController : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float thrustForce = 20f;
    public float pitchSpeed = 60f;
    public float yawSpeed = 60f;
    public float rollSpeed = 90f;

    public AudioSource thrusterAudioSource;
    public float maxThrusterVolume = 0.6f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        if (GameManager.Instance.IsGameOver) return;
        // Thrust forward/backward - W/S or Up/Down arrows
        float thrustInput = Input.GetAxis("Vertical");
        rb.AddForce(transform.forward * thrustInput * thrustForce);

        // NEW - thruster sound reacts to actual input
        if (Mathf.Abs(thrustInput) > 0.1f)
        {
            if (!thrusterAudioSource.isPlaying)
                thrusterAudioSource.Play();
            thrusterAudioSource.volume = Mathf.Abs(thrustInput) * maxThrusterVolume;
        }
        else
        {
            thrusterAudioSource.Stop();
        }

        // Pitch (nose up/down) - mouse Y or arrow keys
        float pitchInput = -Input.GetAxis("Mouse Y");
        // Yaw (turn left/right) - mouse X
        float yawInput = Input.GetAxis("Mouse X");
        // Roll (bank left/right) - Q/E keys
        float rollInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rollInput = 1f;
        if (Input.GetKey(KeyCode.E)) rollInput = -1f;

        Vector3 torque = new Vector3(pitchInput * pitchSpeed, yawInput * yawSpeed, rollInput * rollSpeed);
        rb.AddTorque(transform.TransformDirection(torque) * Time.fixedDeltaTime * 50f);

        
        Vector3 pos = transform.position;
        float boundary = 1100f; // adjust to match your spawn area size
        pos.x = Mathf.Clamp(pos.x, -boundary, boundary);
        pos.y = Mathf.Clamp(pos.y, -boundary, boundary);
        pos.z = Mathf.Clamp(pos.z, -boundary, boundary);
        transform.position = pos;

    }
}