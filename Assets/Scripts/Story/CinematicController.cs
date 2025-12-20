using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicController : MonoBehaviour
{
    public RectTransform textTransform;

    public float scrollSpeed = 300f;
    public float endY = 1800f;
    public string nextSceneName = "MainMenu";

    void Update()
    {
        textTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (textTransform.anchoredPosition.y >= endY)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}