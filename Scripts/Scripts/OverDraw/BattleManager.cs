//using csiimnida.CSILib.SoundManager.RunTime;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class BattleManager : MonoBehaviour
//{
//    [Header("Battle State")]
//    public int playerCurrentSum;
//    private CardSuit lastDrawnSuit;
//    private bool isAttacking = false;
//    private int diff;

//    [Header("UI - Battle Info")]
//    public TextMeshProUGUI currentSumText;
//    public Image currentSuitIcon;
//    public Sprite[] suitSprites;
//    public Button bustBtn;


//    private void Start()
//    {
//        currentSuitIcon.gameObject.SetActive(false);
//    }

//    public void OnClickDraw()
//    {
//        if (isAttacking) return;

//        Card drawnCard = CardUIManager.Instance.DrawCard();
//        SoundManager.Instance.PlaySound("Woosh");

//        playerCurrentSum += drawnCard.value;
//        lastDrawnSuit = drawnCard.suit;

//        UpdateBattleUI();
//        CheckBust();
//    }

//    private void UpdateBattleUI()
//    {
//        currentSumText.text = playerCurrentSum.ToString();
//        currentSuitIcon.sprite = suitSprites[(int)lastDrawnSuit];
//        currentSuitIcon.gameObject.SetActive(true);
//    }

//    private void CheckBust()
//    {
//        if (playerCurrentSum > GameManager.Instance.enemyManager.enemyTargetNumber)
//        {
//            isAttacking = true;
//            int bustDamage = 20 + (GameManager.Instance.currentStage * 3);

//            Transform playerTarget = PlayerManager.Instance.playerHealthSlider.transform;
//            GameManager.Instance.totalBustCount++;

//            CardUIManager.Instance.ShootCardsToPlayer(playerTarget, () =>
//            {
//                PlayerManager.Instance.TakeDamage(bustDamage, DamageType.Bust);
//                SoundManager.Instance.PlaySound("Wrong");
//                ResetTurn();
//            });
//        }
//    }

//    public void OnClickStand()
//    {
//        if (isAttacking) return;
//        isAttacking = true;

//        int target = GameManager.Instance.enemyManager.enemyTargetNumber;
//        diff = target - playerCurrentSum;

//        int basePower = CalculateDamageBySuit(playerCurrentSum, lastDrawnSuit) + PlayerManager.Instance.bonusDamage;
        
//        // 카드가 적에게 날아가는 연출이 끝난 후 데미지 계산 콜백
//        CardUIManager.Instance.ShootCardsToEnemy(GameManager.Instance.enemyManager.enemyImage.transform, () =>
//        {
//            if (diff == 0)
//            {
//                GameManager.Instance.totalPerfectCount++;
//                DealDamageToEnemy(Mathf.RoundToInt(basePower * PlayerManager.Instance.perfectMultiplier), true);
//            }
//            else if (diff > 0 && diff <= 2)
//            {
//                DealDamageToEnemy(basePower, false);
//            }
//            else
//            {
//                DealDamageToEnemy(basePower, false);
//                int counterDamage = Mathf.Abs(diff) + (GameManager.Instance.currentStage * 2);

//                // 수정됨: 반격(일반) 데미지 타입 명시
//                PlayerManager.Instance.TakeDamage(counterDamage, DamageType.Normal);
//            }

//            ResetTurn();
//        });
//    }

//    private int CalculateDamageBySuit(int baseDamage, CardSuit suit)
//    {
//        switch (suit)
//        {
//            case CardSuit.Spade:
//                return Mathf.RoundToInt(baseDamage * 1.5f);
//            case CardSuit.Heart:
//                PlayerManager.Instance.Heal(5);
//                return baseDamage;
//            case CardSuit.Club:
//                MinusDiff(baseDamage);
//                return baseDamage;
//            case CardSuit.Diamond:
//                GameManager.Instance.AddDiaScore(baseDamage);
//                return baseDamage;
//            default:
//                return baseDamage;
//        }
//    }

//    private void MinusDiff(int val)
//    {
//        diff = Mathf.Clamp(diff - (val / 10), 0, 999);
//    }

//    private void DealDamageToEnemy(int damageAmount, bool isPerfect)
//    {
//        GameManager.Instance.enemyManager.TakeDamage(damageAmount, isPerfect);

//        // 적 체력이 0 이하가 되면 사망 연출 후 다음 스테이지로 진행
//        if (GameManager.Instance.enemyManager.enemyCurrentHealth <= 0)
//        {
//            bustBtn.interactable = false;
//            isAttacking = true; // 다음 몬스터 나오기 전까지 조작 제한
//            GameManager.Instance.enemyManager.PlayDeathAnimation(() =>
//            {
//                GameManager.Instance.OnEnemyDefeated();
//                bustBtn.interactable = true;
//            });
//        }
//    }

//    public void ResetTurn()
//    {
//        playerCurrentSum = 0;
//        currentSumText.text = "0";
//        currentSuitIcon.gameObject.SetActive(false);
//        CardUIManager.Instance.ClearHandCards();
//        isAttacking = false;
//    }
//}