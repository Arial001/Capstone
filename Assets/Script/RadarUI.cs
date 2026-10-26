using UnityEngine;
using TMPro;

public class RadarUI : MonoBehaviour
{
    public static RadarUI Instance;

    public TMP_Text coordText;      // opsional

    [Header("Radar")]
    public GameObject radarNormal;  // Radar_0 (hijau)
    public GameObject radarUp;      // Radar up (merah atas)
    public GameObject radarDown;    // Radar bottom (merah bawah)
    public GameObject radarLeft;    // Radar Left (merah kiri)
    public GameObject radarRight;   // Radar Right (merah kanan)

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateRadar(Vector2Int gridPos, PlayerMover player)
    {
    if (player == null) return;

    if (coordText != null)
        coordText.text = $"X: {gridPos.x}, Y: {gridPos.y}";

    bool up    = player.IsWallInDirection(DPadButton.Direction.Up);
    bool down  = player.IsWallInDirection(DPadButton.Direction.Down);
    bool left  = player.IsWallInDirection(DPadButton.Direction.Left);
    bool right = player.IsWallInDirection(DPadButton.Direction.Right);

    // radarNormal tidak disentuh, jadi selalu aktif
    if (radarUp != null)    radarUp.SetActive(up);
    if (radarDown != null)  radarDown.SetActive(down);
    if (radarLeft != null)  radarLeft.SetActive(left);
    if (radarRight != null) radarRight.SetActive(right);
    }
}    