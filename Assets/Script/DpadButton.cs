using UnityEngine;
using UnityEngine.EventSystems;

public class DPadButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerMover.Instance.StartHolding(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerMover.Instance.StopHolding(direction);
    }
}