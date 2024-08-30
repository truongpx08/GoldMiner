using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    private static int _highScore;

    private void Start()
    {
        // Lấy điểm cao nhất từ PlayerPrefs  
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        //UpdateHighScoreText();
        highScoreText.text = _highScore.ToString();
    }

    public static void CheckNewHighScore(int currentScore)
    {
        if (currentScore > _highScore)
        {
            _highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", _highScore);
            PlayerPrefs.Save(); // Lưu thay đổi  
        }
    }
}