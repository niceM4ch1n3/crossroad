using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<int> scoreList;

    public bool isGlobal;

    private int score;
    private string dataPath;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        dataPath = Application.persistentDataPath + "/leaderboard.json";
        //scoreList = GetScoreListData();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOverEvent;
        EventHandler.GetPointEvent += OnGetPointEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
        EventHandler.GetPointEvent -= OnGetPointEvent;
    }

    private void Start()
    {
        scoreList = GetScoreListData();
    }

    private void OnGetPointEvent(int point)
    {
        score = point;
    }

    private void OnGameOverEvent()
    {
        if (!isGlobal)
        {
            //在List里添加新的分数，并进行排序
            if (!scoreList.Contains(score))
            {
                scoreList.Add(score);
            }

            scoreList.Sort();
            scoreList.Reverse();

            File.WriteAllText(dataPath, JsonConvert.SerializeObject(scoreList));
        }
        else
        {
            //发送分数到Playfab
            //Debug.Log("score:" + score);
            PlayfabManager.instance.SendLeaderboard(score);
        }
    }

    /// <summary>
    /// 读取保存数据的记录
    /// </summary>
    /// <returns></returns>
    public List<int> GetScoreListData()
    {
        if (!isGlobal)
        {
            if (File.Exists(dataPath))
            {
                string jsonData = File.ReadAllText(dataPath);
                return JsonConvert.DeserializeObject<List<int>>(jsonData);
            }
            return new List<int>();
        }
        else
        {
            return PlayfabManager.instance.scoreList;
        }
    }
}
