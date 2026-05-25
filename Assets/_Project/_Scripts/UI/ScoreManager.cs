using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    void Start()
    {
        UpdateScore(0); 
    }

    void Update()
    {
        
    }
    public void UpdateScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }
    public int GetScore()
    {
        return score;
    }
}
