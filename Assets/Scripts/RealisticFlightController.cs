using UnityEngine;
using TMPro;

public class RealisticFlightController : MonoBehaviour
{
    [Header("Speed Settings")]
    public float speed = 50f;
    public float acceleration = 10f;
    public float deceleration = 8f;
    public float maxSpeed = 100f;
    public float minSpeed = 20f;
    public float stallSpeed = 15f;

    [Header("Rotation Settings")]
    public float turnSpeed = 50f;            // Manual yaw speed
    public float pitchSpeed = 30f;            // How fast you pitch up/down
    public float rollAmount = 45f;            // How much plane rolls left/right
    public float smoothRotation = 3f;         // Smoothing for rotation
    public float bankedTurnMultiplier = 2f;   // Yaw added based on bank (automatic)

    [Header("Flight Physics")]
    public float gravityForce = 9.81f;        // Gravity pull
    public float liftForce = 12f;              // Lift upwards
    public float autoLevelSpeed = 2f;          // Auto-level roll when not pressing keys

    [Header("UI Elements")]
    public TextMeshProUGUI altitudeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI headingText;
    public TextMeshProUGUI warningText;        // Optional: Warning when too low

    [Header("Altitude Settings")]
    public LayerMask terrainLayerMask;         // Set this to the "Terrain" Layer
    public float lowAltitudeThreshold = 20f;   // Altitude warning threshold
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;

    private float yaw;
    private float pitch;
    private float roll;
    private float targetRoll;
    private Vector3 velocity;

    // Smooth display values
    private float displayedAltitude = 0f;
    private float displayedSpeed = 0f;
    private float displayedHeading = 0f;

    void Update()
    {
        HandleInput();
        ApplyPhysics();
        MovePlane();
        UpdateHUD();
    }

    void HandleInput()
    {
        // --- Roll Control (LEFT/RIGHT arrows) ---
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            targetRoll = rollAmount; // Roll left
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            targetRoll = -rollAmount; // Roll right
        }
        else
        {
            targetRoll = 0f; // Auto-level roll
        }
        roll = Mathf.Lerp(roll, targetRoll, Time.deltaTime * smoothRotation);

        // --- Pitch Control (UP/DOWN arrows) ---
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pitch -= pitchSpeed * Time.deltaTime; // Tilt nose down
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            pitch += pitchSpeed * Time.deltaTime; // Tilt nose up
        }
        pitch = Mathf.Clamp(pitch, -45f, 45f); // Limit pitch angle

        // --- Throttle Control (W/S keys) ---
        if (Input.GetKey(KeyCode.W))
        {
            speed += acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            speed -= deceleration * Time.deltaTime;
        }
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
    }

    void ApplyPhysics()
    {
        // Gravity
        velocity += Vector3.down * gravityForce * Time.deltaTime;

        // Lift
        float lift = Mathf.Clamp01(Vector3.Dot(transform.up, Vector3.up));
        velocity += transform.up * lift * liftForce * Time.deltaTime;

        // Stall
        if (speed <= stallSpeed)
        {
            velocity += Vector3.down * gravityForce * 2f * Time.deltaTime;
        }
    }

    void MovePlane()
    {
        // Forward movement
        transform.position += transform.forward * speed * Time.deltaTime;

        // Banked Yaw (auto turn while rolling)
        float bankedYaw = -roll * bankedTurnMultiplier * Time.deltaTime;
        yaw = bankedYaw;

        // Full rotation from pitch, yaw and roll
        Quaternion targetRotation = Quaternion.Euler(pitch, transform.eulerAngles.y + yaw, roll);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotation);

        // Apply gravity and lift effects
        transform.position += velocity * Time.deltaTime;

        // Dampen velocity
        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 1f);
    }

    void UpdateHUD()
    {
        if (altitudeText)
        {
            float currentAltitude = 0f;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, terrainLayerMask))
            {
                currentAltitude = hit.distance;
            }
            else
            {
                currentAltitude = 0f;
            }

            // Smooth transition
            displayedAltitude = Mathf.Lerp(displayedAltitude, currentAltitude, Time.deltaTime * 5f);
            altitudeText.text = "Altitude: " + Mathf.RoundToInt(displayedAltitude) + " m";

            // 🚨 Altitude Warning
            if (displayedAltitude <= lowAltitudeThreshold)
            {
                altitudeText.color = warningColor;
                if (warningText) {
                    // warningText.enabled = true;
                    warningText.gameObject.SetActive(true);
                }
            }
            else
            {
                altitudeText.color = normalColor;
                if (warningText) warningText.gameObject.SetActive(false);
            }
        }

        if (speedText)
        {
            displayedSpeed = Mathf.Lerp(displayedSpeed, speed, Time.deltaTime * 5f);
            //speedText.text = "Speed: " + Mathf.RoundToInt(displayedSpeed) + " km/h";
            speedText.text = "Speed: 3,540 km/h";
        }

        if (headingText)
        {
            float currentHeading = transform.eulerAngles.y;
            displayedHeading = Mathf.LerpAngle(displayedHeading, currentHeading, Time.deltaTime * 5f);
            headingText.text = "Heading: " + Mathf.RoundToInt(displayedHeading) + "°";
        }
    }
}
