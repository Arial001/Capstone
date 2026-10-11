using UnityEngine;
using UnityEditor;

public class WallPlacerWindow : EditorWindow
{
    private bool placingActive = false;
    private float snapSize = 0.0935f; // samakan dengan Tile Size di PlayerMover
    private bool useSnap = true;

    private bool isDragging = false;
    private Vector3 dragStart;
    private Vector3 dragCurrent;

    private int wallCounter = 0;

    [MenuItem("Tools/Wall Placer")]
    public static void ShowWindow()
    {
        GetWindow<WallPlacerWindow>("Wall Placer");
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Wall Collider Placer", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        placingActive = EditorGUILayout.ToggleLeft("Mode Placement Aktif", placingActive);

        EditorGUILayout.Space();
        useSnap = EditorGUILayout.ToggleLeft("Snap ke Grid", useSnap);
        snapSize = EditorGUILayout.FloatField("Grid Size (Tile Size)", snapSize);

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "1. Centang 'Mode Placement Aktif'\n" +
            "2. Di Scene view, klik-tahan di pojok tembok, geser ke pojok seberang, lepas\n" +
            "3. Collider otomatis dibuat menutupi area itu, di-assign ke Layer 'Wall'",
            MessageType.Info);

        if (LayerMask.NameToLayer("Wall") == -1)
        {
            EditorGUILayout.HelpBox("Layer 'Wall' belum ada! Buat dulu lewat Edit > Project Settings > Tags and Layers", MessageType.Error);
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!placingActive) return;

        Event e = Event.current;
        Vector3 mouseWorldPos = GetMouseWorldPosition(e.mousePosition);

        if (useSnap)
            mouseWorldPos = SnapToGrid(mouseWorldPos);

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            isDragging = true;
            dragStart = mouseWorldPos;
            dragCurrent = mouseWorldPos;
            e.Use();
        }
        else if (e.type == EventType.MouseDrag && e.button == 0 && isDragging)
        {
            dragCurrent = mouseWorldPos;
            e.Use();
        }
        else if (e.type == EventType.MouseUp && e.button == 0 && isDragging)
        {
            isDragging = false;
            CreateWallCollider(dragStart, dragCurrent);
            e.Use();
        }

        if (isDragging)
        {
            Vector3 center = (dragStart + dragCurrent) / 2f;
            Vector3 size = new Vector3(Mathf.Abs(dragCurrent.x - dragStart.x), Mathf.Abs(dragCurrent.y - dragStart.y), 0f);

            Handles.DrawSolidRectangleWithOutline(
                new Vector3[]
                {
                    new Vector3(center.x - size.x / 2, center.y - size.y / 2, 0),
                    new Vector3(center.x + size.x / 2, center.y - size.y / 2, 0),
                    new Vector3(center.x + size.x / 2, center.y + size.y / 2, 0),
                    new Vector3(center.x - size.x / 2, center.y + size.y / 2, 0),
                },
                new Color(1f, 0f, 0f, 0.2f),
                Color.red
            );

            sceneView.Repaint();
        }
    }

    Vector3 GetMouseWorldPosition(Vector2 mousePos)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        if (snapSize <= 0) return pos;
        pos.x = Mathf.Round(pos.x / snapSize) * snapSize;
        pos.y = Mathf.Round(pos.y / snapSize) * snapSize;
        return pos;
    }

    void CreateWallCollider(Vector3 start, Vector3 end)
    {
        Vector3 center = (start + end) / 2f;
        Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y), 0f);

        if (size.x < 0.001f || size.y < 0.001f) return;

        wallCounter++;
        GameObject wall = new GameObject($"Wall_{wallCounter:00}");
        wall.transform.position = center;

        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
        col.size = size;

        int wallLayer = LayerMask.NameToLayer("Wall");
        if (wallLayer != -1)
            wall.layer = wallLayer;
        else
            Debug.LogWarning("Layer 'Wall' belum ada, GameObject dibuat dengan layer Default.");

        Undo.RegisterCreatedObjectUndo(wall, "Create Wall Collider");
        Selection.activeGameObject = wall;
    }
}