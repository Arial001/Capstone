using UnityEngine;
using UnityEngine.EventSystems;

public class DPadButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerMover.Instance.SetDirection(direction, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerMover.Instance.SetDirection(direction, false);
    }
}