using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneChanger : MonoBehaviour, IPointerEnterHandler
{
    public string sceneName;
    public bool changeOnHover = false;

    public void ChangeScene()
    {
        Debug.Log("Changing scene to: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (changeOnHover)
        {
            Debug.Log("Mouse hovered over: " + gameObject.name);
            ChangeScene();
        }
    }
}