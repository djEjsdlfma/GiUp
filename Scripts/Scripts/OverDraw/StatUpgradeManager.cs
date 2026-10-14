//using UnityEngine;
//using System.Collections.Generic;
//using System.Numerics; // BigInteger 사용을 위한 네임스페이스

//public enum StatType
//{
//    MaxHealth,
//    BaseDamage,
//    Defense,
//    PerfectMultiplier
//}

//// 스탯별 강화 기획 데이터
//[System.Serializable]
//public class StatUpgradeData
//{
//    public StatType statType;

//    [Tooltip("BigInteger 변환을 위해 문자열로 입력 (예: 100, 100000)")]
//    public string baseCost = "100";

//    [Header("UI 표시 설정")]
//    public string displayName;

//    public float costMultiplier = 1.5f;
//    public int maxLevel = 1000;
//    public float baseValue = 0f;
//    public float valuePerLevel = 5f;
//}

//public class StatUpgradeManager : MonoBehaviour
//{
//    public static StatUpgradeManager Instance;

//    [Header("Upgrade Database")]
//    public List<StatUpgradeData> upgradeDatabase = new List<StatUpgradeData>();

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    // 현재 플레이어의 보유 포인트 확인 (string으로 불러와 BigInteger로 변환)
//    public BigInteger GetCurrentPoints()
//    {
//        string pointStr = PlayerPrefs.GetString("PlayerCoin", "0");
//        if (BigInteger.TryParse(pointStr, out BigInteger points))
//        {
//            return points;
//        }
//        return BigInteger.Zero;
//    }

//    public string GetStatDisplayName(StatType type)
//    {
//        StatUpgradeData data = upgradeDatabase.Find(x => x.statType == type);

//        // 데이터가 있고, displayName이 비어있지 않다면 해당 한글/지정 이름 반환
//        if (data != null && !string.IsNullOrEmpty(data.displayName))
//        {
//            return data.displayName;
//        }

//        // 입력된 값이 없다면 기존처럼 기본 Enum 이름 반환
//        return type.ToString();
//    }

//    // 특정 스탯의 현재 레벨 확인
//    public int GetStatLevel(StatType type)
//    {
//        return PlayerPrefs.GetInt($"StatLevel_{(int)type}", 0);
//    }

//    // 다음 레벨업 비용 계산 (BigInteger 연산)
//    public BigInteger GetNextUpgradeCost(StatType type)
//    {
//        StatUpgradeData data = upgradeDatabase.Find(x => x.statType == type);
//        if (data == null) return new BigInteger(999999);

//        int currentLevel = GetStatLevel(type);

//        // 1. 기본 비용 파싱
//        if (!BigInteger.TryParse(data.baseCost, out BigInteger cost))
//        {
//            cost = new BigInteger(100); // 파싱 실패 시 기본값
//        }

//        // 2. float 증가율을 BigInteger와 계산하기 위해 100을 곱해 정수로 만듦 (1.5f -> 150)
//        int multiplierPercent = Mathf.RoundToInt(data.costMultiplier * 100f);

//        // 3. 현재 레벨만큼 증가율 적용
//        for (int i = 0; i < currentLevel; i++)
//        {
//            cost = (cost * multiplierPercent) / 100;
//        }

//        return cost;
//    }

//    public bool IsMaxLevel(StatType type)
//    {
//        StatUpgradeData data = upgradeDatabase.Find(x => x.statType == type);
//        if (data == null) return true;

//        return GetStatLevel(type) >= data.maxLevel;
//    }

//    // 실제 구매(강화) 함수
//    public bool TryUpgradeStat(StatType type)
//    {
//        if (IsMaxLevel(type))
//        {
//            Debug.Log($"{type} 스탯은 이미 최대 레벨입니다.");
//            return false;
//        }

//        BigInteger cost = GetNextUpgradeCost(type);
//        BigInteger currentPoints = GetCurrentPoints();

//        if (currentPoints >= cost)
//        {
//            // 포인트 차감 후 string으로 저장
//            BigInteger remainPoints = currentPoints - cost;
//            PlayerPrefs.SetString("PlayerCoin", remainPoints.ToString());

//            // 레벨 1 증가
//            int currentLevel = GetStatLevel(type);
//            PlayerPrefs.SetInt($"StatLevel_{(int)type}", currentLevel + 1);

//            PlayerPrefs.Save();

//            Debug.Log($"{type} 스탯 강화 성공! (비용: {cost})");
//            return true;
//        }
//        else
//        {
//            Debug.Log("포인트가 부족합니다.");
//            return false;
//        }
//    }

//    public float GetTotalStatValue(StatType type)
//    {
//        StatUpgradeData data = upgradeDatabase.Find(x => x.statType == type);
//        if (data == null) return 0f;

//        int currentLevel = GetStatLevel(type);
//        return data.baseValue + (currentLevel * data.valuePerLevel);
//    }
//}