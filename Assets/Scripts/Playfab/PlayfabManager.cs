using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.Advertisements;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;
    public string playerName;

    public List<int> scoreList;
    public List<string> nameList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        Login();
    }

    #region 登录信息
    private void Login()
    {
        //var request = new LoginWithCustomIDRequest
        //{
        //    CustomId = SystemInfo.deviceUniqueIdentifier,
        //    CreateAccount = true,
        //};

        var request = new LoginWithCustomIDRequest();
        request.CustomId = SystemInfo.deviceUniqueIdentifier;
        request.CreateAccount = true;
        request.InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
        {
            GetPlayerProfile = true
        };

        //调用API登录
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successful login / create account!");
        if(result.InfoResultPayload.PlayerProfile != null)
        {
            playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
            Debug.Log("Get DisplayName");
        }
    }
    #endregion

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error" + error.GenerateErrorReport());
    }

    #region 排行榜请求
    /// <summary>
    /// 发送数据请求
    /// </summary>
    /// <param name="score">得分</param>
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest();
        request.Statistics = new List<StatisticUpdate>
        {
            new StatisticUpdate
            {
                StatisticName = "HighScores",
                Value = score
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        //获得Leaderboard数据
        GetLeaderboardData();

        Debug.Log("Leaderboard Updated!");
    }

    /// <summary>
    /// 请求服务器数据
    /// </summary>
    public void GetLeaderboardData()
    {
        var request = new GetLeaderboardRequest();
        request.StatisticName = "HighScores";
        request.StartPosition = 0;
        request.MaxResultsCount = 7;

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboard, OnError);
    }

    private void OnGetLeaderboard(GetLeaderboardResult result)
    {
        scoreList = new List<int>();
        nameList = new List<string>();
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
            scoreList.Add(item.StatValue);
            nameList.Add(item.DisplayName);
        }
    }
    #endregion

    /// <summary>
    /// 更新服务器名字
    /// </summary>
    /// <param name="name">名字</param>
    public void SubmitName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        playerName = result.DisplayName;
        Debug.Log("DisplayName Updated");
    }
}
