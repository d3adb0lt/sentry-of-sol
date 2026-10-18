using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ShipAim : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 720f;
    [SerializeField] private float fireAngleTolerance = 8f;
    
    public bool IsAiming { get; private set; }
    public bool IsAligned =>
        IsAiming &&
        Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, _targetAngle)) <= fireAngleTolerance;

    private static readonly List<RaycastResult> uiHits = new(); // reusable, so as to not include garbo each frame
    
    private Camera _cam;
    private float _targetAngle;
    private bool _pressStartedOnUI;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        Pointer pointer = Pointer.current;
        bool pressed = pointer != null && pointer.press.isPressed;
        
        if (pressed && pointer.press.wasPressedThisFrame)
            _pressStartedOnUI = IsOverUI(pointer.position.ReadValue());
        
        IsAiming = pressed && !_pressStartedOnUI;
        
        // While the screen is touched (or the mouse is held), aim at that point
        if (IsAiming)
        {
            Vector2 screenPos = pointer.position.ReadValue();
            Vector3 worldPos = _cam.ScreenToWorldPoint(screenPos);
            Vector2 dir = worldPos - transform.position;

            _targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        }

        // Turn smoothly toward the target instead of snapping
        float z = Mathf.MoveTowardsAngle(transform.eulerAngles.z, _targetAngle,
            rotateSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, z);
    }
    
    static bool IsOverUI(Vector2 screenPos)
    {
        if (EventSystem.current == null) return false;
        
        var data = new PointerEventData(EventSystem.current) { position = screenPos };
        uiHits.Clear();
        EventSystem.current.RaycastAll(data, uiHits);
        return uiHits.Count > 0;
    }
}