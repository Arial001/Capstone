<<<<<<< HEAD
using UnityEngine;

public class DPadZones : MonoBehaviour
{
    public float deadZoneRadius = 0.3f;

    private Camera cam;
    private SpriteRenderer sr;
    private DPadButton.Direction? currentDirection = null;
    private bool pressing = false;

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        pressing = true;
        DPadButton.Direction? dir = GetDirectionFromClick();
        currentDirection = dir;
        if (dir.HasValue)
            PlayerMover.Instance.StartHolding(dir.Value);
    }

    void Update()
    {
        if (pressing && Input.GetMouseButton(0))
        {
            DPadButton.Direction? dir = GetDirectionFromClick();
            if (currentDirection != dir)
            {
                if (currentDirection.HasValue)
                    PlayerMover.Instance.StopHolding(currentDirection.Value);

                currentDirection = dir;

                if (dir.HasValue)
                    PlayerMover.Instance.StartHolding(dir.Value);
            }
        }

        if (pressing && Input.GetMouseButtonUp(0))
        {
            pressing = false;
            if (currentDirection.HasValue)
                PlayerMover.Instance.StopHolding(currentDirection.Value);
            currentDirection = null;
        }
    }

    DPadButton.Direction? GetDirectionFromClick()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z;

        Vector3 center = (sr != null) ? sr.bounds.center : transform.position;
        Vector3 offset = mouseWorld - center;

        if (offset.magnitude < deadZoneRadius)
            return null;

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        if (angle >= 45 && angle < 135) return DPadButton.Direction.Up;
        if (angle >= 135 && angle < 225) return DPadButton.Direction.Left;
        if (angle >= 225 && angle < 315) return DPadButton.Direction.Down;
        return DPadButton.Direction.Right;
    }
=======
using UnityEngine;

public class DPadZones : MonoBehaviour
{
    public float deadZoneRadius = 0.3f;

    private Camera cam;
    private SpriteRenderer sr;
    private DPadButton.Direction? currentDirection = null;
    private bool pressing = false;

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        pressing = true;
        DPadButton.Direction? dir = GetDirectionFromClick();
        currentDirection = dir;
        if (dir.HasValue)
            PlayerMover.Instance.StartHolding(dir.Value);
    }

    void Update()
    {
        if (pressing && Input.GetMouseButton(0))
        {
            DPadButton.Direction? dir = GetDirectionFromClick();
            if (currentDirection != dir)
            {
                if (currentDirection.HasValue)
                    PlayerMover.Instance.StopHolding(currentDirection.Value);

                currentDirection = dir;

                if (dir.HasValue)
                    PlayerMover.Instance.StartHolding(dir.Value);
            }
        }

        if (pressing && Input.GetMouseButtonUp(0))
        {
            pressing = false;
            if (currentDirection.HasValue)
                PlayerMover.Instance.StopHolding(currentDirection.Value);
            currentDirection = null;
        }
    }

    DPadButton.Direction? GetDirectionFromClick()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z;

        // pakai titik tengah VISUAL sprite (bounds.center), bukan transform.position,
        // supaya tidak terpengaruh pivot yang offset
        Vector3 center = (sr != null) ? sr.bounds.center : transform.position;
        Vector3 offset = mouseWorld - center;

        if (offset.magnitude < deadZoneRadius)
            return null;

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        if (angle >= 45 && angle < 135) return DPadButton.Direction.Up;
        if (angle >= 135 && angle < 225) return DPadButton.Direction.Left;
        if (angle >= 225 && angle < 315) return DPadButton.Direction.Down;
        return DPadButton.Direction.Right;
    }
>>>>>>> 943d557c649507990c639cf23da8aab4414eb84c
}