/*using csiimnida._01_Code.Player;
using csiimnida._01_Code.SaveSystem;
using csiimnida._01_Code.SaveSystem.Interface;
using DG.Tweening;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.CompilerServices.RuntimeHelpers;


public class SJ_UI : MonoBehaviour
{
    [SerializeField] private GameObject BackgroundPannel;
    [SerializeField] private GameObject SettingPannel;
    [SerializeField] private GameObject OffBtn;
    [SerializeField] private SettingManager SettingManager;

    [Header("슬라이더")]
    [SerializeField] private Slider BGM;
    [SerializeField] private Slider SFX;
    [SerializeField] private Slider Master;
    [SerializeField] private Slider SenceSlider;

    [SerializeField] private Image FadeImage;
    [SerializeField] private AudioMixer _audioMixer;

    [Header("슬라이더 이미지")]
    [SerializeField] private Image BgmImage;
    [SerializeField] private Image SfxImage;
    [SerializeField] private Image MasterImage;


    [SerializeField] private TextMeshProUGUI resetText;

    [SerializeField] private Sprite[] SoundImage;

    private readonly int _valueHash = Shader.PropertyToID("_Fill");

    public bool isInGame;
    public bool didClickBtn;

    private void Awake()
    {
        if (FadeImage != null)
            FadeImage.material = new Material(FadeImage.material);
        SenceSlider.minValue = 0f;
        SenceSlider.maxValue = 2f;

        BGM.value = -15f;
        SFX.value = -15f;
        Master.value = -20f;

        SenceSlider.value = 1f;
    }

    private void Start()
    {
        if (FadeImage != null)
            OffFade();
        BGM.onValueChanged.AddListener(SetBGMVolume);
        SFX.onValueChanged.AddListener(SetSFXVolume);
        Master.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SettingOn()
    {
        if (BackgroundPannel != null)
            BackgroundPannel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        SettingPannel.SetActive(true);

        SettingPannel.transform.DOScaleX(1f, 0.05f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (isInGame)
                Time.timeScale = 0f;
        });
    }

    public void SettingOff()
    {
        if (isInGame)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }


        SettingPannel.transform.DOScaleX(0f, 0.05f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                SettingPannel.SetActive(false);
                if (BackgroundPannel != null)
                    BackgroundPannel.SetActive(false);
            });
    }

    public void GameEnd()
    {
        Application.Quit();
    }

    private void Update()
    {
        //VFX SFX 나중에 넣기
        if (Input.GetKeyDown(KeyCode.Escape) && BackgroundPannel.activeSelf == false)
            SettingOn();


        ChangeSliderImage(BGM, BgmImage);
        ChangeSliderImage(SFX, SfxImage);
        ChangeSliderImage(Master, MasterImage);
    }

    public void OnFade(string SceneName)
    {
        Cursor.lockState = CursorLockMode.None;
        FadeImage.material.SetFloat(_valueHash, 1.2f);
        FadeImage.material.DOFloat(0f, _valueHash, 0.7f)
            .OnComplete(() => SceneManager.LoadScene(SceneName));
    }

    public void OffFade()
    {
        FadeImage.material.SetFloat(_valueHash, 0f);
        FadeImage.material.DOFloat(2.5f, _valueHash, 10f);
    }

    public void DoReset()
    {
        if (didClickBtn == false)
        {
            didClickBtn = true;
            resetText.text = "Really?";
        }
        else
        {
            SettingOff();
            Debug.Log("리셋");
            DataManager.Instance.ResetAllData();
            didClickBtn = false;
            resetText.text = "Stage Reset";
        }
    }

    private void ChangeSliderImage(Slider slider, Image img)
    {
        if (slider.value > -40 && slider.value < -20)
            img.sprite = SoundImage[1];
        else if (slider.value >= -20)
            img.sprite = SoundImage[2];
        else if (slider.value <= -40 && slider.value > -70)
            img.sprite = SoundImage[0];
        else if (slider.value == -70)
            img.sprite = SoundImage[3];

    }

    private void SetMasterVolume(float value)
    {
        _audioMixer.SetFloat("Master", value);
    }

    private void SetBGMVolume(float value)
    {
        _audioMixer.SetFloat("BGM", value);
    }

    private void SetSFXVolume(float value)
    {
        _audioMixer.SetFloat("SFX", value);
    }
}*/