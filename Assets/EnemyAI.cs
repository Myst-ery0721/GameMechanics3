using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 4f;
    public float knockbackForce = 10f;

    private Rigidbody rb;
    private bool isKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isKnockedBack) // Only chase if not knocked back
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item")) // If hit by a thrown object
        {
            Vector3 hitDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(hitDirection * knockbackForce, ForceMode.Impulse); // Apply force

            isKnockedBack = true; // Stops the enemy permanently
        }
    }
}