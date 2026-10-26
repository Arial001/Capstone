using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerPositionRestorer : MonoBehaviour
{
    void Awake()
    {
        var gm = GameManager.Instance;
        if (gm != null && gm.hasSavedPosition)
        {
            transform.position = new Vector3(
                gm.lastPlayerWorldPos.x,
                gm.lastPlayerWorldPos.y,
                transform.position.z
            );
        }
    }
}