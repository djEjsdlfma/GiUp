//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.Collections.Generic;
//using csiimnida.CSILib.SoundManager.RunTime;

//public class AppearanceManager : MonoBehaviour
//{
//    public static AppearanceManager Instance;

//    [Header("Default Equip Items")]
//    // 처음에 기본으로 입혀줄 아이템 ID들을 인스펙터에 적어줍니다.
//    public List<string> defaultEquipItemIDs = new List<string> { "DefaultTrail", "DefaultEffect", "DefaultSound" };

//    [Header("Equipped Cosmetics")]
//    public List<ShopItemData> cosmeticItems = new List<ShopItemData>();

//    [Header("Target Components")]
//    public Image playerClassIcon;

//    private SoundSo currentHitSound;
//    public GameObject currentTrailPrefab { get; private set; }
//    public GameObject currentVFXPrefab { get; private set; }

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private void Start()
//    {
//        // 게임 최초 실행 여부 체크 및 기본 장착
//        CheckFirstLaunchDefaultEquip();
//        ApplyEquippedItems();
//    }

//    private void CheckFirstLaunchDefaultEquip()
//    {
//        // "IsFirstLaunch" 값이 없다면(0이라면) 최초 실행
//        if (PlayerPrefs.GetInt("IsFirstLaunch", 0) == 0)
//        {
//            Debug.Log("게임 최초 실행: 기본 외형(아이템)을 세팅합니다.");

//            // ShopManager가 초기화되어 데이터(allItems)가 준비되어 있어야 합니다.
//            if (ShopManager.Instance != null && ShopManager.Instance.allItems != null)
//            {
//                foreach (string defaultID in defaultEquipItemIDs)
//                {
//                    // 전체 아이템 리스트에서 해당 ID의 데이터를 찾음
//                    ShopItemData defaultItem = ShopManager.Instance.allItems.Find(item => item.itemID == defaultID);

//                    if (defaultItem != null)
//                    {
//                        // ShopManager의 장착 함수를 빌려와서 기본 아이템 장착 처리
//                        ShopManager.Instance.EquipItem(defaultItem);
//                    }
//                }
//            }

//            if (ShopUIManager.Instance != null)
//            {
//                ShopUIManager.Instance.RefreshAllSlots();
//            }

//            // 최초 실행 처리가 끝났으므로 1로 저장하여 다음부터는 실행되지 않게 함
//            PlayerPrefs.SetInt("IsFirstLaunch", 1);
//            PlayerPrefs.Save();
//        }
//    }

//    public void ApplyEquippedItems()
//    {
//        // 1. 기존 장착 리스트를 비웁니다 (새로고침을 위해)
//        cosmeticItems.Clear();

//        // 2. ShopManager의 모든 아이템 중 '현재 장착된' 아이템만 찾아 리스트에 넣습니다.
//        if (ShopManager.Instance != null && ShopManager.Instance.allItems != null)
//        {
//            foreach (ShopItemData item in ShopManager.Instance.allItems)
//            {
//                string equippedID = PlayerPrefs.GetString("Equipped_" + item.category.ToString(), "");
//                if (item.itemID == equippedID)
//                {
//                    cosmeticItems.Add(item);
//                }
//            }
//        }

//        foreach (ShopItemData data in cosmeticItems)
//        {
//            if (data.category == ItemCategory.Effect && data.effectPrefab != null)
//            {
//                ApplyVFX();
//            }
//            else if (data.category == ItemCategory.Trail && data.trailPrefab != null)
//            {
//                ApplyTrail();
//            }
//            else if (data.category == ItemCategory.Sound && data.themeBGM != null)
//            {
//                ApplyHitSound();
//            }
//        }
//    }

//    private void ApplyTrail()
//    {
//        // 기본 트레일 ID를 "DefaultTrail"로 가정
//        string equippedID = PlayerPrefs.GetString("Equipped_Trail", "DefaultTrail");
//        ShopItemData data = cosmeticItems.Find(x => x.itemID == equippedID);

//        if (data != null && data.trailPrefab != null)
//        {
//            currentTrailPrefab = data.trailPrefab;
//        }
//    }

//    private void ApplyVFX()
//    {
//        // 기본 VFX ID를 "DefaultEffect"로 가정
//        string equippedID = PlayerPrefs.GetString("Equipped_VFX", "DefaultEffect");
//        ShopItemData data = cosmeticItems.Find(x => x.itemID == equippedID);

//        if (data != null && data.effectPrefab != null)
//        {
//            currentVFXPrefab = data.effectPrefab;
//        }
//    }

//    private void ApplyHitSound()
//    {
//        string equippedID = PlayerPrefs.GetString("Equipped_Sound", "DefaultSound");
//        ShopItemData data = cosmeticItems.Find(x => x.itemID == equippedID);
//        if (data != null && data.themeBGM != null)
//        {
//            currentHitSound = data.themeBGM;
//        }
//    }

//    private void ApplyClass()
//    {
//        string equippedID = PlayerPrefs.GetString("Equipped_Class", "Class_Default");
//        ShopItemData data = cosmeticItems.Find(x => x.itemID == equippedID);
//        if (data != null && data.itemIcon != null && playerClassIcon != null)
//        {
//            playerClassIcon.sprite = data.itemIcon;
//        }
//    }

//    public GameObject GetEquippedTrailPrefab()
//    {
//        string equippedID = PlayerPrefs.GetString("Equipped_Trail", "Trail_Default");
//        ShopItemData data = cosmeticItems.Find(x => x.itemID == equippedID);
//        return data != null ? data.trailPrefab : null;
//    }

//    public GameObject GetEquippedEffectPrefab()
//    {
//        string equippedID = PlayerPrefs.GetString("Equipped_Effect", "Effect_Default");
//        ShopItemData data = cosmeticItems.Find(x => x.itemID == equippedID);
//        return data != null ? data.effectPrefab : null;
//    }

//    public void PlayHitSound()
//    {
//        if (SoundManager.Instance != null && currentHitSound != null)
//        {
//            SoundManager.Instance.PlaySound(currentHitSound.soundName);
//        }
//    }
//}