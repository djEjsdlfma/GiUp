/*using LSW._02._Scripts.Manager;
using Moon._01.Script.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;
    private float attackSpeed => 1.0f / (1f + 0.35f * IntervalLevel);
    private float IntervalLevel;

    private float attackTimer;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float Level => IntervalLevel;
    public float ExtraHarv => _extraHarvProb;

    [SerializeField] private Bullet _bullet;
    public float maxRange = 20f;          // 사거리
    public float distEpsilon = 0.05f;

    public int testPener;
    public int testDamage;
    public int testWeight;

    [field:SerializeField] public int CurrentPoint { get; private set; }

    public event Action<int> OnPointChanged; 
    public event Action<UpgradeType, float> OnStatChanged; 
    private float _extraPointMultiplier = 1.0f;
    private float _extraHarvProb = 0f;
    private bool isDead = false;
    private Castle _castle;

    private void Awake()
    {
        ReferenceManager.Instance.Player = this;

        _castle = ReferenceManager.Instance.Castle;

        if (_castle != null)
            _castle.OnCastleDied += CastleDead;
    }

    private void Update()
    {
        if (isDead)
            return;

        attackTimer += Time.deltaTime;
        
        if(attackTimer >= attackSpeed)
        {
            attackTimer = 0f;
            ShootBullet(testWeight);
        }

        //if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        //    Time.timeScale += 1.0f;
        //if (Keyboard.current.rightShiftKey.wasPressedThisFrame)
        //    Time.timeScale -= 1.0f;
    }

    private void ShootBullet(int num = 1)
    {
        List<Enemy> targets = GetNearestEnemy(num);

        bool isShoot = false;


        foreach (Enemy target in targets)
        {
            Bullet b = Instantiate(_bullet, transform.position, Quaternion.identity);
            b.SetBullet(testDamage, testPener);
            b.SetTarget(target);
            isShoot = true;
        }

        if(isShoot)
        {
            SoundPlayer.Instance.Play("Bullet");
        }

    }

    private List<Enemy> GetNearestEnemy(int count = 1)
    {
        List<Enemy> result = new List<Enemy>();
        if (count <= 0 || Enemy.All.Count == 0) return result;

        Vector2 myPos = transform.position;

        // 사거리 안의 적만 후보로 모음
        List<Enemy> candidates = new List<Enemy>();
        foreach (Enemy e in Enemy.All)
        {
            if (Vector2.Distance(myPos, e.transform.position) <= maxRange)
                candidates.Add(e);
        }

        // 우선순위로 정렬: 거리 가까운 순 → 체력 낮은 순 → 랜덤
        candidates.Sort((a, b) =>
        {
            float da = Vector2.Distance(myPos, a.transform.position);
            float db = Vector2.Distance(myPos, b.transform.position);

            // 1. 거리 비교 (epsilon 안이면 같다고 봄)
            if (Mathf.Abs(da - db) > 0.05f)
                return da.CompareTo(db);

            // 2. 체력 비교
            if (a.health != b.health)
                return a.health.CompareTo(b.health);

            // 3. 랜덤
            return Random.value < 0.5f ? -1 : 1;
        });

        // 위에서 count개만큼 자르기 (적이 적으면 있는 만큼만)
        int take = Mathf.Min(count, candidates.Count);
        for (int i = 0; i < take; i++)
            result.Add(candidates[i]);

        return result;
    }

    public void UpgradeStat(UpgradeType upgradeType, float amount, float overflow, int price)
    {
        switch (upgradeType)
        {
            case UpgradeType.AtkPower:
                testDamage += (int)amount;
                break;
            case UpgradeType.AtkSpeed:
                IntervalLevel += amount;
                testDamage += (int)(overflow / 2);
                break;
            case UpgradeType.ExtraAtk: 
                testWeight = (int)amount + 1;
                testPener += (int)overflow;
                break;
            case UpgradeType.WallMaxHp:
                // OnStatChanged?.Invoke(upgradeType, amount); <- 에서 처리
                break;
            case UpgradeType.WallAutoRepair:   // 성벽 자동수리량
                // OnStatChanged?.Invoke(upgradeType, amount); <- 에서 처리
                break;
            case UpgradeType.PointMultipe:
                _extraPointMultiplier += amount;
                break;
            case UpgradeType.ExtraHarv:
                _extraHarvProb += amount;
                break;
        }
        CurrentPoint -= price;
        OnStatChanged?.Invoke(upgradeType, amount);
        OnPointChanged?.Invoke(CurrentPoint);
    }
    
    [ContextMenu("Add Point")]
    public void AddPoint()
        => AddPoint(10000);
    
    public void AddPoint(int amount)
    {
        CurrentPoint += (int)(amount * _extraPointMultiplier);
        OnPointChanged?.Invoke(CurrentPoint);
    }


    private void CastleDead()
    {
        isDead = true;
    }
}
*/