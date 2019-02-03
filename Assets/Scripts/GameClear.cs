using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour 
{
    public GameObject heroPrefab;
    public Transform heroInPos;
    public Transform heroStopPos;
    public float heroSpeed = 5.0f;
    public Text scoreBoard;
    public float scoreCountTime = 2.0f;
    public float typeWordSpeed = 0.2f;
    [TextArea]
    public string sentence;
    int countScore;

    void  Start()
    {
        countScore = 0;
        StartCoroutine(PushInHero());
        StartCoroutine(AddScore());
    }

    private IEnumerator PushInHero()
    {
        float timer = 0.0f;
        GameObject hero = Instantiate(heroPrefab, heroInPos.position, Quaternion.identity) as GameObject;

        do
        {
            timer += Time.deltaTime;
            hero.transform.position = Vector2.Lerp(hero.transform.position, heroStopPos.position, heroSpeed * Time.deltaTime);
            yield return null;
        } while (hero.transform.position.x >= heroStopPos.position.x);
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
        yield return new WaitForSeconds(2.0f);
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

        do
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(0);
            }
            yield return null;
        } while (true);
    }
}
