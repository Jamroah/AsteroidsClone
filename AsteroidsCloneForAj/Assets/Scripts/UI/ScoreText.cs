using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreText : MonoBehaviour
{
    public Text scoreText;

    void OnEnable()
    {
        Messenger<int>.AddListener("Update Score UI", UpdateScore);
    }

    void OnDisable()
    {
        Messenger<int>.RemoveListener("Update Score UI", UpdateScore);
    }

    void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
