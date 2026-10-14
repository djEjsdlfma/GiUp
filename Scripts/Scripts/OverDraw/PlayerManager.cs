//using csiimnida.CSILib.SoundManager.RunTime;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class PlayerManager : MonoBehaviour
//{
//    public static PlayerManager Instance;

//    [Header("Player Base Stats")]
//    public int playerMaxHealth = 100;
//    public int playerCurrentHealth;
//    public int bonusDamage = 0;
//    public float perfectMultiplier = 3.0f;
//    public int playerDefense = 0;

//    [Header("UI References")]
//    public Slider playerHealthSlider;
//    public TextMeshProUGUI playerHealthText;
//    public DamageFeedbackManager feedbackManager;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    public void SetStartPlayer()
//    {
//        ApplyShopUpgrades();
//        InitPlayerHealth();
//    }

//    private void ApplyShopUpgrades()
//    {
//        if (StatUpgradeManager.Instance == null) return;

//        // 스탯 수치 계산은 StatManager에게 전담시킵니다.
//        playerMaxHealth = (int)StatUpgradeManager.Instance.GetTotalStatValue(StatType.MaxHealth);
//        bonusDamage = (int)StatUpgradeManager.Instance.GetTotalStatValue(StatType.BaseDamage);
//        playerDefense = (int)StatUpgradeManager.Instance.GetTotalStatValue(StatType.Defense);
//        perfectMultiplier += (StatUpgradeManager.Instance.GetTotalStatValue(StatType.PerfectMultiplier) / 10f);
//    }

//    private void InitPlayerHealth()
//    {
//        playerCurrentHealth = playerMaxHealth;
//        playerHealthSlider.maxValue = playerMaxHealth;
//        UpdatePlayerHealthUI();
//    }

//    public void UpdatePlayerHealthUI()
//    {
//        playerHealthSlider.value = playerCurrentHealth;
//        playerHealthText.text = $"{GameUtility.FormatNumber(playerCurrentHealth)} / {GameUtility.FormatNumber(playerMaxHealth)}";
//    }

//    public void TakeDamage(int rawDamage, DamageType damageType)
//    {
//        AndroidVibrator.Vibrate(100);

//        if(damageType == DamageType.Bust)
//        {
//            SoundManager.Instance.PlaySound("Normal");
//            feedbackManager.PlayDamageFeedback();
//        }

//        int finalDamage = Mathf.Max(0, rawDamage - playerDefense);
//        playerCurrentHealth -= finalDamage;

//        if (playerCurrentHealth < 0) playerCurrentHealth = 0;
//        UpdatePlayerHealthUI();

//        // 전달받은 damageType을 그대로 팝업 함수에 넘겨줍니다.
//        CardUIManager.Instance.SpawnDamagePopup(playerHealthSlider.transform.position, finalDamage, damageType);

//        if (playerCurrentHealth <= 0)
//        {
//            RevivePopupUI.Instance.ShowRevivePopup();
//        }
//    }

//    public void Heal(int amount)
//    {
//        playerCurrentHealth = Mathf.Min(playerMaxHealth, playerCurrentHealth + amount);
//        UpdatePlayerHealthUI();

//        if (playerCurrentHealth <= 0)
//        {
//            GameManager.Instance.GameOver();
//        }
//    }
//}