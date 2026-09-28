using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public static PlayerMover Instance;

    public float speed = 5f;

    private bool moveUp, moveDown, moveLeft, moveRight;

    void Awake()
    {
        Instance = this;
    }

    public void SetDirection(DPadButton.Direction dir, bool isPressed)
    {
        switch (dir)
        {
            case DPadButton.Direction.Up: moveUp = isPressed; break;
            case DPadButton.Direction.Down: moveDown = isPressed; break;
            case DPadButton.Direction.Left: moveLeft = isPressed; break;
            case DPadButton.Direction.Right: moveRight = isPressed; break;
        }
    }

    void Update()
    {
        Vector3 dir = Vector3.zero;

        if (moveUp) dir += Vector3.up;
        if (moveDown) dir += Vector3.down;
        if (moveLeft) dir += Vector3.left;
        if (moveRight) dir += Vector3.right;

        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}