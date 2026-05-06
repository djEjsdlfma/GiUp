/*using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform _center;
    [SerializeField] private List<LevelListSO> TargetMovement;
    [SerializeField] private GameObject hitPoint;
    [SerializeField] private CircleCollider2D centerRadius;
    [SerializeField] private GameModuleSO playerPoint;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private FadeUI fadeUI;


    private CircleCollider2D targetRadius;

    private int level = 0;
    private float movingTime = 0;
    private int stage;

    private float timer = 0;
    private float distance = 0f;
    private float _cRadius = 0;
    private float _TRadius = 0;

    private LevelSO nowLevel;

    private bool DoMove = false;
    private bool moveEnd = false;
    private int teleProtNum = 0;

    private int temp = 0;

    private void Awake()
    {
        targetRadius = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        nowLevel = TargetMovement[stage]._Level[level];
    }

    public void TargetInit()
    {
        hitPoint.SetActive(false);
        if (TargetMovement.Count > stage)
        {
            TextMove();
            level = Random.Range(0, TargetMovement[stage]._Level.Count);
            nowLevel = TargetMovement[stage]._Level[level];
            transform.localScale = nowLevel.TargetSize;
            transform.position = nowLevel._targetPos[0];
            distance = 0;

            if (nowLevel.useMoveTimeChange == false)
                movingTime = nowLevel.moveTime;
            else
                movingTime = nowLevel.moveTimeList[0];

            if (nowLevel._useDealy == false)
            {
                TargetMove();
            }

            _cRadius = (centerRadius.radius * transform.lossyScale.x) * 3;
            _TRadius = targetRadius.radius * transform.lossyScale.x;
        }
        else
            fadeUI.OnFade("EndingScene");
    }

    private void Update()
    {
        if (nowLevel != null && nowLevel._useDealy != false)
        {
            timer += Time.deltaTime;
        }
        if (moveEnd == false && nowLevel._useDealy != false && DoMove == true)
        {
            TeleportMove(teleProtNum);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && temp == 0)
        {
            temp++;
            TargetInit();
            TextMove();
        }
    }

    private void TeleportMove(int num = 0)
    {
        if (timer > nowLevel.DealyTime)
        {
            if (num < nowLevel._targetPos.Count)
            {
                transform.position = nowLevel._targetPos[num];
                timer = 0;
                teleProtNum++;
            }
            else
            {
                CaculatePoint();
                moveEnd = true;
            }
        }
    }

    private void BounceMove()
    {
        float Yvalue = Mathf.Abs(Mathf.Sin(Time.time * 5f)) * 2f;
        transform.position = new Vector3(transform.position.x, Yvalue);

    }

    private void TargetMove(int num = 0)
    {
        transform.DOMove(nowLevel._targetPos[num], movingTime).SetEase(nowLevel.easeGraph)
         .OnUpdate(() =>
         {
             if (nowLevel._useBounce != false)
             {
                 BounceMove();
             }
         })
        .OnComplete(() =>
        {
            gameObject.SetActive(true);
            int value = 0;
            if (num < nowLevel._targetPos.Count - 1)
            {
                value = num + 1;
                if (nowLevel.useMoveTimeChange == true && value < nowLevel.moveTimeList.Count)
                    movingTime = nowLevel.moveTimeList[value];
                TargetMove(++num);
            }
            else
                CaculatePoint();
        });
    }

    private void CaculatePoint()
    {
        stage++;
        transform.DOMove(Vector3.zero, 0.2f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            if (nowLevel._isHitMiddlePoint)
            {
                playerPoint.GameScore += 100 + (10 * playerPoint._targetRen);
                playerPoint._targetRen++;
                InGameUI.Instance.ShowUI(nowLevel._isHitMiddlePoint, nowLevel._isHit, 50, playerPoint._targetRen);
            }
            else if (nowLevel._isHit)
            {
                playerPoint.GameScore += (50 - Mathf.CeilToInt(distance * (transform.localScale.x * 25))) + (10 * playerPoint._targetRen);
                playerPoint._targetRen++;
                InGameUI.Instance.ShowUI(nowLevel._isHitMiddlePoint, nowLevel._isHit,
                    50 - Mathf.CeilToInt(distance * (transform.localScale.x * 25)), playerPoint._targetRen);
            }
            else
            {
                playerPoint._targetRen = 0;
                InGameUI.Instance.ShowUI(nowLevel._isHitMiddlePoint, nowLevel._isHit,
                    50 - Mathf.CeilToInt(distance * (transform.localScale.x * 25)), playerPoint._targetRen);
            }
        });

    }

    public void CheckDistance()
    {
        hitPoint.transform.position = Vector3.zero;
        distance = Vector3.Distance(hitPoint.transform.position, _center.position);

        if (distance < _TRadius)
        {
            impulse.GenerateImpulse(0.65f);
            hitSound.Play();
            hitPoint.SetActive(true);

            if (distance < _cRadius)
            {
                nowLevel._isHitMiddlePoint = true;
                nowLevel._isHit = true;
            }
            else
                nowLevel._isHit = true;
        }
    }

    public bool CanDestroy()
    {
        if (distance < _TRadius)
        {
            return true;
        }
        else return false;
    }

    public void TextMove()
    {
        roundText.text = (stage + 1).ToString() + "¹øÂ°";
        roundText.gameObject.transform.DOMoveY(700f, 0.7f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            roundText.gameObject.transform.DOMoveY(1400f, 0.5f).SetEase(Ease.InBack);
            DoMove = true;
        });
    }
}*/