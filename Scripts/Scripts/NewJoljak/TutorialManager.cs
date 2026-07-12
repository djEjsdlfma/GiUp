/*using System;
using System.Collections;
using System.Collections.Generic; // TextMeshPro
using System.ComponentModel;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 대화
[Serializable]
public struct TutorialSequence
{
    [TextArea]
    public string dialogText;
    public GameObject highlightMasks;
}

// 대화 분야
[Serializable]
public struct TutorialPhase
{
    [Header("이 분야의 대사 흐름")]
    public List<TutorialSequence> sequences;

    [Header("미션값 (있을때만)")]
    public int missionGoalValue; // 예: 5 (단어 5개) 또는 60 (60초 버티기)
}
public static class TextInput
{
    public static event Action onTyping;

    public static void InvokeEvent()
    {
        onTyping?.Invoke();
    }
}
public static class TextMake
{
    public static event Action onMaking;

    public static void InvokeEvent()
    {
        onMaking?.Invoke();
    }
}

public static class EnemySpawn
{
    public static event Action onSpawn;

    public static void InvokeEvent()
    {
        onSpawn?.Invoke();
    }
}

public static class ShopBuy
{
    public static event Action onbuying;

    public static void InvokeEvent()
    {
        onbuying?.Invoke();
    }
}


public class TutorialManager : MonoBehaviour
{
    public List<TutorialPhase> Tutorials;

    [SerializeField] private GameObject TutoCanvas;
    [SerializeField] private TextMeshProUGUI TutoText;

    private int TutorialIndex;
    private int TutorialPahseIndex;
    private GameObject nowPanel;
    private bool isPlaying;

    private int MissionValue = 1;

    private float timer;

    private void Start()
    {
        if(GameModeManager.CurrentMode == GameMode.Tutorial)
        {
            TutorialStart();
            SetText();

            AddEvent();
        }
        else
        {
            TutoCanvas.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void TutorialStart()
    {
        TutorialIndex = 0;
        TutorialPahseIndex = 0;
        Time.timeScale = 0f;
        isPlaying = false;
    }

    private void Update()
    {
        // 1. 대사를 진행하는 상태 (게임 멈춤)
        if (!isPlaying)
        {
            // 핵심 수정: 스페이스바, 클릭, 엔터 입력을 '가장 먼저' 확인합니다.
            // 이렇게 하면 유저가 버튼을 누르기 전까지는 절대 다음으로 넘어가지 않습니다.
            if (Keyboard.current.spaceKey.wasPressedThisFrame ||
                Mouse.current.leftButton.wasPressedThisFrame ||
                Keyboard.current.enterKey.wasPressedThisFrame)
            {
                // 입력이 들어왔을 때, 대사가 아직 남아있다면 다음 대사 출력
                if (TutorialIndex < Tutorials[TutorialPahseIndex].sequences.Count)
                {
                    SetText();
                }
                // 입력이 들어왔는데 대사를 다 본 상태라면 -> 미션(게임) 시작
                else
                {
                    if (TutorialPahseIndex < Tutorials.Count)
                    {
                        Time.timeScale = 1f;
                        isPlaying = true;
                        TutoCanvas.SetActive(false);

                        MissionValue = Tutorials[TutorialPahseIndex].missionGoalValue;
                    }
                }
            }
        }
        // 2. 미션을 진행하는 상태 (게임 진행 중)
        else
        {
            // 이벤트를 통해 MissionValue가 0 이하가 되면 다음 페이즈로 넘어감
            if (MissionValue <= 0)
            {
                isPlaying = false; // 다시 대사 상태로 전환 준비
                NextPhase();
            }
        }

        if(TutorialPahseIndex >= 3 && isPlaying)
        {
            timer += Time.deltaTime;

            if(timer > 1f)
            {
                MissionValue--;
                timer = 0f;
            }
        }
    }

    private void SetText()
    {
        // 1. 방어 코드 (Early Return): 인덱스가 범위를 벗어나면 아무것도 하지 않고 함수 종료
        if (TutorialPahseIndex >= Tutorials.Count ||
            TutorialIndex >= Tutorials[TutorialPahseIndex].sequences.Count)
        {
            return;
        }

        // 2. 캐싱 (Caching): 긴 배열 접근을 짧은 변수 하나로 빼서 가독성 확보
        TutorialSequence currentSequence = Tutorials[TutorialPahseIndex].sequences[TutorialIndex];

        // 3. UI 갱신
        TutoText.text = currentSequence.dialogText;
        SetHighlight(currentSequence.highlightMasks);

        // 4. 다음 대사를 위해 인덱스 증가
        TutorialIndex++;
    }

    private void NextPhase()
    {
        TutorialPahseIndex++;
        DeleteEvent(TutorialPahseIndex);
        // 튜토리얼을 모두 마쳤다면 여기서 튜토리얼 종료 처리
        if (TutorialPahseIndex >= Tutorials.Count)
        {
            Debug.Log("튜토리얼 올 클리어!");
            TutoCanvas.SetActive(false);
            SceneManager.LoadScene(0);
            return;
        }

        TutorialIndex = 0;
        Time.timeScale = 0f;
        TutoCanvas.SetActive(true);
        SetText();
    }

    private void SetHighlight(GameObject gameObj)
    {
        if (nowPanel != null)
            nowPanel.SetActive(false);

        gameObj.SetActive(true);
        nowPanel = gameObj;
    }

    private void AddEvent()
    {
        TextInput.onTyping += DiscountEventValue;
        TextMake.onMaking += DiscountEventValue;
        ShopBuy.onbuying += DiscountEventValue;
    }

    private void DeleteEvent(int num)
    {
        switch (num)
        {
            case 1:
                TextInput.onTyping -= DiscountEventValue;
                break;
            case 2:
                TextMake.onMaking -= DiscountEventValue;
                break;
            case 3:
                {
                    ShopBuy.onbuying -= DiscountEventValue;
                    EnemySpawn.InvokeEvent();
                }
                break;
        }
    }

    private void DiscountEventValue() => MissionValue--;
}*/