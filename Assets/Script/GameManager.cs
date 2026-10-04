using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [System.Serializable]
    public class ObjectiveInfo
    {
        public string id;
        public Vector2Int gridPosition;
        public bool isClaimed;
    }

    public List<ObjectiveInfo> objectives = new List<ObjectiveInfo>();
    public Vector2Int lastPlayerGridPos;

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

    public void UpdatePlayerPosition(Vector2Int gridPos)
    {
        lastPlayerGridPos = gridPos;
    }

    public enum ClaimResult { Success, AlreadyClaimed, NoMatch }

    public ClaimResult TryClaimObjective(out ObjectiveInfo claimedObjective)
    {
        claimedObjective = null;
        foreach (var obj in objectives)
        {
            if (obj.gridPosition == lastPlayerGridPos)
            {
                if (obj.isClaimed)
                    return ClaimResult.AlreadyClaimed;

                obj.isClaimed = true;
                claimedObjective = obj;
                return ClaimResult.Success;
            }
        }
        return ClaimResult.NoMatch;
    }

    public bool IsClaimed(string id)
    {
        var found = objectives.Find(o => o.id == id);
        return found != null && found.isClaimed;
    }
}