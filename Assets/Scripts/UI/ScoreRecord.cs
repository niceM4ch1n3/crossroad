using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecord : MonoBehaviour
{
    public Text scoreText;
    public Text playerName;

    public void SetScoreText(int point)
    {
        scoreText.text = point.ToString();
    }

    public void SetName(string name)
    {
        playerName.text = name;
        if(name == PlayfabManager.instance.playerName)
        {
            playerName.color = Color.blue;
        }
        else
        {
            playerName.color = Color.black;
        }
    }
}
