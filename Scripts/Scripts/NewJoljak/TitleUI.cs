/*using DG.Tweening;
using Moon._01.Script.Chat;
using Moon._01.Script.Sounds;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

public class TitleUI : MonoBehaviour
{
    [Header("Transition Settings")]
    public Image transitionImage;
    public float transitionDuration = 1.0f;
    public string shaderPropertyName = "_Value";
    public Ease EaseGraph;

    [Header("Value Settings")]
    public float startValue = 2.5f;
    public float endValue = 0f;

    public GameObject SettingPanel;
    private InputSound _inputSound;
    private Dictionary<Type, IChatable> _components;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private TextMeshProUGUI text;
    private bool SelectGameMode;

    private void Awake()
    {
        Init();
        AddCompos();
    }

    private void Init()
    {

    }

    private void AddCompos()
    {
        _inputSound = GetComponentInChildren<InputSound>();
    }



    public void CheckText(string Text)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(Text))
        {
            inputField.text = "";
            _inputSound.ResetChat();
            inputField.ActivateInputField();
            return;
        }
        SoundPlayer.Instance.Play("Collect");

        if(SelectGameMode == false)
        {
            switch (Text)
            {
                case "게임시작":
                case "시작":
                case "게임 시작":
                    SelectGameMode = true;
                    text.SetText("튜토리얼 / 이지모드 / 일반모드");
                    break;
                case "설정":
                    inputField.text = "";
                    Setting();
                    return;
                case "나가기":
                case "끄기":
                case "종료":
                    GameEnd();
                    break;
            }
        }
        else
        {
            switch (Text)
            {
                case "튜토리얼":
                case "튜토":
                    GameModeManager.CurrentMode = GameMode.Tutorial;
                    ChangeScene("Main");
                    break;
                case "이지모드":
                case "이지 모드":
                case "이지":
                    GameModeManager.CurrentMode = GameMode.Easy;
                    ChangeScene("Main");
                    return;
                case "일반모드":
                case "일반 모드":
                case "일반":
                case "노멀모드":
                case "노멀 모드":
                case "노멀":
                    GameModeManager.CurrentMode = GameMode.Normal;
                    ChangeScene("Main");
                    break;
            }
        }

            inputField.text = "";
        _inputSound.ResetChat();
        inputField.ActivateInputField();
    }

    public void ChangeScene(string sceneName)
    {
        transitionImage.gameObject.SetActive(true);
        Material transitionMaterial = transitionImage.material;

        transitionMaterial.SetFloat(shaderPropertyName, startValue);

        transitionMaterial.DOFloat(endValue, shaderPropertyName, transitionDuration)
            .SetEase(EaseGraph)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
    }

    public void Setting()
    {
        SettingPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameEnd()
    {
        Application.Quit();
    }

    public void OffSetting()
    {
        SettingPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
*/