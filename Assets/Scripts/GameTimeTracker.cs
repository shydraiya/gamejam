using UnityEngine;

public class GameTimeTracker : MonoBehaviour
{
    public static float FinalTime { get; private set; }

    private float playTime;

    private void Update()
    {
        playTime += Time.deltaTime;
    }

    public void SaveFinalTime()
    {
        FinalTime = playTime;
    }
}