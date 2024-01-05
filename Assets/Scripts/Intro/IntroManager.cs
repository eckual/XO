using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [Serializable]
    public enum ButtonName
    {
        None = 0,
        VsAI = 1,
        TwoPlayers = 2,
        StartGame = 3
    }

    [Serializable]
    public class ButtonsInfo
    {
        public ButtonName buttonAction;
        public Button button;
    }

    [SerializeField] private List<ButtonsInfo> buttons;
    private ButtonsInfo _oldSelectedBtn;
    private ButtonsInfo _startGameBtn;

    private void OnDestroy()
    {
        buttons = null;
        _oldSelectedBtn = null;
        _startGameBtn = null;
    }

    private void Awake()
    {
        _startGameBtn = buttons.FirstOrDefault(btn => btn.buttonAction == ButtonName.StartGame);
        if (_startGameBtn != null) _startGameBtn.button.interactable = false;
        for (var i = 0; i < buttons.Count; i++)
        {
            var j = i;
            buttons[i].button.onClick.AddListener(() => ButtonClicked(buttons[j]));
        }
    }

    private void ButtonClicked(ButtonsInfo inButton)
    {
        if (inButton.buttonAction == ButtonName.StartGame)
        {
            SceneManager.LoadScene(GameConstants.InGameSceneName);
            return;
        }

        if (_oldSelectedBtn != null) _oldSelectedBtn.button.interactable = true;
        inButton.button.interactable = false;
        PlayerPrefs.SetString(GameConstants.PlayerPrefsModeKey, inButton.buttonAction.ToString());
        _oldSelectedBtn = inButton;
        _startGameBtn.button.interactable = true;
    }
    
}
