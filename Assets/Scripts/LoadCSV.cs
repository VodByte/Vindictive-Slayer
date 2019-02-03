using UnityEngine;
using System.Collections.Generic;

public struct TimeStamp
{
    public float time;
    public int enemyType;
    public int createDir;
}

public class LoadCSV : MonoBehaviour
{
    public static Queue<TimeStamp> timeStampQueue;

    private void Awake()
    {
        timeStampQueue = new Queue<TimeStamp>();

        TextAsset csvFile = Resources.Load<TextAsset>("TimeStamp");

        string[] data = csvFile.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length; i++)
        {
			string[] row = data[i].Split(new char[] { ',' });
            TimeStamp ts = new TimeStamp();
            float.TryParse(row[0], out ts.time);
            int.TryParse(row[1], out ts.enemyType);
            int.TryParse(row[2], out ts.createDir);
            timeStampQueue.Enqueue(ts);
        }
    }
}