using UnityEngine;
using TMPro;

public class RadarUI : MonoBehaviour
{
    public static RadarUI Instance;

    public TMP_Text coordText;     // opsional, boleh dikosongkan

    public GameObject radarNormal; // GameObject "Radar" (hijau)
    public GameObject radarAlert;  // GameObject "Radar Alert" (merah)

    void Awake()
    {
        Instance = this;
    }

    public void UpdateRadar(Vector2Int gridPos, PlayerMover player)
    {
        if (coordText != null)
            coordText.text = $"X: {gridPos.x}, Y: {gridPos.y}";

        bool wallNearby =
            player.IsWallInDirection(DPadButton.Direction.Up) ||
            player.IsWallInDirection(DPadButton.Direction.Down) ||
            player.IsWallInDirection(DPadButton.Direction.Left) ||
            player.IsWallInDirection(DPadButton.Direction.Right);

        if (radarNormal != null) radarNormal.SetActive(!wallNearby);
        if (radarAlert != null) radarAlert.SetActive(wallNearby);
    }
}