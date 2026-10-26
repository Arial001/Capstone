using UnityEngine;
using System.Collections;
using TMPro;

public class RetrieverScan : MonoBehaviour
{
    public float scanDuration = 3f;
    public TMP_Text resultText;
    public string collectedMessage = "Objective collected";
    public string uncollectedMessage = "Objective uncollected";
    public bool hideSquare = false; // centang kalau kotak putih mau disembunyikan

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
        if (resultText != null) resultText.text = "";

        yield return new WaitForSeconds(scanDuration);

        bool collected = false;

        if (GameManager.Instance != null)
        {
            var result = GameManager.Instance.TryClaimObjective(out int index);
            Debug.Log($"Scan: {result}, objective index {index}");
            collected = (result != GameManager.ClaimResult.NoMatch);
        }
        else
        {
            Debug.LogWarning("GameManager tidak ada. Jalankan game dari scene MainControl, baru pindah scene.");
        }

        if (resultText != null)
            resultText.text = collected ? collectedMessage : uncollectedMessage;

        isScanning = false;
    }
}