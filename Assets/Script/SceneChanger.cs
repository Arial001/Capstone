using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToHall1()
    {
        SceneManager.LoadScene("Retriever");
    }

    public void GoToHall2()
    {
        SceneManager.LoadScene("MainControl");
    }
}