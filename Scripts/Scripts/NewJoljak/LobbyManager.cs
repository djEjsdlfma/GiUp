//using Photon.Pun;
//using Photon.Pun.Demo.Asteroids;
//using Photon.Realtime;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using Hashtable = ExitGames.Client.Photon.Hashtable;

//public class LobbyManager : MonoBehaviourPunCallbacks
//{
//    // 닉네임 입력 UI 추가
//    [Header("--- 로비: 플레이어 이름 설정 ---")]
//    public TMP_InputField playerNameInput;

//    [Header("--- 패널 (Panel) ---")]
//    public GameObject lobbyPanel; // 로비(방 목록) 화면
//    public GameObject roomPanel;  // 대기방 화면

//    [Header("--- 로비: 방 목록 UI ---")]
//    public Transform roomListContent;       // 스크롤뷰의 Content
//    public GameObject roomEntryPrefab;      // RoomListEntryUI가 붙은 1줄짜리 프리팹

//    [Header("--- 로비: 방 생성/접속 UI ---")]
//    public TMP_InputField createRoomNameInput; // 방 생성 시 입력할 방 제목
//    public TMP_InputField createPasswordInput; // 방 생성 시 입력할 비밀번호 (비우면 공개방)
//    public TMP_InputField joinPasswordInput;   // 비번방 들어갈 때 입력할 비밀번호
//    public TextMeshProUGUI createRoomWarningText;
//    private RoomInfo _selectedRoom;            // 들어가려고 클릭한 방 기억용

//    [Header("--- 대기방: 방장 전용 UI ---")]
//    public GameObject hostGroup;
//    public TMP_InputField editRoomNameInput; // 방 제목 변경용
//    public TMP_InputField editPasswordInput; // 방 비밀번호 변경용
//    public Button btnStartGame;              // 방장용 게임 시작 버튼
//    public GameObject kickButtonObj;

//    [Header("--- 대기방: 방장 팝업 (Popup) ---")]
//    public GameObject editRoomNamePopup;
//    public GameObject editPasswordPopup;

//    [Header("--- 대기방: 참가자 전용 UI ---")]
//    public GameObject clientGroup;
//    public TextMeshProUGUI readyButtonText;  // 레디 상태 표시 텍스트

//    [Header("--- 대기방: 공통 UI ---")]
//    public TextMeshProUGUI roomTitleText;    // 현재 대기방 상단 제목 표시
//    public TextMeshProUGUI roomPasswordText;
//    public TextMeshProUGUI hostNameText;   // 방장 이름 표시용
//    public TextMeshProUGUI clientNameText; // 참가자 이름 및 레디 상태 표시용
//    public GameObject clientSlotObj;

//    [Header("--- 팝업 패널 (Popup) ---")]
//    public GameObject createRoomPopup; // 방 만들기 팝업창 (패널)
//    public GameObject passwordPopup;   // 비밀번호 입력 팝업창 (패널)

//    // 포톤에서 받아온 현재 살아있는 방 목록 데이터
//    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();
//    private bool _isReady = false;

//    // 닉네임 저장용 키값 상수
//    private const string PLAYER_NAME_PREF_KEY = "SavedPlayerName";

//    private void Start()
//    {
//        lobbyPanel.SetActive(true);
//        roomPanel.SetActive(false);

//        // 일반 입력창 글자 수 제한 설정
//        if (playerNameInput != null) playerNameInput.characterLimit = 8;
//        if (createRoomNameInput != null) createRoomNameInput.characterLimit = 15;
//        if (editRoomNameInput != null) editRoomNameInput.characterLimit = 15;

//        // 비밀번호 입력창 제한 설정 (15글자, 영어 및 숫자만 허용)
//        SetPasswordInputRestriction(createPasswordInput);
//        SetPasswordInputRestriction(joinPasswordInput);
//        SetPasswordInputRestriction(editPasswordInput);

//        LoadPlayerName();

//        PhotonNetwork.ConnectUsingSettings();

//        if (playerNameInput != null) playerNameInput.onSubmit.AddListener((s) => SavePlayerName());
//        if (createRoomNameInput != null) createRoomNameInput.onSubmit.AddListener((s) => CreateRoom());
//        if (createPasswordInput != null) createPasswordInput.onSubmit.AddListener((s) => CreateRoom());
//        if (joinPasswordInput != null) joinPasswordInput.onSubmit.AddListener((s) => ConfirmJoinPassword());
//        if (editRoomNameInput != null) editRoomNameInput.onSubmit.AddListener((s) => CmdChangeRoomName());
//        if (editPasswordInput != null) editPasswordInput.onSubmit.AddListener((s) => CmdChangePassword());
//    }

//    private void SetPasswordInputRestriction(TMP_InputField input)
//    {
//        if (input == null) return;

//        input.characterLimit = 15;
//        input.contentType = TMP_InputField.ContentType.Custom;
//        input.inputType = TMP_InputField.InputType.Password;
//        input.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
//    }

//    public void SavePlayerName()
//    {
//        if (playerNameInput == null || string.IsNullOrWhiteSpace(playerNameInput.text)) return;

//        string newName = playerNameInput.text;
//        PhotonNetwork.NickName = newName;

//        PlayerPrefs.SetString(PLAYER_NAME_PREF_KEY, newName);
//        PlayerPrefs.Save();

//        Debug.Log("[시스템] 닉네임이 기기에 저장되었습니다: " + newName);
//    }

//    private void LoadPlayerName()
//    {
//        string defaultName = "유저_" + Random.Range(1000, 9999);
//        string savedName = PlayerPrefs.GetString(PLAYER_NAME_PREF_KEY, defaultName);

//        if (savedName.Length > 8) savedName = savedName.Substring(0, 8);

//        PhotonNetwork.NickName = savedName;

//        if (playerNameInput != null)
//        {
//            playerNameInput.text = savedName;
//        }
//    }

//    public override void OnConnectedToMaster()
//    {
//        PhotonNetwork.JoinLobby();
//        PhotonNetwork.AutomaticallySyncScene = true;
//    }

//    #region 로비 (팝업 및 방 생성/입장)

//    public override void OnRoomListUpdate(List<RoomInfo> roomList)
//    {
//        foreach (RoomInfo info in roomList)
//        {
//            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
//            {
//                if (_cachedRoomList.ContainsKey(info.Name)) _cachedRoomList.Remove(info.Name);
//            }
//            else _cachedRoomList[info.Name] = info;
//        }
//        RefreshRoomListUI();
//    }

//    private void RefreshRoomListUI()
//    {
//        foreach (Transform child in roomListContent) Destroy(child.gameObject);
//        foreach (RoomInfo info in _cachedRoomList.Values)
//        {
//            GameObject entryGo = Instantiate(roomEntryPrefab, roomListContent);
//            RoomListUI entryUI = entryGo.GetComponent<RoomListUI>();
//            entryUI.Init(info, this);
//        }
//    }

//    public void OpenCreateRoomPopup()
//    {
//        createRoomNameInput.text = "";
//        if (createPasswordInput != null) createPasswordInput.text = "";
//        if (createRoomWarningText != null) createRoomWarningText.text = "";
//        createRoomPopup.SetActive(true);
//    }
//    public void CloseCreateRoomPopup() { createRoomPopup.SetActive(false); }

//    public void OpenPasswordPopup()
//    {
//        if (joinPasswordInput != null) joinPasswordInput.text = "";
//        passwordPopup.SetActive(true);
//        if (joinPasswordInput != null) joinPasswordInput.ActivateInputField();
//    }
//    public void ClosePasswordPopup() { passwordPopup.SetActive(false); }

//    public void TryJoinRoom(RoomInfo info)
//    {
//        _selectedRoom = info;
//        string pw = info.CustomProperties.ContainsKey("PW") ? info.CustomProperties["PW"].ToString() : "";

//        if (!string.IsNullOrEmpty(pw)) OpenPasswordPopup();
//        else PhotonNetwork.JoinRoom(info.Name);
//    }

//    private void CreateRoom()
//    {
//        if (string.IsNullOrWhiteSpace(createRoomNameInput.text))
//        {
//            if (createRoomWarningText != null) createRoomWarningText.text = "방 제목을 입력해 주세요!";
//            return;
//        }
//        if (createRoomWarningText != null) createRoomWarningText.text = "";

//        string hiddenRealName = System.Guid.NewGuid().ToString();
//        string displayName = createRoomNameInput.text;
//        string password = createPasswordInput != null ? createPasswordInput.text : "";

//        string myName = PhotonNetwork.NickName;

//        Hashtable props = new Hashtable
//            {
//                { "DN", displayName }, { "PW", password }, { "MD", (int)GameModeManager.CurrentMode }, { "HN", myName }
//            };

//        RoomOptions options = new RoomOptions
//        {
//            MaxPlayers = 2,
//            CustomRoomProperties = props,
//            CustomRoomPropertiesForLobby = new string[] { "DN", "PW", "MD", "HN" }
//        };

//        PhotonNetwork.CreateRoom(hiddenRealName, options);
//        CloseCreateRoomPopup();
//    }

//    private void ConfirmJoinPassword()
//    {
//        if (joinPasswordInput.text == _selectedRoom.CustomProperties["PW"].ToString())
//        {
//            PhotonNetwork.JoinRoom(_selectedRoom.Name);
//            ClosePasswordPopup();
//        }
//        else
//        {
//            Debug.Log("[시스템] 비밀번호가 틀렸습니다.");
//            joinPasswordInput.text = "";
//            joinPasswordInput.ActivateInputField();
//        }
//    }

//    #endregion

//    #region 대기방 (팝업 추가 및 로직)

//    public override void OnJoinedRoom()
//    {
//        lobbyPanel.SetActive(false);
//        roomPanel.SetActive(true);

//        _isReady = false;
//        if (readyButtonText != null) readyButtonText.text = "준비 (Ready)";

//        UpdateRoomUI();

//        if (!PhotonNetwork.IsMasterClient) SetMyReadyStatus(false);

//        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsKicked", false } });
//    }

//    private void UpdateRoomUI()
//    {
//        bool isHost = PhotonNetwork.IsMasterClient;
//        hostGroup.SetActive(isHost);
//        clientGroup.SetActive(!isHost);

//        if (PhotonNetwork.InRoom)
//        {
//            if (roomTitleText != null && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("DN"))
//            {
//                roomTitleText.text = $"방 제목\n{PhotonNetwork.CurrentRoom.CustomProperties["DN"]}";
//            }

//            if (roomPasswordText != null && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("PW"))
//            {
//                string currentPw = PhotonNetwork.CurrentRoom.CustomProperties["PW"].ToString();
//                if (string.IsNullOrEmpty(currentPw))
//                {
//                    roomPasswordText.text = "비밀번호\n(없음)";
//                }
//                else
//                {
//                    roomPasswordText.text = $"비밀번호\n({currentPw})";
//                }
//            }
//        }

//        if (isHost && btnStartGame != null) btnStartGame.interactable = CheckAllReady();

//        UpdatePlayerListUI();
//    }

//    private void UpdatePlayerListUI()
//    {
//        if (!PhotonNetwork.InRoom) return;

//        Photon.Realtime.Player hostPlayer = null;
//        Photon.Realtime.Player clientPlayer = null;

//        foreach (var p in PhotonNetwork.PlayerList)
//        {
//            if (p.IsMasterClient) hostPlayer = p;
//            else clientPlayer = p;
//        }

//        if (hostNameText != null && hostPlayer != null)
//        {
//            string hName = string.IsNullOrEmpty(hostPlayer.NickName) ? "익명" : hostPlayer.NickName;
//            hostNameText.text = $"방장: {hName}";
//        }

//        if (clientSlotObj != null)
//        {
//            if (clientPlayer != null)
//            {
//                clientSlotObj.SetActive(true);

//                if (clientNameText != null)
//                {
//                    string cName = string.IsNullOrEmpty(clientPlayer.NickName) ? "익명" : clientPlayer.NickName;
//                    bool isReady = false;

//                    if (clientPlayer.CustomProperties.TryGetValue("IsReady", out object readyObj))
//                    {
//                        isReady = (bool)readyObj;
//                    }

//                    string readyStatus = isReady ? "<color=#00FF00>[준비]</color>" : "<color=#FF0000>[대기]</color>";
//                    clientNameText.text = $"유저: {cName} {readyStatus}";
//                }

//                if (kickButtonObj != null) kickButtonObj.SetActive(PhotonNetwork.IsMasterClient);
//            }
//            else
//            {
//                clientSlotObj.SetActive(false);
//            }
//        }
//    }

//    public void OpenEditRoomNamePopup()
//    {
//        if (editRoomNameInput != null)
//        {
//            editRoomNameInput.text = "";
//            editRoomNameInput.ActivateInputField();
//        }
//        if (editRoomNamePopup != null) editRoomNamePopup.SetActive(true);
//    }
//    public void CloseEditRoomNamePopup() { if (editRoomNamePopup != null) editRoomNamePopup.SetActive(false); }

//    public void OpenEditPasswordPopup()
//    {
//        if (editPasswordInput != null)
//        {
//            editPasswordInput.text = "";
//            editPasswordInput.ActivateInputField();
//        }
//        if (editPasswordPopup != null) editPasswordPopup.SetActive(true);
//    }
//    public void CloseEditPasswordPopup() { if (editPasswordPopup != null) editPasswordPopup.SetActive(false); }

//    private void CmdChangeRoomName()
//    {
//        if (PhotonNetwork.IsMasterClient && !string.IsNullOrWhiteSpace(editRoomNameInput.text))
//        {
//            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "DN", editRoomNameInput.text } });
//            CloseEditRoomNamePopup();
//        }
//    }

//    private void CmdChangePassword()
//    {
//        if (PhotonNetwork.IsMasterClient)
//        {
//            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "PW", editPasswordInput.text } });
//            CloseEditPasswordPopup();
//        }
//    }

//    public void CmdChangeDifficulty(int modeIndex)
//    {
//        if (PhotonNetwork.IsMasterClient) PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "MD", modeIndex } });
//    }

//    public void CmdKickPlayer()
//    {
//        if (!PhotonNetwork.IsMasterClient) return;

//        foreach (Photon.Realtime.Player targetPlayer in PhotonNetwork.PlayerListOthers)
//        {
//            targetPlayer.SetCustomProperties(new Hashtable { { "IsKicked", true } });
//            break;
//        }
//    }

//    public void CmdStartGame()
//    {
//        // "GameDifficulty"를 "MD"로 변경
//        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("MD"))
//        {
//            Debug.LogWarning("방장이 난이도를 선택하지 않았습니다!");
//            return;
//        }

//        PhotonNetwork.AutomaticallySyncScene = true;
//        if (PhotonNetwork.IsMasterClient && CheckAllReady())
//        {
//            PhotonNetwork.LoadLevel("MMain");
//        }
//    }

//    public void ToggleReady()
//    {
//        _isReady = !_isReady;
//        SetMyReadyStatus(_isReady);
//        if (readyButtonText != null) readyButtonText.text = _isReady ? "준비 취소" : "준비 (Ready)";
//    }

//    private void SetMyReadyStatus(bool isReady)
//    {
//        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsReady", isReady } });
//    }

//    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
//    {
//        if (targetPlayer.IsLocal && changedProps.ContainsKey("IsKicked") && (bool)changedProps["IsKicked"])
//        {
//            Debug.Log("[시스템] 방장에 의해 추방되었습니다.");
//            PhotonNetwork.LeaveRoom();
//            return;
//        }

//        if (changedProps.ContainsKey("IsReady"))
//        {
//            if (PhotonNetwork.IsMasterClient && btnStartGame != null) btnStartGame.interactable = CheckAllReady();
//            UpdatePlayerListUI();
//        }
//    }

//    private bool CheckAllReady()
//    {
//        if (PhotonNetwork.CurrentRoom.PlayerCount < 2) return false;

//        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
//        {
//            if (p.CustomProperties.TryGetValue("IsReady", out object isReadyObj)) return (bool)isReadyObj;
//        }
//        return false;
//    }

//    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
//    {
//        UpdateRoomUI();
//        RefreshRoomListUI();
//    }

//    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
//    {
//        if (PhotonNetwork.IsMasterClient && btnStartGame != null) btnStartGame.interactable = false;
//        UpdatePlayerListUI();
//    }

//    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
//    {
//        if (PhotonNetwork.IsMasterClient && btnStartGame != null) btnStartGame.interactable = false;
//        UpdatePlayerListUI();
//    }

//    public void LeaveRoom() { PhotonNetwork.LeaveRoom(); }

//    public override void OnLeftRoom()
//    {
//        roomPanel.SetActive(false);
//        lobbyPanel.SetActive(true);
//        RefreshRoomListUI();
//    }

//    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
//    {
//        UpdateRoomUI();

//        if (PhotonNetwork.IsMasterClient)
//        {
//            string myName = string.IsNullOrEmpty(PhotonNetwork.NickName) ? "익명" : PhotonNetwork.NickName;
//            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "HN", myName } });
//        }
//    }
//    #endregion
//}