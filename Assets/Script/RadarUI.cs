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
}