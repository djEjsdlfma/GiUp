//using DG.Tweening;
//using TMPro;
//using UnityEngine;

//public class StageManager : MonoBehaviour
//{
//    public GameObject crownIcon; // 유니티 인스펙터에서 왕관 이미지(Image) 연결

//    // 스테이지가 넘어갈 때마다, 혹은 게임 오버 시점에 호출해주면 됩니다.
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
//}
