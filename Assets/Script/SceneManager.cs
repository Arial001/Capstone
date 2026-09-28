using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            // Hitung index scene berikutnya, kembali ke 0 jika sudah di scene terakhir
            int nextIndex = (currentIndex + 1) % sceneCount;

            SceneManager.LoadScene(nextIndex);
        }
    }
}