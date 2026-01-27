using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicController : MonoBehaviour
{
    public RectTransform textTransform;

    public float scrollSpeed = 300f;
    public float endY = 1800f;


    void Update()
    {
        textTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (textTransform.anchoredPosition.y >= endY)
        {
            GameFlowManager.Instance.GoToMainMenu();
        }
    }
}