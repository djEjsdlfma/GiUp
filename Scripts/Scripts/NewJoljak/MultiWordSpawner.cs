//using LSW._02._Scripts.So;
//using Moon._01.Script.Chat;
//using Photon.Pun;
//using UnityEngine;

//public class MultiWordSpawner : MonoBehaviourPunCallbacks
//{
//    [SerializeField] private float spawnTime = 2f;
//    [SerializeField] private float moveSpeed = 1f;
//    [SerializeField] private float maxSpeed = 5f;
//    [SerializeField] private StageData stageData;
//    [SerializeField] private Transform spawnMinPoint;
//    [SerializeField] private Transform spawnMaxPoint;

//    private float _spawnTimer = 0f;
//    private ChatObjectPool _objectPool;
//    private WordManager _wordManager;
//    private ChatManager _owner;

//    private float _gameTime = 0f;
//    private int _currentProgress = -1;
//    private float _multiplier = 1f;

//    private bool _isMultiplayerWaiting = true;
//    private System.Random _syncedRandom;

//    public override void OnEnable()
//    {
//        base.OnEnable();
//        MultiGameManager.OnGameStartEvent += StartGameLogic;
//    }

//    public override void OnDisable()
//    {
//        base.OnDisable();
//        MultiGameManager.OnGameStartEvent -= StartGameLogic;
//    }

//    // 기존 방식대로 ChatManager가 호출해 줄 초기화 함수
//    public void Init(ChatManager owner)
//    {
//        _owner = owner;
//        _wordManager = _owner.GetCompo<WordManager>();
//        _objectPool = _owner.GetCompo<ChatObjectPool>();
//    }
//    private void Start()
//    {
//        if (_wordManager == null)
//        {
//            _wordManager = FindFirstObjectByType<WordManager>();
//        }
//        if (_objectPool == null)
//        {
//            _objectPool = FindFirstObjectByType<ChatObjectPool>();
//        }

//        if (_wordManager == null || _objectPool == null)
//        {
//            Debug.LogError("[오류] WordManager 또는 ChatObjectPool을 씬에서 찾을 수 없습니다!");
//        }
//    }

//    public void StartGameLogic()
//    {
//        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RandomSeed", out object seedObj))
//        {
//            // 적 스포너와 단어 스포너의 난수 패턴이 겹치지 않도록 시드값에 오프셋(+200)을 줍니다.
//            int seed = (int)seedObj + 200;
//            _syncedRandom = new System.Random(seed);
//        }

//        // 대기 상태를 해제하여 Update문을 돌리기 시작합니다.
//        _isMultiplayerWaiting = false;
//    }

//    private void Update()
//    {
//        // 카운트다운 중이면 타이머와 로직을 완전히 멈춥니다.
//        if (_isMultiplayerWaiting) return;

//        _spawnTimer += Time.deltaTime;
//        _gameTime += Time.deltaTime;

//        int calculatedProgress = Mathf.FloorToInt(_gameTime / 60f);

//        if (calculatedProgress != _currentProgress)
//        {
//            _currentProgress = calculatedProgress;
//            UpdateStageStats();
//        }

//        if (_spawnTimer >= spawnTime)
//        {
//            SpawnRandomChat();
//            _spawnTimer = 0f;
//        }
//    }

//    private void UpdateStageStats()
//    {
//        Stage currentStage = GetStageFromSO(_currentProgress);
//        _multiplier = currentStage.multiply;
//    }

//    private Stage GetStageFromSO(int progress)
//    {
//        if (stageData == null || stageData.stagesContainer.stages == null || stageData.stagesContainer.stages.Count == 0)
//            return default;

//        var stages = stageData.stagesContainer.stages;
//        Stage matched = stages[0];
//        foreach (var stage in stages)
//        {
//            if (progress >= stage.progress)
//            {
//                matched = stage;
//            }
//            else
//            {
//                break;
//            }
//        }
//        return matched;
//    }

//    private void SpawnRandomChat()
//    {
//        if (_syncedRandom == null || _wordManager == null || _objectPool == null) return;

//        // (주의) WordManager 쪽에 _syncedRandom을 매개변수로 받아 단어를 뽑아주는 
//        // GetSyncedRandomWord 함수가 구현되어 있어야 완벽하게 동기화됩니다.
//        string randomWord = _wordManager.GetSyncedRandomWord(_syncedRandom);

//        if (string.IsNullOrEmpty(randomWord))
//            return;

//        ChatObj chatObj = _objectPool.Spawn(randomWord);

//        // 1. 단어 떨어지는 속도 부여 (원인 해결)
//        float speed = Mathf.Min(moveSpeed * _multiplier, maxSpeed);
//        chatObj.SetSpeed(speed);

//        // 2. 동기화된 난수를 사용하여 X 좌표 계산 (양쪽 유저 동일한 위치)
//        float range = spawnMaxPoint.position.x - spawnMinPoint.position.x;
//        float randomX = spawnMinPoint.position.x + (float)(_syncedRandom.NextDouble() * range);

//        chatObj.transform.position = new Vector3(randomX, spawnMaxPoint.position.y, 0);
//    }
//}
