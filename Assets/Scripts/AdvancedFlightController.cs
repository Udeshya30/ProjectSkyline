using UnityEngine;

public class AdvancedFlightController : MonoBehaviour
{
    public float speed = 20f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float maxSpeed = 50f;
    public float minSpeed = 5f;

    public float turnSpeed = 50f;
    public float rollAmount = 30f;    // How much the plane rolls while turning
    public float pitchAmount = 15f;   // How much the plane pitches based on speed
    public float smoothRotation = 2f; // How smooth rotation changes

    private float yaw;
    private float pitch;
    private float roll;
    private float targetRoll;

    void Update()
    {
        HandleInput();
        MovePlane();
    }

    void HandleInput()
    {
        // Speed control
        if (Input.GetKey(KeyCode.UpArrow))
        {
            speed += acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            speed -= deceleration * Time.deltaTime;
        }
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        // Turning and Rolling
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            yaw = -turnSpeed;
            targetRoll = rollAmount; // Tilt Left
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            yaw = turnSpeed;
            targetRoll = -rollAmount; // Tilt Right
        }
        else
        {
            yaw = 0;
            targetRoll = 0; // Go back to level
        }

        // Smooth the roll
        roll = Mathf.Lerp(roll, targetRoll, Time.deltaTime * smoothRotation);

        // Pitch a bit based on speed
        pitch = Mathf.Lerp(pitch, (speed / maxSpeed) * pitchAmount, Time.deltaTime * smoothRotation);
    }

    void MovePlane()
    {
        // Move Forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Rotate the plane
        Quaternion targetRotation = Quaternion.Euler(-pitch, transform.eulerAngles.y + yaw * Time.deltaTime, roll);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotation);
    }
}
