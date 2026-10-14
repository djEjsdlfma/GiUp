//using csiimnida.CSILib.SoundManager.RunTime;
//using UnityEditor;
//using UnityEngine;

//public enum ItemCategory { None, Trail, Sound, Effect }
//public enum CurrencyType { Point, Gem }

//[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Item Data")]
//public class ShopItemData : ScriptableObject
//{
//    [Header("Basic Info")]
//    public string itemID;
//    public ItemCategory category;
//    public CurrencyType currencyType;
//    public string itemName;

//    [Tooltip("BigInteger 변환을 위해 문자열로 가격 입력 (예: 100, 10000)")]
//    public string price = "100";

//    [TextArea]
//    public string description;

//    [Header("Equip Assets (Optional)")]
//    public Sprite itemIcon;            // 상점 표시 및 Skin 카테고리 장착용
//    public GameObject trailPrefab;     // Trail 카테고리용 (카드 이동 궤적)
//    public GameObject effectPrefab;    // Effect 카테고리용 (적 처치 폭발 VFX)
//    public SoundSo themeBGM;           // Sound 카테고리용 (타격음 등)

//    [Header("Extra Data")]
//    public string stringParameter;

//#if UNITY_EDITOR
//    private void OnValidate()
//    {
//        // itemName이 비어있으면 무시
//        if (string.IsNullOrWhiteSpace(itemName)) return;

//        // OnValidate 실행 도중 즉시 에셋을 수정하면 에러가 발생할 수 있으므로, 프레임이 끝난 후 실행하도록 지연 호출
//        EditorApplication.delayCall += () =>
//        {
//            // 지연 호출 도중 오브젝트가 파괴되었을 수 있으므로 null 체크
//            if (this == null) return;

//            string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());

//            // 프로젝트에 아직 파일로 저장되지 않은 메모리 상의 객체라면 무시
//            if (string.IsNullOrEmpty(assetPath)) return;

//            // 현재 실제 파일 이름 추출 (확장자 제외)
//            string currentFileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

//            // 인스펙터에 적은 itemName과 실제 파일 이름이 다를 경우에만 이름 변경 시도
//            if (currentFileName != itemName)
//            {
//                AssetDatabase.RenameAsset(assetPath, itemName);
//                AssetDatabase.SaveAssets();
//            }
//        };
//    }
//#endif
//}