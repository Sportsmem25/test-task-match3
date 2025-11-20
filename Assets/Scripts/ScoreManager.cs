using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int Score 
    { 
        get; 
        private set; 
    }

    public TextMeshProUGUI scoreLabel;

    /// <summary>
    /// Points update
    /// </summary>
    /// <param name="amount"></param>
    public void Add(int amount)
    {
        Score += amount;
        scoreLabel.text = Score.ToString();
    }
}