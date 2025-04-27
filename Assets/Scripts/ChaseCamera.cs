using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 8, -30); // Higher and farther back
    public float rotationSpeed = 5f;   // Only rotation is smoothed now

    void LateUpdate()
    {
        if (!target) return;

        // Instantly follow the plane position (no lag)
        transform.position = target.position + target.TransformDirection(offset);

        // Smoothly rotate to look at the plane
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
