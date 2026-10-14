//using UnityEngine;
//using UnityEngine.UI;
//using DG.Tweening;
//using TMPro;
//using System.Collections;

//public class EnemyManager : MonoBehaviour
//{
//    [Header("Enemy Status")]
//    public int enemyTargetNumber;
//    public int enemyMaxHealth;
//    public int enemyCurrentHealth;

//    [Header("UI References")]
//    public TextMeshProUGUI enemyNameText;
//    public TextMeshProUGUI enemyHealthText;
//    public TextMeshProUGUI enemyTargetText;
//    public Slider enemyHealthSlider;
//    public Image enemyImage;
//    public Sprite[] baseEnemySprites;

//    [Header("UI References")]
//    public RectTransform enemyUIPanel;
//    public Image whiteImage;

//    [Header("Flash Settings")]
//    public float flashInDuration = 0.1f;  // 불투명(255)해지는데 걸리는 시간
//    public float flashOutDuration = 0.4f; // 다시 투명(0)해지는데 걸리는 시간

//    private Color originalColor;
//    private Vector3 originalScale;

//    public void GenerateEnemy(int currentStage, bool isSlideIn)
//    {
//        EnemyTier tier = DetermineTier(currentStage);
//        Sprite selectedSprite = baseEnemySprites[Random.Range(0, baseEnemySprites.Length)];
//        enemyImage.sprite = selectedSprite;

//        string prefix = "";
//        originalColor = Color.white;
//        originalScale = Vector3.one;
//        int baseHealth = 0;

//        switch (tier)
//        {
//            case EnemyTier.Normal:
//                prefix = GetRandomPrefix(new string[] { "평범한", "느릿한", "약한" });
//                baseHealth = 50;
//                originalColor = Color.white;
//                originalScale = Vector3.one;
//                enemyTargetNumber = Random.Range(18, 22);
//                break;
//            case EnemyTier.Elite:
//                prefix = GetRandomPrefix(new string[] { "날렵한", "단단한", "정예" });
//                baseHealth = 120;
//                originalColor = new Color(1f, 0.5f, 0.5f);
//                originalScale = new Vector3(1.2f, 1.2f, 1.2f);
//                enemyTargetNumber = Random.Range(14, 18); // 엘리트는 버스트 유도
//                break;
//            case EnemyTier.Boss:
//                prefix = GetRandomPrefix(new string[] { "거대한", "파멸의", "타락한" });
//                baseHealth = 300;
//                originalColor = new Color(0.7f, 0.3f, 1f);
//                originalScale = new Vector3(1.5f, 1.5f, 1.5f);
//                enemyTargetNumber = Random.Range(24, 28); // 보스는 턴 연장 유도
//                break;
//        }

//        enemyMaxHealth = baseHealth + (currentStage * 15);
//        enemyCurrentHealth = enemyMaxHealth;

//        enemyHealthSlider.maxValue = enemyMaxHealth;
//        enemyHealthSlider.value = enemyCurrentHealth;

//        enemyNameText.text = $"{prefix} Monster";
//        enemyImage.color = originalColor;

//        enemyImage.transform.localScale = originalScale;


//        UpdateEnemyUI();

//        if (isSlideIn && enemyUIPanel != null)
//        {
//            // 원래 위치 저장 후 우측 밖으로 이동시켰다가 제자리로 애니메이션
//            Vector2 centerPos = enemyImage.rectTransform.anchoredPosition;
//            enemyImage.rectTransform.anchoredPosition = new Vector2(centerPos.x + 1000f, centerPos.y);
//            enemyImage.rectTransform.DOAnchorPos(centerPos, 0.5f).SetEase(Ease.OutQuad);
//        }
//    }

//    private EnemyTier DetermineTier(int stage)
//    {
//        if (stage > 0 && stage % 10 == 0) return EnemyTier.Boss;

//        int randomVal = Random.Range(0, 100);
//        if (stage <= 5)
//        {
//            return EnemyTier.Normal;
//        }
//        else if (stage <= 15)
//        {
//            if (randomVal < 30) return EnemyTier.Elite;
//            else return EnemyTier.Normal;
//        }
//        else
//        {
//            if (randomVal < 10) return EnemyTier.Boss;
//            else if (randomVal < 50) return EnemyTier.Elite;
//            else return EnemyTier.Normal;
//        }
//    }

//    private string GetRandomPrefix(string[] prefixes) { return prefixes[Random.Range(0, prefixes.Length)]; }

//    public void TakeDamage(int damageAmount, bool isPerfect)
//    {
//        AppearanceManager.Instance.PlayHitSound();
//        SpawnHitEffect();
//        enemyCurrentHealth -= damageAmount;
//        if (enemyCurrentHealth < 0) enemyCurrentHealth = 0;
//        enemyHealthSlider.value = enemyCurrentHealth;

//        if (isPerfect)
//        {
//            AndroidVibrator.Vibrate(150);
//            PerfectEffect();
//            StartCoroutine(HitStopRoutine(0.08f));
//        }

//        enemyImage.transform.DOComplete();
//        enemyImage.DOComplete();

//        enemyImage.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo).OnComplete(() => enemyImage.color = originalColor);

//        Vector3 squashScale = new Vector3(originalScale.x * 1.2f, originalScale.y * 0.8f, originalScale.z);

//        Sequence hitSeq = DOTween.Sequence();
//        hitSeq.Append(enemyImage.transform.DOScale(squashScale, 0.1f).SetEase(Ease.OutQuad));
//        hitSeq.Append(enemyImage.transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutElastic));

//        DamageType dmgType = isPerfect ? DamageType.Perfect : DamageType.Normal;
//        CardUIManager.Instance.SpawnDamagePopup(enemyImage.transform.position, damageAmount, dmgType);

//        UpdateEnemyUI();
//    }

//    private void PerfectEffect()
//    {
//        if (whiteImage == null) return;

//        // 연속으로 호출될 경우를 대비해 기존 트윈을 취소
//        DOTween.Kill(whiteImage);

//        Sequence flashSeq = DOTween.Sequence();
//        flashSeq.SetId(whiteImage);

//        // 1. 알파값을 1(투명도 255)로 서서히 올림
//        flashSeq.Append(whiteImage.DOFade(1f, flashInDuration).SetEase(Ease.OutQuad));

//        // 2. 알파값을 다시 0으로 서서히 내림
//        flashSeq.Append(whiteImage.DOFade(0f, flashOutDuration).SetEase(Ease.InQuad));
//    }

//    public void SpawnHitEffect()
//    {
//        if (AppearanceManager.Instance == null) return;

//        GameObject effectPrefab = AppearanceManager.Instance.GetEquippedEffectPrefab();
//        if (effectPrefab != null)
//        {
//            // 1. 랜덤 위치 계산 (적 위치를 기준으로 X, Y축으로 일정 범위 내에서 랜덤 값 추출)
//            float randomX = Random.Range(-0.5f, 0.6f);
//            float randomY = Random.Range(-0.5f, 0.6f);
//            Vector3 randomOffset = new Vector3(randomX, randomY, 0f);

//            // 2. 이펙트 생성 (적의 원래 위치 + 랜덤 오프셋 적용)
//            GameObject hitEffect = Instantiate(effectPrefab, enemyImage.transform.position + randomOffset, effectPrefab.transform.rotation);

//            // 3. 크기를 본래 크기의 0.1배로 축소
//            hitEffect.transform.localScale = effectPrefab.transform.localScale * 0.15f;

//            // 4. 이펙트 재생이 끝나면 파괴 예약
//            ParticleSystem ps = hitEffect.GetComponentInChildren<ParticleSystem>();
//            if (ps != null)
//            {
//                // 파티클의 실제 재생 시간만큼 기다렸다가 삭제
//                Destroy(hitEffect, ps.main.duration);
//            }
//            else
//            {
//                // 파티클 컴포넌트가 없는 경우를 대비한 안전 장치 (기본 1초 후 삭제)
//                Destroy(hitEffect, 1.0f);
//            }
//        }
//    }

//    private IEnumerator HitStopRoutine(float duration)
//    {
//        Time.timeScale = 0f;
//        yield return new WaitForSecondsRealtime(duration);
//        Time.timeScale = 1f;
//    }

//    public void PlayDeathAnimation(System.Action onComplete)
//    {
//        // 이펙트 길이를 담을 변수 (기본 0초)
//        float dynamicWaitTime = 0f;

//        if (AppearanceManager.Instance != null)
//        {
//            GameObject effectPrefab = AppearanceManager.Instance.GetEquippedEffectPrefab();
//            if (effectPrefab != null)
//            {
//                // Instantiate로 생성한 오브젝트를 변수에 담습니다
//                GameObject spawnedEffect = Instantiate(effectPrefab, (enemyUIPanel.transform.position + effectPrefab.transform.position),
//                    effectPrefab.transform.rotation);

//                // 생성된 이펙트에서 ParticleSystem 컴포넌트를 찾습니다 (자식 객체에 있을 수도 있으니 InChildren 사용)
//                ParticleSystem ps = spawnedEffect.GetComponentInChildren<ParticleSystem>();
//                if (ps != null)
//                {
//                    // 해당 이펙트의 고유 재생 시간(Duration)을 가져옵니다!
//                    dynamicWaitTime = ps.main.duration;
//                }
//                else
//                {
//                    // 파티클이 없는 특이한 이펙트일 경우를 대비한 기본값
//                    dynamicWaitTime = 1.0f;
//                }
//            }
//        }

//        // 적이 사라지는 연출 (0.3초 소요)
//        enemyImage.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
//        {
//            // 이펙트는 0초에 시작했고, 적이 사라지는 데 0.3초가 걸렸으므로
//            // 남은 시간 = (이펙트 전체 길이 - 0.3초) 
//            float remainingWait = Mathf.Max(0f, dynamicWaitTime - 0.3f);

//            // 남은 시간만큼만 딱 기다렸다가 다음 적을 소환!
//            DOVirtual.DelayedCall(remainingWait, () =>
//            {
//                onComplete?.Invoke();
//            });
//        });
//    }


//    private void UpdateEnemyUI()
//    {
//        enemyHealthText.text = $"{GameUtility.FormatNumber(enemyCurrentHealth)} / {GameUtility.FormatNumber(enemyMaxHealth)}";
//        enemyTargetText.text = $"Target Number : {enemyTargetNumber}";
//    }
//}