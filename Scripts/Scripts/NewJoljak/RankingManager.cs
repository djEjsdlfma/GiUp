//using Firebase;
//using Firebase.Database;
//using Firebase.Extensions;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//[System.Serializable]
//public class RankEntry
//{
//    public string uid;         // 유저를 식별할 고유 번호 (추가됨)
//    public string playerName;  // 현재 닉네임
//    public int score;
//}

//public class RankingManager : MonoBehaviour
//{
//    public static RankingManager Instance;
//    private DatabaseReference dbReference;
//    private const string MY_UID_KEY = "MyUID";

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//            InitFirebase(); // 게임 시작 시 파이어베이스 초기화
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void InitFirebase()
//    {
//        // 파이어베이스가 정상적으로 연결될 수 있는지 확인
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
//            if (task.Result == DependencyStatus.Available)
//            {
//                string dbUrl = "https://wordwarranking-default-rtdb.asia-southeast1.firebasedatabase.app/";
//                dbReference = FirebaseDatabase.GetInstance(dbUrl).RootReference;
//                Debug.Log("Firebase 연동 완료 및 데이터베이스 준비됨");
//            }
//            else
//            {
//                Debug.LogError("Firebase 에러: " + task.Result);
//            }
//        });
//    }

//    // 내 기기의 고유 번호(UID) 가져오기
//    public string GetMyUID()
//    {
//        if (!PlayerPrefs.HasKey(MY_UID_KEY))
//        {
//            PlayerPrefs.SetString(MY_UID_KEY, System.Guid.NewGuid().ToString());
//            PlayerPrefs.Save();
//        }
//        return PlayerPrefs.GetString(MY_UID_KEY);
//    }


//    // 싱글플레이: 기존 최고 점수와 비교해서 더 높을 때만 갱신
//    public void AddSingleScore(string uid, string currentName, int score)
//    {
//        // 1. 함수가 정상적으로 호출되었는지 확인
//        Debug.Log($"AddSingleScore 호출됨 - uid: {uid}, name: {currentName}, score: {score}");

//        if (dbReference == null)
//        {
//            Debug.LogError("dbReference가 null입니다! Firebase 초기화가 안 되었거나 참조가 연결되지 않았습니다.");
//            return;
//        }

//        dbReference.Child("SingleRank").Child(uid).GetValueAsync().ContinueWithOnMainThread(task => {

//            // 1. 읽기 작업 자체가 실패했는지 확인
//            if (task.IsFaulted || task.IsCanceled)
//            {
//                Debug.LogError("DB 읽기 에러: " + task.Exception);
//                return;
//            }

//            int highestScore = score;

//            // 2. 데이터가 존재할 경우 안전하게 점수 비교
//            if (task.IsCompleted && task.Result.Exists)
//            {
//                object scoreObj = task.Result.Child("score").Value;
//                if (scoreObj != null)
//                {
//                    // int.Parse 대신 안전한 int.TryParse 사용
//                    if (int.TryParse(scoreObj.ToString(), out int existingScore))
//                    {
//                        if (existingScore > highestScore)
//                        {
//                            highestScore = existingScore;
//                        }
//                    }
//                    else
//                    {
//                        Debug.LogWarning("기존 점수를 변환할 수 없습니다: " + scoreObj.ToString());
//                    }
//                }
//            }

//            RankEntry entry = new RankEntry { uid = uid, playerName = currentName, score = highestScore };
//            string json = JsonUtility.ToJson(entry);
            
//            // 3. 데이터 저장 후 성공/실패 여부를 반드시 확인
//            dbReference.Child("SingleRank").Child(uid).SetRawJsonValueAsync(json).ContinueWithOnMainThread(writeTask => {
//                if (writeTask.IsFaulted)
//                {
//                    Debug.LogError("DB 점수 저장 실패 (권한 등 에러): " + writeTask.Exception);
//                }
//                else if (writeTask.IsCanceled)
//                {
//                    Debug.LogError("DB 점수 저장 취소됨");
//                }
//                else
//                {
//                    Debug.Log("DB 점수 저장 성공! 등록된 점수: " + highestScore);
//                }
//            });
//        });
//    }

//    // 멀티플레이: 기존 승수를 가져와서 1을 더한 후 갱신
//    public void AddMultiWin(string uid, string currentName)
//    {
//        if (dbReference == null) return;

//        dbReference.Child("MultiRank").Child(uid).GetValueAsync().ContinueWithOnMainThread(task => {
//            int newScore = 1;

//            if (task.IsCompleted && task.Result.Exists)
//            {
//                newScore = int.Parse(task.Result.Child("score").Value.ToString()) + 1;
//            }

//            RankEntry entry = new RankEntry { uid = uid, playerName = currentName, score = newScore };
//            string json = JsonUtility.ToJson(entry);
//            dbReference.Child("MultiRank").Child(uid).SetRawJsonValueAsync(json);
//        });
//    }

//    // 싱글플레이 랭킹 목록 가져오기
//    public void LoadSingleRanking(System.Action<List<RankEntry>> onLoaded)
//    {
//        if (dbReference == null) return;

//        dbReference.Child("SingleRank").GetValueAsync().ContinueWithOnMainThread(task => {
//            List<RankEntry> list = new List<RankEntry>();
//            if (task.IsCompleted && task.Result.Exists)
//            {
//                foreach (DataSnapshot child in task.Result.Children)
//                {
//                    string json = child.GetRawJsonValue();
//                    RankEntry entry = JsonUtility.FromJson<RankEntry>(json);
//                    list.Add(entry);
//                }
//            }
//            // 점수 내림차순(높은 순)으로 정렬하여 UI로 전달
//            onLoaded?.Invoke(list.OrderByDescending(x => x.score).ToList());
//        });
//    }

//    // 멀티플레이 랭킹 목록 가져오기
//    public void LoadMultiRanking(System.Action<List<RankEntry>> onLoaded)
//    {
//        if (dbReference == null) return;

//        dbReference.Child("MultiRank").GetValueAsync().ContinueWithOnMainThread(task => {
//            List<RankEntry> list = new List<RankEntry>();
//            if (task.IsCompleted && task.Result.Exists)
//            {
//                foreach (DataSnapshot child in task.Result.Children)
//                {
//                    string json = child.GetRawJsonValue();
//                    RankEntry entry = JsonUtility.FromJson<RankEntry>(json);
//                    list.Add(entry);
//                }
//            }
//            // 승수 내림차순(높은 순)으로 정렬하여 UI로 전달
//            onLoaded?.Invoke(list.OrderByDescending(x => x.score).ToList());
//        });
//    }

//    // 이름 중복 검사
//    public void CheckNameDuplicate(string desiredName, System.Action<bool> onResult)
//    {
//        if (dbReference == null)
//        {
//            onResult?.Invoke(false);
//            return;
//        }

//        string myUID = GetMyUID();

//        dbReference.GetValueAsync().ContinueWithOnMainThread(task => {
//            bool isDuplicated = false;

//            if (task.IsCompleted && task.Result.Exists)
//            {
//                DataSnapshot singleSnap = task.Result.Child("SingleRank");
//                DataSnapshot multiSnap = task.Result.Child("MultiRank");

//                // 싱글, 멀티 폴더를 모두 훑으며 내 UID가 아닌데 이름이 같은지 검사
//                if (singleSnap.Exists)
//                {
//                    foreach (DataSnapshot child in singleSnap.Children)
//                    {
//                        if (child.Child("playerName").Value.ToString() == desiredName &&
//                            child.Child("uid").Value.ToString() != myUID) isDuplicated = true;
//                    }
//                }

//                if (multiSnap.Exists)
//                {
//                    foreach (DataSnapshot child in multiSnap.Children)
//                    {
//                        if (child.Child("playerName").Value.ToString() == desiredName &&
//                            child.Child("uid").Value.ToString() != myUID) isDuplicated = true;
//                    }
//                }
//            }

//            onResult?.Invoke(isDuplicated);
//        });
//    }
//}