using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Trap"))
        {
            Die();
        }
    }
    private void Die()
    {
        transform.position = startPosition;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

}
