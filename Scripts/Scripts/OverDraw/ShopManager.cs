//using DG.Tweening;
//using System.Collections.Generic;
//using System.Numerics;
//using TMPro;
//using UnityEngine;

//public class ShopManager : MonoBehaviour
//{
//    public static ShopManager Instance;
//    public List<ShopItemData> allItems;
//    public AdMobManager admob;

//    [Header("UI - Toast Notification")]
//    public CanvasGroup toastCanvasGroup; // 배경+텍스트 통합 투명도 조절용
//    public RectTransform toastRectTransform; // 패널 전체 위치 조절용
//    public TextMeshProUGUI toastText;
//    private Sequence toastSequence;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    public bool IsUnlocked(string itemID)
//    {
//        // 아이템 ID에 "Default"라는 단어가 포함되어 있다면 무조건 해금된 것으로 판정
//        if (itemID.Contains("Default"))
//        {
//            return true;
//        }

//        // 그 외의 아이템들은 기존처럼 PlayerPrefs 저장소에서 구매 여부를 확인
//        return PlayerPrefs.GetInt("Unlocked_" + itemID, 0) == 1;
//    }

//    public bool TryPurchaseItem(ShopItemData item)
//    {
//        CurrencyUIManager.Instance.UpdateCurrencyUI();

//        string prefsKey = item.currencyType == CurrencyType.Point ? "PlayerCoin" : "PlayerGem";

//        // 1. 현재 보유 재화 불러오기 (BigInteger)
//        string currencyStr = PlayerPrefs.GetString(prefsKey, "0");
//        if (!BigInteger.TryParse(currencyStr, out BigInteger currentCurrency))
//        {
//            currentCurrency = BigInteger.Zero;
//        }

//        // 2. 아이템 가격 파싱 (BigInteger)
//        if (!BigInteger.TryParse(item.price, out BigInteger itemPrice))
//        {
//            itemPrice = BigInteger.Zero;
//            Debug.LogWarning($"{item.itemName}의 가격 파싱 실패. 0으로 처리됩니다.");
//        }

//        // 3. 재화 비교 및 차감
//        if (currentCurrency >= itemPrice)
//        {
//            // 차감 후 문자열로 저장
//            BigInteger remainCurrency = currentCurrency - itemPrice;
//            PlayerPrefs.SetString(prefsKey, remainCurrency.ToString());

//            PlayerPrefs.SetInt("Purchased_" + item.itemID, 1);

//            PlayerPrefs.Save();
//            ShowToast("Purchase Successful!", Color.green);
//            return true;
//        }
//        else
//        {
//            string currencyName = item.currencyType == CurrencyType.Point ? "Points" : "Gems";
//            ShowToast($"Not enough {currencyName}.", Color.red);
//            return false;
//        }
//    }

//    public void EquipItem(ShopItemData item)
//    {
//        if (!IsUnlocked(item.itemID)) return;

//        if (item.category == ItemCategory.Effect || item.category == ItemCategory.Sound ||
//            item.category == ItemCategory.Trail)
//        {
//            string prefsKey = "Equipped_" + item.category.ToString();
//            PlayerPrefs.SetString(prefsKey, item.itemID);
//            PlayerPrefs.Save();

//            ShowToast($"Equipped {item.itemName}.", Color.cyan);
//        }
//    }

//    public string GetEquippedItemID(ItemCategory type)
//    {
//        return PlayerPrefs.GetString("Equipped_" + type.ToString(), "");
//    }

//    public void ShowToast(string message, Color color)
//    {
//        // 텍스트뿐만 아니라 캔버스 그룹과 트랜스폼도 할당되어 있는지 확인
//        if (toastText == null || toastCanvasGroup == null || toastRectTransform == null) return;

//        if (toastSequence != null && toastSequence.IsActive()) toastSequence.Kill();

//        toastText.text = message;
//        toastText.color = color;

//        // 개별 텍스트가 아닌, 배경 이미지를 포함한 전체 패널 오브젝트를 활성화
//        toastCanvasGroup.gameObject.SetActive(true);

//        // CanvasGroup을 통해 배경과 텍스트의 투명도를 동시에 0으로 초기화
//        toastCanvasGroup.alpha = 0f;

//        // 패널 전체의 시작 위치를 아래로 50만큼 내림
//        toastRectTransform.anchoredPosition = new UnityEngine.Vector2(0, -50f);

//        toastSequence = DOTween.Sequence();

//        // 텍스트 대신 toastCanvasGroup과 toastRectTransform을 타겟으로 DOTween 실행
//        toastSequence.Append(toastCanvasGroup.DOFade(1f, 0.3f));
//        toastSequence.Join(toastRectTransform.DOAnchorPosY(0f, 0.3f).SetEase(Ease.OutBack));
//        toastSequence.AppendInterval(1f);
//        toastSequence.Append(toastCanvasGroup.DOFade(0f, 0.5f));
//        toastSequence.OnComplete(() => toastCanvasGroup.gameObject.SetActive(false));
//    }

//    public void BuyRemoveAdsComplete()
//    {
//        PlayerPrefs.SetInt("RemoveAdsPurchased", 1);
//        PlayerPrefs.Save();

//        if (admob != null)
//        {
//            admob.hasPurchasedRemoveAds = true;
//        }

//        ShowToast("AD Remove Purchase successful!", Color.green);
//    }

//    // 보석 구매 공통 함수 (매개변수 amount로 구매량 조절)
//    public void BuyGemsComplete(int amount)
//    {
//        string GemsStr = PlayerPrefs.GetString("PlayerGem", "0");
//        BigInteger curGem = BigInteger.Zero;
//        BigInteger.TryParse(GemsStr, out curGem);

//        PlayerPrefs.SetString("PlayerGem", (curGem + amount).ToString());
//        PlayerPrefs.Save();

//        ShowToast("Gem " + amount + " Purchase successful!", Color.green);
//        CurrencyUIManager.Instance.UpdateCurrencyUI();
//    }

//    public void FailBuyAnything()
//    {
//        ShowToast("Purchase failed", Color.red);
//    }
//}