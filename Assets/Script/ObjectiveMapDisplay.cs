using UnityEngine;

public class ObjectiveMapDisplay : MonoBehaviour
{
    public GameObject[] uncollectedMarkers; // 7 kotak kuning
    public GameObject[] collectedMarkers;   // 7 kotak jingga, urutan sama dengan kuning

    public GameObject normalBackground;     // background biru
    public GameObject alertBackground;      // background merah
    public int collectedNeededForAlert = 4;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager tidak ditemukan di scene.");
            return;
        }

        var areas = new Rect[uncollectedMarkers.Length];
        for (int i = 0; i < uncollectedMarkers.Length; i++)
            areas[i] = GetWorldRect(uncollectedMarkers[i]);

        GameManager.Instance.RegisterObjectiveAreas(areas);
        Refresh();
    }

    void Refresh()
    {
        var gm = GameManager.Instance;

        for (int i = 0; i < uncollectedMarkers.Length; i++)
        {
            bool claimed = gm.IsClaimed(i);
            if (uncollectedMarkers[i] != null) uncollectedMarkers[i].SetActive(!claimed);
            if (i < collectedMarkers.Length && collectedMarkers[i] != null)
                collectedMarkers[i].SetActive(claimed);
        }

        bool alert = gm.ClaimedCount() >= collectedNeededForAlert;
        if (normalBackground != null) normalBackground.SetActive(!alert);
        if (alertBackground != null) alertBackground.SetActive(alert);
    }

    Rect GetWorldRect(GameObject go)
    {
        Bounds b;
        var r = go.GetComponent<Renderer>();
        var c = go.GetComponent<Collider2D>();

        if (r != null) b = r.bounds;
        else if (c != null) b = c.bounds;
        else b = new Bounds(go.transform.position, Vector3.one * 0.1f);

        return new Rect(b.min.x, b.min.y, b.size.x, b.size.y);
    }
}