/*using LSW._02._Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class CastleUI : MonoBehaviour
{
    public Slider slider;      // Slider 컴포넌트 연결
    public Image fillImage;    // Slider의 Fill Image 연결

    public Color highColor = Color.green;
    public Color midColor = Color.yellow;
    public Color lowColor = Color.red;

    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private GameObject highScoreAlarm;

    private void Awake()
    {
        ReferenceManager.Instance.CastleUI = this;
    }

    // 현재값/최대값으로 바 갱신
    public void SetValue(float current, float max)
    {
        if (max <= 0f) return;

        float ratio = current / max;
        slider.maxValue = max;
        slider.value = current;

        // 체력 비율에 따라 초록 → 노랑 → 빨강 보간
        if (ratio > 0.65f)
        {
            // 100% ~ 50% : 초록 → 노랑
            float t = (ratio - 0.65f) / 0.65f;   // 0.65~1.0 을 0~1로 변환
            fillImage.color = Color.Lerp(midColor, highColor, t);
        }
        else
        {
            // 65% ~ 0% : 노랑 → 빨강
            float t = ratio / 0.3f;            // 0~0.3 를 0~1로 변환
            fillImage.color = Color.Lerp(lowColor, midColor, t);
        }
    }

    public void ShowFinalScore(int score, bool isNewRecord)
    {
        finalScore.SetText($"최종점수: {score}");

        // StatUI에서 신기록이라고 판별해줬다면 알람 켜기
        highScoreAlarm.SetActive(isNewRecord);
    }
}
*/