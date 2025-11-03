using TMPro; 
using UnityEngine;
public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    private float elapsedTime = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds / 10:00}";
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log($"Final time: {elapsedTime:F2} seconds");
    }

    public float GetFinalTime()
    {
        return elapsedTime;
    }
}
