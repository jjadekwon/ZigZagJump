using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : BasePopup
{
    public Text scoreText;
    public Text bestScoreText;

    public override void Init(int score = -1)
    {
        base.Init();

        scoreText.text = score.ToString();

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore) {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
        bestScoreText.text = bestScore.ToString();
    }

    public void OnRetryButtonClick ()
    {
        Debug.Log("Retry");
        GameManager.instance.Retry();
    }
}
