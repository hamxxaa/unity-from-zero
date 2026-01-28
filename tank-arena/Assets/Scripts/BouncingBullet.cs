using UnityEngine;

public class BouncingBullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed = 20f;
    [SerializeField] int maxBounces = 1;
    [SerializeField] float maxLifetime = 3f;
    [SerializeField] LayerMask collisionMask;

    private int currentBounces = 0;

    void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    void Update()
    {
        MoveAndCheckCollision();
    }

    void MoveAndCheckCollision()
    {
        float moveDistance = speed * Time.deltaTime;

        Debug.DrawRay(transform.position, transform.forward * moveDistance, Color.magenta, 0.1f);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask))
        {
            HandleHit(hit);
        }
        else
        {
            transform.Translate(Vector3.forward * moveDistance);
        }
    }

    void HandleHit(RaycastHit hit)
    {
        transform.position = hit.point;

        IDamageable damageable = hit.transform.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(20);
            Destroy(gameObject);
            return;
        }

        if (currentBounces < maxBounces)
        {
            Bounce(hit.normal);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Bounce(Vector3 collisionNormal)
    {
        Vector3 reflectDir = Vector3.Reflect(transform.forward, collisionNormal);

        transform.rotation = Quaternion.LookRotation(reflectDir);

        currentBounces++;
    }
}