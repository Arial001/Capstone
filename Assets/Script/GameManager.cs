using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [System.Serializable]
    public class ObjectiveInfo
    {
        public Rect area;      // diisi otomatis dari kotak kuning di MainControl
        public bool isClaimed;
    }

    [System.Serializable]
    public struct SavedPlayerPosition
    {
        public Vector2Int gridPos;
        public Vector2 worldPos;
    }

    public List<ObjectiveInfo> objectives = new List<ObjectiveInfo>();

    public Vector2Int lastPlayerGridPos;
    public Vector2 lastPlayerWorldPos;
    public bool hasSavedPosition = false;

    // Menyimpan koordinat terakhir untuk masing-masing scene
    private readonly Dictionary<string, SavedPlayerPosition> _scenePositions = new Dictionary<string, SavedPlayerPosition>();

    public enum ClaimResult { Success, AlreadyClaimed, NoMatch }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Dipanggil PlayerMover versi kamu (1 argumen).
    // Posisi dunia diambil langsung dari PlayerMover.
    public void UpdatePlayerPosition(Vector2Int gridPos)
    {
        Vector2 worldPos = lastPlayerWorldPos;
        if (PlayerMover.Instance != null)
            worldPos = PlayerMover.Instance.transform.position;

        UpdatePlayerPosition(gridPos, worldPos);
    }

    public void UpdatePlayerPosition(Vector2Int gridPos, Vector2 worldPos)
    {
        lastPlayerGridPos = gridPos;
        lastPlayerWorldPos = worldPos;
        hasSavedPosition = true;

        string activeScene = SceneManager.GetActiveScene().name;
        _scenePositions[activeScene] = new SavedPlayerPosition
        {
            gridPos = gridPos,
            worldPos = worldPos
        };
    }

    // Dipanggil saat pindah scene agar posisi terakhir selalu tersimpan
    public void SaveCurrentPlayerPosition()
    {
        if (PlayerMover.Instance == null) return;

        Vector2Int currentGrid = PlayerMover.Instance.GetGridPos();
        Vector2 currentWorld = PlayerMover.Instance.transform.position;
        UpdatePlayerPosition(currentGrid, currentWorld);
    }

    // Dipanggil PlayerMover saat scene baru dimuat untuk memulihkan posisi
    public bool TryGetSavedPositionForCurrentScene(out SavedPlayerPosition savedPos)
    {
        string activeScene = SceneManager.GetActiveScene().name;
        if (_scenePositions.TryGetValue(activeScene, out savedPos))
        {
            lastPlayerGridPos = savedPos.gridPos;
            lastPlayerWorldPos = savedPos.worldPos;
            hasSavedPosition = true;
            return true;
        }

        hasSavedPosition = false;
        return false;
    }

    // Dipanggil MainControl tiap scene dimuat. Status claimed dipertahankan
    // selama jumlah objective sama.
    public void RegisterObjectiveAreas(Rect[] areas)
    {
        if (objectives.Count != areas.Length)
        {
            objectives = new List<ObjectiveInfo>();
            for (int i = 0; i < areas.Length; i++)
                objectives.Add(new ObjectiveInfo());
        }

        for (int i = 0; i < areas.Length; i++)
            objectives[i].area = areas[i];
    }

    public ClaimResult TryClaimObjective(out int index)
    {
        index = -1;
        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].area.Contains(lastPlayerWorldPos))
            {
                index = i;
                if (objectives[i].isClaimed) return ClaimResult.AlreadyClaimed;

                objectives[i].isClaimed = true;
                return ClaimResult.Success;
            }
        }
        return ClaimResult.NoMatch;
    }

    public bool IsClaimed(int index)
    {
        return index >= 0 && index < objectives.Count && objectives[index].isClaimed;
    }

    public int ClaimedCount()
    {
        int n = 0;
        foreach (var o in objectives)
            if (o.isClaimed) n++;
        return n;
    }
}