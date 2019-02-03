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
    public float scoreCountGap = 2.0f;
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
            countScore += 20;
            scoreBoard.text = "         Score: " + countScore;
            yield return new WaitForSeconds(scoreCountGap);
        } while (countScore <= GameInfo_StaticClass.total_Score);
        GetComponent<AudioSource>().Stop();
        yield return new WaitForSeconds(2.0f);
        scoreBoard.text = "         Score: " + countScore + "\n" + "Stage Time: " 
            + (int)GameInfo_StaticClass.stageTime / 60 + "mins"
            + (int)GameInfo_StaticClass.stageTime % 60 + "seconds";

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
