using UnityEngine;
using System.Collections;
using TMPro;

public class RetrieverScan : MonoBehaviour
{
    [Header("Scan Settings")]
    public float scanDuration = 3f;
    public bool hideSquare = false; // centang kalau kotak putih mau disembunyikan

    [Header("UI Feedback")]
    public TMP_Text resultText;
    public string scanningMessage = "Scanning...";
    public string collectedMessage = "Objective collected";
    public string alreadyClaimedMessage = "Objective already collected";
    public string uncollectedMessage = "Objective uncollected";

    [Header("Optional References")]
    [Tooltip("Isi jika scene retriever juga memiliki AlertBackgroundController")]
    public AlertBackgroundController backgroundController;

    private bool isScanning = false;

    void Start()
    {
        if (resultText != null) resultText.text = "";

        if (hideSquare)
        {
            // Hanya sprite yang disembunyikan, klik tetap jalan lewat collider
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
        }
    }

    void OnMouseDown()
    {
        if (isScanning) return;
        StartCoroutine(ScanRoutine());
    }

    IEnumerator ScanRoutine()
    {
        isScanning = true;

        if (resultText != null)
            resultText.text = scanningMessage;

        yield return new WaitForSeconds(scanDuration);

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager tidak ada. Jalankan game dari scene MainControl, baru pindah scene.");
            if (resultText != null) resultText.text = uncollectedMessage;
            isScanning = false;
            yield break;
        }

        if (!GameManager.Instance.hasSavedPosition)
        {
            Debug.LogWarning("Koordinat kapal belum tersimpan. Pastikan kapal sudah dimuat di scene navigasi terlebih dahulu.");
        }

        var result = GameManager.Instance.TryClaimObjective(out int index);
        Debug.Log($"Scan: {result}, objective index {index} di koordinat {GameManager.Instance.lastPlayerWorldPos}");

        switch (result)
        {
            case GameManager.ClaimResult.Success:
                if (resultText != null) resultText.text = collectedMessage;
                if (backgroundController != null) backgroundController.RefreshBackground();
                break;

            case GameManager.ClaimResult.AlreadyClaimed:
                if (resultText != null)
                    resultText.text = string.IsNullOrEmpty(alreadyClaimedMessage) ? collectedMessage : alreadyClaimedMessage;
                break;

            case GameManager.ClaimResult.NoMatch:
                if (resultText != null) resultText.text = uncollectedMessage;
                break;
        }

        isScanning = false;
    }
}