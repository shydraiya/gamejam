using UnityEngine;

public class XPTest : MonoBehaviour
{
    public PlayerProgression progression;
    public float xpPerPress = 5f;
    public KeyCode testkey = KeyCode.L;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(testkey))
        {
            progression.AddXP(xpPerPress);
            Debug.Log($"xp 5");
        }
    }
}
