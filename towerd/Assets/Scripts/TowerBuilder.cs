using UnityEngine;
using UnityEngine.InputSystem;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] GameObject turretPrefab;
    [SerializeField] LayerMask groundLayer;

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentState != GameManager.GameState.Playing)
    {
        return;
    }
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            BuildTurret();
        }
    }

    void BuildTurret()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 spawnPosition = hit.point;

            spawnPosition.y += 0.25f;

            Instantiate(turretPrefab, spawnPosition, Quaternion.identity);
        }
    }
}