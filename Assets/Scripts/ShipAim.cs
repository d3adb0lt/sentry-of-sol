using UnityEngine;
using UnityEngine.InputSystem;

public class ShipAim : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 720f; // degrees per second

    private Camera _cam;
    private float _targetAngle;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        // While the screen is touched (or the mouse is held), aim at that point
        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = _cam.ScreenToWorldPoint(screenPos);
            Vector2 dir = worldPos - transform.position;

            _targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        }

        // Turn smoothly toward the target instead of snapping
        float z = Mathf.MoveTowardsAngle(transform.eulerAngles.z, _targetAngle,
            rotateSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, z);
    }
}