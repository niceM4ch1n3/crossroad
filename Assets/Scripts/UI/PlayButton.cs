using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    private Button button;

    public GameObject nameInputPanel;
    public Button confirmButton;
    public InputField inputField;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(StartGame);
        confirmButton.onClick.AddListener(ConfirmName);
    }

    private void StartGame()
    {
        //启动游戏
        // SceneManager.LoadScene("GamePlay");
        if(PlayfabManager.instance.playerName == string.Empty)
        {
            nameInputPanel.SetActive(true);
        }
        else
        {
            nameInputPanel.SetActive(false);
            TransitionManager.instance.Transition("GamePlay");
        }
        
    }

    private void ConfirmName()
    {
        PlayfabManager.instance.SubmitName(inputField.text);
        nameInputPanel.SetActive(false);
        TransitionManager.instance.Transition("GamePlay");
    }
}
