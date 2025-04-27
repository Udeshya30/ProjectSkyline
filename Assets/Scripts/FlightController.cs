using UnityEngine;

public class FlightController : MonoBehaviour
{
    public float speed = 10f;           // Current speed
    public float acceleration = 2f;     // How fast to speed up/slow down
    public float turnSpeed = 50f;        // How fast to turn left/right

    void Update()
    {
        // Speed control
        if (Input.GetKey(KeyCode.UpArrow))
        {
            speed += acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            speed -= acceleration * Time.deltaTime;
            speed = Mathf.Max(0, speed); // Prevent going backwards
        }

        // Turning
        float turn = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turn = -turnSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            turn = turnSpeed * Time.deltaTime;
        }
        transform.Rotate(0, turn, 0);

        // Move forward constantly
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
