//using System.Collections.Generic;
//using UnityEngine;

//public class RankingUI : MonoBehaviour
//{
//    [Header("--- UI 연결 ---")]
//    public GameObject rankSlotPrefab; // 랭킹 1줄짜리 프리팹
//    public GameObject Ranking;
//    public Transform contentParent;

//    // 멀티플레이
//    public void OpenMultiRanking()
//    {
//        ClearSlots(); // 열기 전에 이전 기록들 청소

//        // 서버에서 데이터를 다 받아올 때까지 기다렸다가, 도착하면 for문 실행
//        RankingManager.Instance.LoadMultiRanking((multiRank) => {
//            for (int i = 0; i < multiRank.Count; i++)
//            {
//                GameObject slotObj = Instantiate(rankSlotPrefab, contentParent);
//                RankingSlotUI slotUI = slotObj.GetComponent<RankingSlotUI>();

//                slotUI.SetSlotData(i + 1, multiRank[i].playerName, multiRank[i].score);
//            }
//        });

//        Ranking.SetActive(!Ranking.activeSelf);
//    }

//    // 싱글플레이
//    public void OpenSingleRanking()
//    {
//        ClearSlots();

//        // 서버에서 데이터를 다 받아올 때까지 기다렸다가, 도착하면 for문 실행
//        RankingManager.Instance.LoadSingleRanking((singleRank) => {
//            for (int i = 0; i < singleRank.Count; i++)
//            {
//                GameObject slotObj = Instantiate(rankSlotPrefab, contentParent);
//                RankingSlotUI slotUI = slotObj.GetComponent<RankingSlotUI>();

//                slotUI.SetSlotData(i + 1, singleRank[i].playerName, singleRank[i].score);
//            }
//        });
//        Ranking.SetActive(!Ranking.activeSelf);
//    }

//    // 기존에 만들어진 슬롯 청소
//    private void ClearSlots()
//    {
//        foreach (Transform child in contentParent)
//        {
//            Destroy(child.gameObject);
//        }
//    }
//}
