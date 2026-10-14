//using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using UnityEngine.UI;

//public class GPGSManager : MonoBehaviour
//{
//    public static GPGSManager Instance;

//    [Header("UI 연결")]
//    public Button googleLoginButton;

//    private bool isAuthenticated = false;

//    private void Awake()
//    {
//        PlayGamesPlatform.DebugLogEnabled = true;

//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);

//            InitGPGS();
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void InitGPGS()
//    {
//        Login();
//    }

//    public void Login()
//    {
//        PlayGamesPlatform.Instance.ManuallyAuthenticate(status =>
//        {
//            if (status == SignInStatus.Success)
//            {
//                isAuthenticated = true;

//                if (googleLoginButton != null)
//                {
//                    googleLoginButton.interactable = false;
//                }
//            }
//            else
//            {
//                isAuthenticated = false;

//                if (googleLoginButton != null)
//                {
//                    googleLoginButton.interactable = true;
//                }
//            }
//        });
//    }

//    public void ShowLeaderboardUI()
//    {
//        if (isAuthenticated)
//        {
//            // GPGS 전용 API 사용
//            PlayGamesPlatform.Instance.ShowLeaderboardUI();
//        }
//        else
//        {
//            Debug.LogWarning("[GPGS] 로그인되어 있지 않아 랭킹을 불러올 수 없습니다.");
//        }
//    }

//    public void ReportStage(long score)
//    {
//        if (isAuthenticated)
//        {
//            string leaderboardID = "X";

//            // Social 클래스를 거치지 않고 GPGS API 직접 호출
//            PlayGamesPlatform.Instance.ReportScore(score, leaderboardID, (bool success) =>
//            {
//                if (success)
//                {
//                    Debug.Log($"[GPGS] 점수 {score} 등록 성공!");
//                }
//                else
//                {
//                    Debug.LogError("[GPGS] 점수 등록 실패");
//                }
//            });
//        }
//        else
//        {
//            Debug.LogWarning("[GPGS] 로그인되어 있지 않아 점수를 등록할 수 없습니다.");
//        }
//    }

//    public void ReportScore(long score)
//    {
//        if (isAuthenticated)
//        {
//            string leaderboardID = "X";

//            // Social 클래스를 거치지 않고 GPGS API 직접 호출
//            PlayGamesPlatform.Instance.ReportScore(score, leaderboardID, (bool success) =>
//            {
//                if (success)
//                {
//                    Debug.Log($"[GPGS] 점수 {score} 등록 성공!");
//                }
//                else
//                {
//                    Debug.LogError("[GPGS] 점수 등록 실패");
//                }
//            });
//        }
//        else
//        {
//            Debug.LogWarning("[GPGS] 로그인되어 있지 않아 점수를 등록할 수 없습니다.");
//        }
//    }
//}