using UnityEngine;
using System.Collections;
using TMPro;

public class RetrieverScan : MonoBehaviour
{
    public float scanDuration = 1.5f;
    public TMP_Text resultText;
    private bool isScanning = false;

    void OnMouseDown()
    {
        if (isScanning) return;
        StartCoroutine(ScanRoutine());
    }

    IEnumerator ScanRoutine()
    {
        isScanning = true;
        if (resultText != null) resultText.text = "Scanning...";

        yield return new WaitForSeconds(scanDuration);

        var result = GameManager.Instance.TryClaimObjective(out var claimed);

        switch (result)
        {
            case GameManager.ClaimResult.Success:
                if (resultText != null) resultText.text = $"Objective '{claimed.id}' berhasil di-claim!";
                break;
            case GameManager.ClaimResult.AlreadyClaimed:
                if (resultText != null) resultText.text = "Objective ini sudah pernah di-claim.";
                break;
            case GameManager.ClaimResult.NoMatch:
                if (resultText != null) resultText.text = "Gagal — posisi tidak sesuai objective.";
                break;
        }

        isScanning = false;
    }
}