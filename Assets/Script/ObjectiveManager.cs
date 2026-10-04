using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [System.Serializable]
    public class ObjectiveData
    {
        public string objectiveName;
        public Vector2Int gridPosition;
        public bool isCompleted;
        public GameObject marker; // object/sprite merah di scene (opsional, buat efek visual)
    }

    public List<ObjectiveData> objectives = new List<ObjectiveData>();
    public TMP_Text objectiveCounterText; // contoh tampilan: "Objectives: 1/4"

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateCounterUI();
    }

    public void CheckObjective(Vector2Int playerGridPos)
    {
        foreach (var obj in objectives)
        {
            if (!obj.isCompleted && obj.gridPosition == playerGridPos)
            {
                CompleteObjective(obj);
            }
        }
    }

    void CompleteObjective(ObjectiveData obj)
    {
        obj.isCompleted = true;
        Debug.Log($"Objective selesai: {obj.objectiveName}");

        if (obj.marker != null)
            obj.marker.SetActive(false); // marker merah hilang setelah tercapai

        UpdateCounterUI();
        CheckAllCompleted();
    }

    void UpdateCounterUI()
    {
        if (objectiveCounterText == null) return;
        int done = objectives.FindAll(o => o.isCompleted).Count;
        objectiveCounterText.text = $"Objectives: {done}/{objectives.Count}";
    }

    void CheckAllCompleted()
    {
        bool allDone = objectives.TrueForAll(o => o.isCompleted);
        if (allDone)
        {
            Debug.Log("Semua objective selesai!");
            // TODO: aksi kalau semua objective kelar, misal pindah scene / munculkan popup menang
        }
    }
}