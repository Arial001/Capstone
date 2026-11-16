using UnityEngine;

public class AlertBackgroundController : MonoBehaviour
{
    public GameObject normalBackground; // background biru
    public GameObject alertBackground;  // background merah
    public int collectedNeededForAlert = 4;

    void Start()
    {
        RefreshBackground();
    }

    public void RefreshBackground()
    {
        if (GameManager.Instance == null) return;

        bool alert = GameManager.Instance.ClaimedCount() >= collectedNeededForAlert;
        if (normalBackground != null) normalBackground.SetActive(!alert);
        if (alertBackground != null) alertBackground.SetActive(alert);
    }
}