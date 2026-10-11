<<<<<<< HEAD
using UnityEngine;
using TMPro;

public class RadarUI : MonoBehaviour
{
    public static RadarUI Instance;

    public TMP_Text coordText;

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
=======
using UnityEngine;
using TMPro;

public class RadarUI : MonoBehaviour
{
    public static RadarUI Instance;

    public TMP_Text coordText; // koordinat ship HANYA tampil di sini
    public GameObject wallIndicatorUp;
    public GameObject wallIndicatorDown;
    public GameObject wallIndicatorLeft;
    public GameObject wallIndicatorRight;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateRadar(Vector2Int gridPos, PlayerMover player)
    {
        if (coordText != null)
            coordText.text = $"X: {gridPos.x}, Y: {gridPos.y}";

        SetIndicator(wallIndicatorUp, player.IsWallInDirection(DPadButton.Direction.Up));
        SetIndicator(wallIndicatorDown, player.IsWallInDirection(DPadButton.Direction.Down));
        SetIndicator(wallIndicatorLeft, player.IsWallInDirection(DPadButton.Direction.Left));
        SetIndicator(wallIndicatorRight, player.IsWallInDirection(DPadButton.Direction.Right));
    }

    void SetIndicator(GameObject indicator, bool wallDetected)
    {
        if (indicator == null) return;
        indicator.SetActive(wallDetected); // nyala/merah kalau ada tembok 1 kotak di arah itu
    }
>>>>>>> 943d557c649507990c639cf23da8aab4414eb84c
}