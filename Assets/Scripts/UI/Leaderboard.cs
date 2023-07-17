using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public List<ScoreRecord> scoreRecords;
    private List<int> scoreList;
    private List<string> nameList;

    [Header("广告按钮")]
    public Button adsButton;

    private void OnEnable()
    {
        scoreList = GameManager.instance.GetScoreListData();
        nameList = PlayfabManager.instance.nameList;
        adsButton.onClick.AddListener(AdsManager.instance.ShowRewardAds);

        PlayfabManager.instance.GetLeaderboardData();
        SetLeaderboardData();
    }

    //private void Start()
    //{
    //    SetLeaderboardData();
    //}
    
    public void SetLeaderboardData()
    {
        for (int i = 0; i < scoreRecords.Count; i++)
        {
            if (i < scoreList.Count)
            {
                //设置分数
                scoreRecords[i].SetScoreText(scoreList[i]);
                //设置名字
                scoreRecords[i].SetName(nameList[i]);
                scoreRecords[i].gameObject.SetActive(true);
            }
            else
            {
                scoreRecords[i].gameObject.SetActive(false);
            }
        }
    }
}
