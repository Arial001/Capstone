using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour
{
    public static PlayerMover Instance;

    public float tileSize = 0.0935f;
    public float initialDelay = 0.3f;
    public float repeatInterval = 0.15f;
    public LayerMask wallLayer;
    public bool hideSprite = true;

    public GameObject wallIndicatorUp;
    public GameObject wallIndicatorDown;
    public GameObject wallIndicatorLeft;
    public GameObject wallIndicatorRight;

    private Vector2Int gridPos;
    private DPadButton.Direction? heldDirection = null;
    private Coroutine moveRoutine;

    void Awake()
    {
        Instance = this;
        gridPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x / tileSize),
            Mathf.RoundToInt(transform.position.y / tileSize)
        );

        if (hideSprite)
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
            var img = GetComponent<UnityEngine.UI.Image>();
            if (img != null) img.enabled = false;
        }
    }

    void Start()
    {
        RefreshAll();
    }

    public void StartHolding(DPadButton.Direction dir)
    {
        heldDirection = dir;
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(HoldMoveLoop(dir));
    }

    public void StopHolding(DPadButton.Direction dir)
    {
        if (heldDirection == dir)
        {
            heldDirection = null;
            if (moveRoutine != null) StopCoroutine(moveRoutine);
        }
    }

    IEnumerator HoldMoveLoop(DPadButton.Direction dir)
    {
        TryMove(dir);
        yield return new WaitForSeconds(initialDelay);

        while (heldDirection == dir)
        {
            TryMove(dir);
            yield return new WaitForSeconds(repeatInterval);
        }
    }

    void TryMove(DPadButton.Direction dir)
    {
        Vector2Int target = gridPos;
        switch (dir)
        {
            case DPadButton.Direction.Up: target += Vector2Int.up; break;
            case DPadButton.Direction.Down: target += Vector2Int.down; break;
            case DPadButton.Direction.Left: target += Vector2Int.left; break;
            case DPadButton.Direction.Right: target += Vector2Int.right; break;
        }

        Vector3 targetWorldPos = GridToWorld(target);

        Collider2D blocker = Physics2D.OverlapBox(targetWorldPos, Vector2.one * (tileSize * 0.9f), 0f, wallLayer);
        if (blocker != null)
        {
            Debug.Log("Gerak " + dir + " DIBLOK oleh: " + blocker.name, blocker.gameObject);
            return;
        }

        gridPos = target;
        transform.position = targetWorldPos;

        RefreshAll();
    }

    void RefreshAll()
    {
        if (GameManager.Instance != null) GameManager.Instance.UpdatePlayerPosition(gridPos);
        if (RadarUI.Instance != null) RadarUI.Instance.UpdateRadar(gridPos, this);

        bool up = IsWallInDirection(DPadButton.Direction.Up);
        bool down = IsWallInDirection(DPadButton.Direction.Down);
        bool left = IsWallInDirection(DPadButton.Direction.Left);
        bool right = IsWallInDirection(DPadButton.Direction.Right);

        if (wallIndicatorUp != null) wallIndicatorUp.SetActive(up);
        if (wallIndicatorDown != null) wallIndicatorDown.SetActive(down);
        if (wallIndicatorLeft != null) wallIndicatorLeft.SetActive(left);
        if (wallIndicatorRight != null) wallIndicatorRight.SetActive(right);
    }

    Vector3 GridToWorld(Vector2Int g) => new Vector3(g.x * tileSize, g.y * tileSize, transform.position.z);

    public bool IsBlocked(Vector3 worldPos)
    {
        Collider2D hit = Physics2D.OverlapBox(worldPos, Vector2.one * (tileSize * 0.9f), 0f, wallLayer);
        return hit != null;
    }

    public bool IsWallInDirection(DPadButton.Direction dir)
    {
        Vector2Int check = gridPos;
        switch (dir)
        {
            case DPadButton.Direction.Up: check += Vector2Int.up; break;
            case DPadButton.Direction.Down: check += Vector2Int.down; break;
            case DPadButton.Direction.Left: check += Vector2Int.left; break;
            case DPadButton.Direction.Right: check += Vector2Int.right; break;
        }
        return IsBlocked(GridToWorld(check));
    }

    public Vector2Int GetGridPos() => gridPos;
}