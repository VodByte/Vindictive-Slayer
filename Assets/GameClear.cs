using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour 
{
    public Text scoreBoard;
    public float scoreCountTime = 2.0f;
    public float typeWordSpeed = 0.2f;
    [TextArea]
    public string sentence;
    int countScore;

    void  Start()
    {
        countScore = 0;
        StartCoroutine(AddScore());
    }

    IEnumerator AddScore()
    {
        do 
        {
            countScore += 1;
            scoreBoard.text = "SCORE: " + countScore;
            yield return new WaitForSeconds(scoreCountTime);
        } while (countScore == GameInfo_StaticClass.total_Score);
        GetComponent<AudioSource>().Stop();
        scoreBoard.text = "SCORE: " + countScore + "\n" + "Game Time:" 
            + (int)GameInfo_StaticClass.stageTime / 60 + "Mins"
            + (int)GameInfo_StaticClass.stageTime % 60 + "Seconds";

        StartCoroutine(TypeWords());
    }

    IEnumerator TypeWords()
    {
        scoreBoard.text += "\n\n";
        for (int i = 0; i < sentence.ToCharArray().Length; i++)
        {
            scoreBoard.text = scoreBoard.text + sentence.ToCharArray()[i];
            yield return new WaitForSeconds(typeWordSpeed);
        }
    }
}
