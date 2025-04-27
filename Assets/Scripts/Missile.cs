using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 200f;
    public float lifetime = 5f;
    public GameObject explosionEffect; // Assign explosion particle prefab here
    public LayerMask terrainLayer;     // Assign Terrain Layer in Inspector
    public float terrainDetectionDistance = 1f;  // Distance to check for terrain

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after 5 seconds anyway
    }

    private void Update()
    {
        // Move the missile forward
        transform.position += transform.forward * speed * Time.deltaTime;

        // Perform raycast to detect terrain collision
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, terrainDetectionDistance, terrainLayer))
        {
            // If ray hits terrain, trigger the explosion
            Explode();
            print("Hit Terrain: " + hit.collider.gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Optional: Hit Enemy
        if (other.CompareTag("Enemy"))
        {
            Explode();
            Destroy(other.gameObject);
        }
        else if (((1 << other.gameObject.layer) & terrainLayer) != 0)
        {
            // If we hit terrain layer
            Explode();
            print("Hit Terrain: " + other.gameObject.name);
        }
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
        }
        Destroy(gameObject);
    }
}
