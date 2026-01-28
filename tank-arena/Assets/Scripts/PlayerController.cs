using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // Singleton
    public static PlayerController Instance;

    [Header("Settings")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotationSpeed = 5f;

    [Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform turretTransform;
    [SerializeField] LayerMask groundLayer;

    [Header("Attack Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private TankControls controls;
    private Vector2 moveInput;



    void Awake()
    {
        controls = new TankControls();
    }

    void OnEnable()
    {
        controls.Enable();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Fire.performed += OnFire;
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void OnFire(InputAction.CallbackContext ctx)
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        CameraShake.Instance?.Shake(0.05f, 0.03f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.shootSound, 0.5f);
    }


    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }


    void Update()
    {
        HandleTurretRotation();
    }

    void HandleTurretRotation()
    {
        Vector2 mouseScreenPos = controls.Player.Look.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPoint = hit.point;
            targetPoint.y = turretTransform.position.y;

            turretTransform.LookAt(targetPoint);
        }
    }
}