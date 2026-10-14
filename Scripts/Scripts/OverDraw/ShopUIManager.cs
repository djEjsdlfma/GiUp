//using csiimnida.CSILib.SoundManager.RunTime;
//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using Vector2 = UnityEngine.Vector2;
//using Vector3 = UnityEngine.Vector3;

//public class ShopUIManager : MonoBehaviour
//{
//    public static ShopUIManager Instance;

//    [Header("Shop Layout (Drag CanvasGroups or Transforms here)")]
//    public GameObject shopSlotPrefab;
//    public Transform trailContent;
//    public Transform effectContent;
//    public Transform soundContent;
//    public ScrollRect shopScrollRect;

//    [Header("Popup UI")]
//    public GameObject popupPanel;
//    public RectTransform popupWindow;
//    public Button popupIconButton;
//    public Image popupItemIcon;
//    public TextMeshProUGUI popupItemName;
//    public TextMeshProUGUI popupPriceText;
//    public TextMeshProUGUI popupMessageText;
//    public Button confirmButton;
//    public Button closePopupButton;

//    [Header("Audio Preview")]
//    private Coroutine previewCoroutine;
//    private GameObject currentPreviewTrail;

//    private ShopSlotUI currentSelectedSlot;
//    private List<ShopSlotUI> spawnedSlots = new List<ShopSlotUI>();

//    private bool isEquipMode = false;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;

//        popupPanel.SetActive(false);
//        closePopupButton.onClick.AddListener(ClosePopup);
//        confirmButton.onClick.AddListener(ConfirmAction);

//        if (popupIconButton != null)
//        {
//            popupIconButton.onClick.RemoveAllListeners();
//            popupIconButton.onClick.AddListener(PlayPreviewSoundOnly);
//        }
//    }

//    private void Start()
//    {
//        InitializeShop();
//    }

//    private void InitializeShop()
//    {
//        // 1. 모든 아이템을 가격(BigInteger) 기준으로 오름차순 정렬
//        var sortedItems = ShopManager.Instance.allItems.OrderBy(item =>
//        {
//            // 가격 문자열을 BigInteger로 변환하여 숫자 크기로 비교
//            if (BigInteger.TryParse(item.price, out BigInteger parsedPrice))
//            {
//                return parsedPrice;
//            }
//            // 변환 실패(빈 문자열 등) 시 가장 앞으로 오도록 0 처리
//            return BigInteger.Zero;
//        }).ToList();

//        // 2. 정렬된 리스트를 바탕으로 슬롯 생성
//        foreach (ShopItemData itemData in sortedItems)
//        {
//            Transform targetContent = null;

//            switch (itemData.category)
//            {
//                case ItemCategory.Trail: targetContent = trailContent; break;
//                case ItemCategory.Effect: targetContent = effectContent; break;
//                case ItemCategory.Sound: targetContent = soundContent; break;
//            }

//            if (targetContent != null)
//            {
//                GameObject slotObj = Instantiate(shopSlotPrefab, targetContent);
//                ShopSlotUI slotUI = slotObj.GetComponent<ShopSlotUI>();

//                slotUI.SetupSlot(itemData);
//                spawnedSlots.Add(slotUI);
//            }
//        }
//    }


//    private void ClearContent(Transform content)
//    {
//        if (content == null) return;
//        foreach (Transform child in content)
//        {
//            Destroy(child.gameObject);
//        }
//    }

//    public void OpenPurchasePopup(ShopSlotUI slot)
//    {
//        currentSelectedSlot = slot;
//        ShopItemData data = slot.itemData;

//        popupItemIcon.sprite = data.itemIcon;
//        popupItemName.text = data.itemName;

//        bool isUnlocked = ShopManager.Instance.IsUnlocked(data.itemID);

//        if (isUnlocked)
//        {
//            isEquipMode = true;
//            if (popupMessageText != null) popupMessageText.text = "Do you want equip it?";
//            if (popupPriceText != null) popupPriceText.gameObject.SetActive(false);
//        }
//        else
//        {
//            isEquipMode = false;
//            if (popupMessageText != null) popupMessageText.text = "Do you want buy it?";
//            if (popupPriceText != null)
//            {
//                popupPriceText.gameObject.SetActive(true);
//                if (BigInteger.TryParse(data.price, out BigInteger parsedPrice))
//                {
//                    popupPriceText.text = $"Price: {GameUtility.FormatNumber(parsedPrice)}";
//                }
//                else
//                {
//                    popupPriceText.text = $"Price: {data.price}";
//                }
//            }
//        }

//        popupPanel.SetActive(true);
//        popupWindow.localScale = Vector3.zero;
//        popupWindow.anchoredPosition = new Vector2(0, -50f);

//        popupWindow.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
//        popupWindow.DOAnchorPosY(0f, 0.3f).SetEase(Ease.OutBack);

//        if (previewCoroutine != null) StopCoroutine(previewCoroutine);

//        if (data.category == ItemCategory.Effect || data.category == ItemCategory.Trail)
//        {
//            previewCoroutine = StartCoroutine(VisualPreviewRoutine(data));
//        }
//    }

//    private IEnumerator VisualPreviewRoutine(ShopItemData data)
//    {
//        while (true)
//        {
//            if (data.category == ItemCategory.Effect && data.effectPrefab != null)
//            {
//                if (EffectManager.Instance != null)
//                {
//                    Vector3 effectPos = popupItemIcon.transform.position;
//                    effectPos.z -= 5f;
//                    EffectManager.Instance.SpawnEffect(data.effectPrefab, effectPos);
//                }
//            }
//            else if (data.category == ItemCategory.Trail && data.trailPrefab != null)
//            {
//                if (currentPreviewTrail != null) Destroy(currentPreviewTrail);

//                Vector3 startPos = popupItemIcon.transform.position + new Vector3(-2f, 0, -5f);
//                Vector3 endPos = popupItemIcon.transform.position + new Vector3(2f, 0, -5f);

//                currentPreviewTrail = Instantiate(data.trailPrefab, startPos, UnityEngine.Quaternion.identity);
//                currentPreviewTrail.transform.DOMove(endPos, 0.4f).SetEase(Ease.InOutSine).OnComplete(() =>
//                {
//                    Destroy(currentPreviewTrail, 1f);
//                });
//            }

//            yield return new WaitForSeconds(2.0f);
//        }
//    }

//    public void PlayPreviewSoundOnly()
//    {
//        if (currentSelectedSlot == null) return;

//        ShopItemData data = currentSelectedSlot.itemData;

//        if (data.category == ItemCategory.Sound && data.themeBGM != null)
//        {
//            SoundManager.Instance.PlaySound(data.themeBGM.soundName);
//        }
//    }

//    private void ClosePopup()
//    {
//        if (previewCoroutine != null)
//        {
//            StopCoroutine(previewCoroutine);
//            previewCoroutine = null;
//        }

//        SoundManager.Instance.StopBGM();

//        if (currentPreviewTrail != null)
//        {
//            Destroy(currentPreviewTrail);
//        }

//        popupWindow.DOScale(0f, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
//        {
//            popupPanel.SetActive(false);
//        });
//    }

//    private void ConfirmAction()
//    {
//        ShopItemData data = currentSelectedSlot.itemData;

//        if (isEquipMode)
//        {
//            ShopManager.Instance.EquipItem(data);
//            RefreshAllSlots();

//            if (AppearanceManager.Instance != null)
//            {
//                AppearanceManager.Instance.ApplyEquippedItems();
//            }
//        }
//        else
//        {
//            bool isSuccess = ShopManager.Instance.TryPurchaseItem(data);
//            if (isSuccess)
//            {
//                PlayerPrefs.SetInt("Unlocked_" + data.itemID, 1);
//                PlayerPrefs.Save();

//                if (CurrencyUIManager.Instance != null)
//                {
//                    CurrencyUIManager.Instance.UpdateCurrencyUI();
//                }
//                RefreshAllSlots();
//            }
//        }

//        ClosePopup();
//    }

//    public void RefreshAllSlots()
//    {
//        foreach (ShopSlotUI slot in spawnedSlots)
//        {
//            slot.RefreshSlotState();
//        }
//    }
//}