using UnityEngine;

public class ObjectiveMarker : MonoBehaviour
{
    public string objectiveId;
    public GameObject redMarker;
    public GameObject greenMarker;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        bool claimed = GameManager.Instance != null && GameManager.Instance.IsClaimed(objectiveId);
        if (redMarker != null) redMarker.SetActive(!claimed);
        if (greenMarker != null) greenMarker.SetActive(claimed);
    }
}