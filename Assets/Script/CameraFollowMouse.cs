using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    public float mouseInfluence = 0.3f;
    public float smoothSpeed = 5f;

    public float minX;
    public float maxX;

    private Camera cam;
    private float startingY;

    void Start()
    {
        cam = Camera.main;
        startingY = transform.position.y;
    }

    void Update()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        float targetX = transform.position.x +
                        (mousePosition.x - transform.position.x) * mouseInfluence;

        targetX = Mathf.Clamp(targetX, minX, maxX);

        Vector3 targetPosition = new Vector3(
            targetX,
            startingY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        Debug.Log("Camera X: " + transform.position.x);
    }
}