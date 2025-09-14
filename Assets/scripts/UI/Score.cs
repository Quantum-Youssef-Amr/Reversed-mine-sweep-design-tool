using UnityEngine;
using TMPro;
using System;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    void Start()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
        EventBus.OnTotalScore += DisplayScore;
        EventBus.OnBoomsPlaced += DisplayError;
    }

    private void DisplayError(int Error, Ceil[,] _map_not_used_here)
    {
        _scoreText.text = $"error:{Error}";
    }

    private void DisplayScore(int totalScore)
    {
        totalScore = Mathf.Clamp(totalScore, 0, int.MaxValue);
        _scoreText.text = totalScore < 10 ? $"0{totalScore}" : $"{totalScore}";
    }
}
