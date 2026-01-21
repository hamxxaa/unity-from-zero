using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float damage = 10f;
    private Transform target;


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        // If we don't have a target, do nothing (to avoid errors)
        if (target == null) return;

        // Provides frame rate independent constant speed.
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.1f)
        {
            Health baseHealth = target.GetComponent<Health>();
            if (baseHealth != null)
            {
                baseHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy self
        }
    }
}