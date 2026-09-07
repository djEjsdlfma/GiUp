//using Moon._01.Script.Chat;
//using Photon.Pun;
//using System;
//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using Hashtable = ExitGames.Client.Photon.Hashtable;

//public class MultiGameManager : MonoBehaviourPunCallbacks
//{
//    public static MultiGameManager Instance;

//    [Header("GameScene")]
//    public GameObject loadingPanel;
//    public GameObject countdownBgPanel;
//    public TextMeshProUGUI countdownText;

//    [Header("GameOverUI")]
//    public GameObject victoryPanel;
//    public GameObject defeatPanel;

//    [Header("GameStart")]
//    public UnityEngine.Events.UnityEvent onGameStart;
//    public static event System.Action OnGameStartEvent;

//    private bool _isCountdownStarted = false;
//    private bool _isGameStarted = false;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//    }

//    private void Start()
//    {
//        if (loadingPanel != null) loadingPanel.SetActive(true);
//        if (countdownBgPanel != null) countdownBgPanel.SetActive(true);
//        if (countdownText != null) countdownText.gameObject.SetActive(false);

//        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MD", out object diffObj))
//        {
//            int modeValue = (int)diffObj;

//            // 1이면 Easy, 2면 Normal을 실제 게임 모드에 세팅
//            if (modeValue == 1) GameModeManager.CurrentMode = GameMode.Easy;
//            else if (modeValue == 2) GameModeManager.CurrentMode = GameMode.Normal;

//            Debug.Log($"[시스템] 게임 난이도 세팅 완료: {GameModeManager.CurrentMode}");
//        }
//    }

//    public void CompleteLocalLoading()
//    {
//        Hashtable props = new Hashtable { { "IsLoaded", true } };
//        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
//        Debug.Log("[시스템] 내 로딩 완료 신호를 서버에 보냈습니다.");
//    }

//    private bool CheckAllPlayersLoaded()
//    {
//        if (PhotonNetwork.CurrentRoom.PlayerCount < 2) return false;

//        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
//        {
//            if (!p.CustomProperties.TryGetValue("IsLoaded", out object isLoaded) || !(bool)isLoaded)
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    private void StartCountdownSync()
//    {
//        _isCountdownStarted = true;
//        Debug.Log("[시스템] 모든 유저 로딩 완료! 카운트다운을 시작합니다.");

//        double startTime = PhotonNetwork.Time + 3.0;
//        int randomSeed = System.Environment.TickCount;

//        Hashtable props = new Hashtable
//            {
//                { "StartTime", startTime },
//                { "RandomSeed", randomSeed }
//            };
//        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
//    }

//    private void Update()
//    {
//        if (PhotonNetwork.CurrentRoom == null) return;

//        // [핵심] 스크립트를 끄는 대신, 게임이 시작되면 Update문의 실행만 차단합니다.
//        if (_isGameStarted) return;

//        if (PhotonNetwork.IsMasterClient && !_isCountdownStarted)
//        {
//            if (CheckAllPlayersLoaded())
//            {
//                StartCountdownSync();
//            }
//        }

//        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("StartTime", out object startTimeObj))
//        {
//            double startTime = (double)startTimeObj;
//            double timeRemaining = startTime - PhotonNetwork.Time;

//            if (timeRemaining > 0)
//            {
//                if (loadingPanel != null) loadingPanel.SetActive(false);

//                if (countdownText != null)
//                {
//                    countdownText.gameObject.SetActive(true);
//                    countdownText.text = Mathf.CeilToInt((float)timeRemaining).ToString();
//                }
//            }
//            else
//            {
//                if (countdownText != null) countdownText.text = "Start!";
//                StartCoroutine(FinishCountdownCoroutine());

//                // 원인 제거: this.enabled = false; 를 지우고 아래 코드로 교체합니다.
//                _isGameStarted = true;
//            }
//        }
//    }

//    private IEnumerator FinishCountdownCoroutine()
//    {
//        yield return new WaitForSecondsRealtime(1f);

//        if (countdownText != null) countdownText.gameObject.SetActive(false);
//        if (countdownBgPanel != null) countdownBgPanel.SetActive(false);

//        if (onGameStart != null)
//        {
//            onGameStart.Invoke();
//        }

//        OnGameStartEvent?.Invoke();
//    }


//    public void OnLocalPlayerDied()
//    {
//        // 서버 전체에 내가 졌다고 내 번호를 보냅니다.
//        photonView.RPC(nameof(RpcGameOver), RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
//    }

//    [PunRPC]
//    public void RpcGameOver(int loserActorNumber)
//    {
//        Debug.Log("[시스템] 게임 종료 신호 수신됨. 게임을 정지합니다.");

//        // 1. 게임 내 모든 시간 흐름 정지 (단어 떨어짐, 몬스터 이동 등)
//        Time.timeScale = 0f;

//        // 2. 플레이어가 더 이상 타자를 칠 수 없도록 채팅 매니저 비활성화
//        ChatManager chatManager = FindFirstObjectByType<ChatManager>();
//        if (chatManager != null)
//        {
//            chatManager.enabled = false;
//        }

//        // 3. 승패 UI 출력
//        if (PhotonNetwork.LocalPlayer.ActorNumber == loserActorNumber)
//        {
//            ShowDefeatUI();
//        }
//        else
//        {
//            string myUID = RankingManager.Instance.GetMyUID();
//            string myName = PhotonNetwork.LocalPlayer.NickName;
//            if (string.IsNullOrEmpty(myName)) myName = "Player_" + PhotonNetwork.LocalPlayer.ActorNumber;

//            RankingManager.Instance.AddMultiWin(myUID, myName);

//            ShowVictoryUI();
//        }
//    }

//    private void ShowVictoryUI()
//    {
//        if (victoryPanel != null) victoryPanel.SetActive(true);
//        Debug.Log("화면 출력: 승리!");
//    }

//    private void ShowDefeatUI()
//    {
//        if (defeatPanel != null) defeatPanel.SetActive(true);
//        Debug.Log("화면 출력: 패배...");
//    }

//    public void LeaveRoomAndReturnToLobby()
//    {
//        Time.timeScale = 1f;

//        PhotonNetwork.LeaveRoom();
        
//    }

//    public override void OnLeftRoom()
//    {
//        SceneManager.LoadScene("TitleScene");
//    }
//}
