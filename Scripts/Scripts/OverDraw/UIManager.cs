//using DG.Tweening;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//// 1. 게임 내 모든 UI 패널의 종류를 정의
//public enum UIState
//{
//    Title,
//    Shop,
//    InGame,
//    GameOver,
//    // 나중에 Settings, Inventory 등이 필요하면 여기에 이름만 추가하면 됩니다.
//}

//// 2. 인스펙터에서 Enum과 CanvasGroup을 짝지어주기 위한 구조체
//[System.Serializable]
//public class UIPanelData
//{
//    public UIState state;
//    public CanvasGroup canvasGroup;
//}

//public class UIManager : MonoBehaviour
//{
//    public static UIManager Instance;

//    [Header("UI Panels Setup")]
//    public List<UIPanelData> panelDatabase; // 인스펙터에서 리스트로 관리

//    [Header("Transition Settings")]
//    public float fadeDuration = 0.4f;

//    // 빠른 검색을 위한 딕셔너리
//    private Dictionary<UIState, CanvasGroup> panels = new Dictionary<UIState, CanvasGroup>();

//    public TextMeshProUGUI stageText;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private void Start()
//    {
//        // 리스트의 데이터를 딕셔너리로 옮기고 모두 비활성화
//        foreach (var data in panelDatabase)
//        {
//            panels.Add(data.state, data.canvasGroup);
//            HideDirectly(data.canvasGroup);
//        }

//        // 시작 시 타이틀 화면만 켬
//        ChangeState(UIState.Title);
//    }

//    // 목표 패널만
//    public void ChangeState(UIState newState)
//    {
//        foreach (var kvp in panels)
//        {
//            if (kvp.Key == newState)
//            {
//                FadeIn(kvp.Value);
//            }
//            else
//            {
//                FadeOut(kvp.Value);
//            }
//        }
//    }

//    // (게임오버 창 용도)
//    public void ShowOverlay(UIState overlayState)
//    {
//        if (panels.TryGetValue(overlayState, out CanvasGroup cg))
//        {
//            FadeIn(cg);
//        }
//    }

//    // 덮어씌운 패널만
//    public void HideOverlay(UIState overlayState)
//    {
//        if (panels.TryGetValue(overlayState, out CanvasGroup cg))
//        {
//            FadeOut(cg);
//        }
//    }

//    public void ChangeState(string stateName)
//    {
//        if (System.Enum.TryParse(stateName, out UIState newState))
//        {
//            ChangeState(newState);
//        }
//        else
//        {
//            Debug.LogError($"잘못된 UI 상태 이름입니다: {stateName}");
//        }
//    }

//    // 유니티 버튼 인스펙터 전용 함수 (오버레이용)
//    public void ShowOverlay(string stateName)
//    {
//        if (System.Enum.TryParse(stateName, out UIState newState))
//        {
//            ShowOverlay(newState);
//        }
//    }

//    private void FadeIn(CanvasGroup cg)
//    {
//        if (cg.gameObject.activeSelf && cg.alpha == 1f) return;

//        cg.gameObject.SetActive(true);
//        cg.blocksRaycasts = true;
//        cg.DOFade(1f, fadeDuration);
//    }

//    private void FadeOut(CanvasGroup cg)
//    {
//        if (!cg.gameObject.activeSelf) return;

//        cg.blocksRaycasts = false;
//        cg.DOFade(0f, fadeDuration).OnComplete(() => cg.gameObject.SetActive(false));
//    }

//    private void HideDirectly(CanvasGroup cg)
//    {
//        cg.alpha = 0f;
//        cg.blocksRaycasts = false;
//        cg.gameObject.SetActive(false);
//    }

//    public void UpdateStageUI(int stageNumber)
//    {
//        if (stageText != null)
//        {
//            stageText.text = $"Stage {stageNumber}";
//        }
//    }
//}