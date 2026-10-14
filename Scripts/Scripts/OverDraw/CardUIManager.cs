//using DG.Tweening;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public enum DamageType { Normal, Perfect, Bust }
//public class CardUIManager : MonoBehaviour
//{
//    public static CardUIManager Instance;

//    [Header("UI - Card Spawning")]
//    public GameObject cardPrefab;
//    public RectTransform spawnPoint;
//    public RectTransform cardGatherPoint;

//    [Header("Card Sprites Setup")]
//    // 인스펙터에서 52장 할당 (스페이드 13장, 하트 13장, 다이아 13장, 클로버 13장 순서)
//    public Sprite[] cardFrontSprites;

//    [Header("Animation Settings")]
//    public float cardSpacing = 80f;
//    public float moveDuration = 0.5f;

//    [Header("Damage Popup")]
//    public GameObject damagePopupPrefab; // TextMeshProUGUI가 달린 프리팹
//    public Transform popupCanvasTransform;

//    [Header("Damage Popup Fonts")]
//    public TMP_FontAsset normalFont;  
//    public TMP_FontAsset perfectFont; 
//    public TMP_FontAsset bustFont;

//    private List<Card> deck = new List<Card>();
//    private List<GameObject> handCardObjects = new List<GameObject>();

//    public Button standButton;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private void Start()
//    {
//        InitDeck();
//    }

//    private void InitDeck()
//    {
//        deck.Clear();
//        for (int s = 0; s < 4; s++)
//        {
//            for (int v = 1; v <= 13; v++)
//            {
//                int cardValue = v > 10 ? 10 : v;
//                deck.Add(new Card((CardSuit)s, (CardType)(v - 1), cardValue));
//            }
//        }
//        ShuffleDeck();
//    }

//    private void ShuffleDeck()
//    {
//        for (int i = 0; i < deck.Count; i++)
//        {
//            int randomIndex = Random.Range(0, deck.Count);
//            Card temp = deck[i];
//            deck[i] = deck[randomIndex];
//            deck[randomIndex] = temp;
//        }
//    }

//    public Card DrawCard()
//    {
//        if (deck.Count == 0) InitDeck();

//        Card drawnCard = deck[0];
//        deck.RemoveAt(0);

//        SpawnAndAnimateCard(drawnCard);

//        return drawnCard;
//    }

//    public void UpdateStandButtonState()
//    {
//        if (standButton != null)
//        {
//            standButton.interactable = (handCardObjects.Count > 0);
//        }
//    }

//    private void SpawnAndAnimateCard(Card cardData)
//    {
//        GameObject newCard = Instantiate(cardPrefab, cardGatherPoint);
//        RectTransform cardRect = newCard.GetComponent<RectTransform>();
//        Image cardImage = newCard.GetComponent<Image>();

//        // 데이터 기반으로 52장 중 알맞은 앞면 할당
//        int spriteIndex = ((int)cardData.suit * 13) + (int)cardData.type;
//        if (spriteIndex < cardFrontSprites.Length)
//            cardImage.sprite = cardFrontSprites[spriteIndex];

//        cardRect.position = spawnPoint.position;
//        cardRect.localScale = Vector3.zero;
//        cardRect.localRotation = Quaternion.Euler(0, 0, -45f);

//        int cardIndex = handCardObjects.Count;
//        Vector2 targetPosition = new Vector2(cardIndex * cardSpacing, 0f);

//        // 크기가 팡 터지면서 날아오고 회전 복구
//        Sequence seq = DOTween.Sequence();
//        seq.Append(cardRect.DOAnchorPos(targetPosition, moveDuration).SetEase(Ease.OutQuad));
//        seq.Join(cardRect.DOScale(1f, moveDuration).SetEase(Ease.OutBack));
//        seq.Join(cardRect.DORotate(Vector3.zero, moveDuration).SetEase(Ease.OutBack));

//        if (AppearanceManager.Instance != null)
//        {
//            GameObject trailPrefab = AppearanceManager.Instance.GetEquippedTrailPrefab();
//            if (trailPrefab != null)
//            {
//                // 생성된 카드 UI의 자식으로 트레일 부착
//                Instantiate(trailPrefab, newCard.transform.position, Quaternion.identity, newCard.transform);
//            }
//        }

//        handCardObjects.Add(newCard);


//        int drawnCount = handCardObjects.Count;
//        if (drawnCount >= 2)
//        {
//            // 2장이면 1회 추가, 3장이면 2회 추가 드로우로 계산
//            GameManager.Instance.totalExtraDrawCount += (drawnCount - 1);
//        }
//    }

//    public void ShootCardsToEnemy(Transform targetTransform, System.Action onComplete)
//    {
//        Sequence shootSeq = DOTween.Sequence();

//        for (int i = 0; i < handCardObjects.Count; i++)
//        {
//            GameObject cardObj = handCardObjects[i];

//            shootSeq.Insert(0f, cardObj.transform.DOMove(targetTransform.position, 0.3f).SetEase(Ease.InBack));
//            shootSeq.Insert(0.2f, cardObj.transform.DOScale(0.5f, 0.1f));
//            shootSeq.InsertCallback(0.3f, () => Destroy(cardObj));
//        }

//        shootSeq.OnComplete(() =>
//        {
//            handCardObjects.Clear();
//            onComplete?.Invoke();
//            UpdateStandButtonState();
//        });
//    }

//    public void ShootCardsToPlayer(Transform targetTransform, System.Action onComplete)
//    {
//        Sequence shootSeq = DOTween.Sequence();

//        for (int i = 0; i < handCardObjects.Count; i++)
//        {
//            GameObject cardObj = handCardObjects[i];
//            float delay = i * 0.1f;

//            // 적에게 날아갈 때와 완전히 동일한 연출 (목표 지점으로 빨려들어가며 작아짐)
//            shootSeq.Insert(delay, cardObj.transform.DOMove(targetTransform.position, 0.3f).SetEase(Ease.InBack));
//            shootSeq.Insert(delay + 0.2f, cardObj.transform.DOScale(0.5f, 0.1f));

//            shootSeq.InsertCallback(delay + 0.3f, () => Destroy(cardObj));
//        }

//        shootSeq.OnComplete(() =>
//        {
//            handCardObjects.Clear();
//            onComplete?.Invoke();
//            UpdateStandButtonState();
//        });
//    }

//    public void SpawnDamagePopup(Vector3 targetPosition, int damage, DamageType type)
//    {
//        if (damagePopupPrefab == null || popupCanvasTransform == null) return;

//        GameObject popup = Instantiate(damagePopupPrefab, popupCanvasTransform);
//        RectTransform popupRect = popup.GetComponent<RectTransform>();
//        TextMeshProUGUI popupText = popup.GetComponent<TextMeshProUGUI>();

//        // 1. 기준점이 되는 타겟의 월드 위치에 먼저 맞춥니다.
//        popupRect.position = targetPosition;

//        // 2. 월드 좌표가 아닌 UI 픽셀 좌표(anchoredPosition)를 기준으로 랜덤 오프셋을 더해줍니다.
//        popupRect.anchoredPosition += new Vector2(Random.Range(-50f, 50f), Random.Range(-30f, 30f));

//        popupText.text = GameUtility.FormatNumber(damage);

//        float moveY = 100f;
//        float duration = 0.6f;

//        switch (type)
//        {
//            case DamageType.Normal:
//                popupText.color = Color.white;
//                popupText.fontSize = 50f;
//                if (normalFont != null) popupText.font = normalFont;
//                break;

//            case DamageType.Perfect:
//                popupText.color = new Color(1f, 0.8f, 0f);
//                popupText.fontSize = 80f;
//                if (perfectFont != null) popupText.font = perfectFont;
//                moveY = 150f;
//                duration = 0.8f;

//                popup.transform.localScale = Vector3.zero;
//                popup.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
//                break;

//            case DamageType.Bust:
//                popupText.color = new Color(1f, 0.2f, 0.2f);
//                popupText.fontSize = 65f;
//                if (bustFont != null) popupText.font = bustFont;
//                moveY = -50f; // 픽셀 기준으로 아래로 50 이동

//                popup.transform.DOShakePosition(0.3f, 10f, 15);
//                break;
//        }

//        Sequence popupSeq = DOTween.Sequence();
//        popupSeq.Append(popupRect.DOAnchorPosY(popupRect.anchoredPosition.y + moveY, duration).SetEase(Ease.OutCubic));
//        popupSeq.Join(popupText.DOFade(0f, duration).SetEase(Ease.InQuad));
//        popupSeq.OnComplete(() => Destroy(popup));
//    }

//    public void ClearHandCards()
//    {
//        foreach (var cardObj in handCardObjects)
//        {
//            cardObj.transform.DOKill();
//            Destroy(cardObj);
//        }
//            handCardObjects.Clear();
//    }
//}