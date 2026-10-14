//using DG.Tweening;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;

//    [Header("Game Progression")]
//    public int currentStage = 1;

//    [Header("Score Tracking")]
//    public int totalPerfectCount = 0;
//    public int totalBustCount = 0;
//    public int totalExtraDrawCount = 0; // 추가 드로우(2장 이상) 횟수
//    public GameObject crownIcon; // 유니티 인스펙터에서 왕관 이미지(Image) 연결
//    public int plusDiaScore;

//    [Header("Manager References")]
//    public EnemyManager enemyManager;
//    public BattleManager battleManager;

//    [Header("UI - Game Over Panel")]
//    public TextMeshProUGUI resultStageText;
//    public TextMeshProUGUI bestStageText;     // 추가: 최고 도달 스테이지 표시용 텍스트
//    public TextMeshProUGUI resultRewardText;
//    public GameObject newRecordAlarmObject;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private void Start()
//    {
//        StartNewStage(false);
//    }

//    // 새 게임 시작 시 점수 초기화
//    public void ResetRunData()
//    {
//        currentStage = 1;
//        totalPerfectCount = 0;
//        totalBustCount = 0;
//        totalExtraDrawCount = 0;
//        plusDiaScore = 0;
//        UpdateStageRecord(currentStage);
//        UIManager.Instance.UpdateStageUI(currentStage);
//    }

//    public void StartNewStage(bool isTransition)
//    {
//        UIManager.Instance.UpdateStageUI(currentStage);
//        battleManager.ResetTurn();
//        enemyManager.GenerateEnemy(currentStage, isTransition);
//    }

//    public void OnEnemyDefeated()
//    {
//        currentStage++;
//        StartNewStage(true);
//        UpdateStageRecord(currentStage);
//    }

//    public void GameOver()
//    {
//        // 1. 점수 계산
//        int stageScore = currentStage * 100;
//        int perfectScore = totalPerfectCount * 50;
//        int bustPenalty = totalBustCount * (-20);
//        int extraDrawScore = totalExtraDrawCount * 10;
//        int diaScore = plusDiaScore;

//        int finalScore = stageScore + perfectScore + bustPenalty + extraDrawScore + diaScore;
//        if (finalScore < 0) finalScore = 0;

//        // 2. 최고 기록 확인
//        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
//        int bestStage = PlayerPrefs.GetInt("BestStage", 0);

//        bool isNewRecord = (finalScore > bestScore) && (finalScore > 0);

//        int earnedPoints = (finalScore - 100) / 10;
//        string recordText = "";

//        // 알림 오브젝트 초기화 (기본 비활성화)
//        if (newRecordAlarmObject != null) newRecordAlarmObject.SetActive(false);

//        // 신기록 달성 시 로직
//        if (isNewRecord)
//        {
//            PlayerPrefs.SetInt("BestScore", finalScore);
//            PlayerPrefs.SetInt("BestStage", currentStage);

//            bestStage = currentStage;

//            earnedPoints = Mathf.RoundToInt(earnedPoints * 1.5f);
//            recordText = "<color=red>NEW RECORD!\n(1.5x Point Bonus)</color>\n";

//            // 신기록 달성 알림 오브젝트 활성화
//            if (newRecordAlarmObject != null) newRecordAlarmObject.SetActive(true);
//        }

//        string coinStr = PlayerPrefs.GetString("PlayerCoin", "0");
//        System.Numerics.BigInteger currentCoin = System.Numerics.BigInteger.Zero;
//        System.Numerics.BigInteger.TryParse(coinStr, out currentCoin);

//        // 2. 획득한 포인트(earnedPoints)를 기존 재화에 더함
//        // (earnedPoints가 int 자료형이더라도 BigInteger와 더하기 연산이 가능합니다)
//        System.Numerics.BigInteger finalCoin = currentCoin + earnedPoints;

//        // 3. 다시 문자열로 변환하여 덮어쓰기
//        PlayerPrefs.SetString("PlayerCoin", finalCoin.ToString());
//        PlayerPrefs.Save();

//        UIManager.Instance.ShowOverlay("GameOver");

//        resultStageText.text = $"Stage: {currentStage}";

//        // 최고 도달 스테이지 출력
//        if (bestStageText != null) bestStageText.text = $"Stage: {bestStage}";

//        resultRewardText.text =
//            $"Stage Score : {stageScore}\n" +
//            $"Perfect: +{perfectScore}\n" +
//            $"Extra Draw: +{extraDrawScore}\n" +
//            $"Bust Penalty: {bustPenalty}\n" +
//            $"Dia Bouns: +{diaScore}\n" +
//            $"--------------------\n" +
//            $"Total Score : {finalScore}\n\n" +
//            recordText +
//            $"<color=yellow>Earned Points : {GameUtility.FormatNumber(earnedPoints)} P</color>";

//        CurrencyUIManager.Instance.UpdateCurrencyUI();

//        GPGSManager.Instance.ReportScore(finalScore);
//        GPGSManager.Instance.ReportStage(currentStage);
//    }

//    public void UpdateStageRecord(int currentStage)
//    {
//        // 1. 기존 최고 기록 불러오기 (기본값은 1)
//        int bestStage = PlayerPrefs.GetInt("BestStage", 1);

//        // 2. 최고 기록 갱신 확인 및 왕관 표시
//        if (currentStage > bestStage)
//        {
//            // 신기록 달성! 데이터 저장
//            PlayerPrefs.SetInt("BestStage", currentStage);
//            PlayerPrefs.Save();

//            // 왕관 UI 켜기 및 연출
//            ShowCrownEffect();
//        }
//        else if (currentStage == bestStage && currentStage > 1)
//        {
//            // 신기록을 달리고 있는 현재 상태일 때도 왕관을 유지하고 싶다면 켜줍니다.
//            // (1스테이지부터 켜지면 어색하므로 currentStage > 1 조건 추가)
//            if (crownIcon != null) crownIcon.SetActive(true);
//        }
//        else
//        {
//            // 최고 기록에 미치지 못하면 왕관 숨기기
//            if (crownIcon != null) crownIcon.SetActive(false);
//        }
//    }

//    private void ShowCrownEffect()
//    {
//        if (crownIcon == null) return;

//        // 왕관 활성화
//        crownIcon.SetActive(true);

//        // DOTween을 활용해 왕관이 뿅! 하고 튀어나오는 역동적인 연출
//        crownIcon.transform.localScale = Vector3.zero;
//        crownIcon.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
//    }

//    public void AddDiaScore(int val) => plusDiaScore += val;
//}