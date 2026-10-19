using UnityEngine;

public class DPadButton : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;

    void OnMouseDown()
    {
        Debug.Log("Tombol ditekan: " + direction);
        if (PlayerMover.Instance == null)
        {
            Debug.LogError("PlayerMover.Instance kosong! Ship nonaktif atau script PlayerMover belum terpasang.");
            return;
        }
        PlayerMover.Instance.StartHolding(direction);
    }

    void OnMouseUp()
    {
        if (PlayerMover.Instance != null)
            PlayerMover.Instance.StopHolding(direction);
    }
}