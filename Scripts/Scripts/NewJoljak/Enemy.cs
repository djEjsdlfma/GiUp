/*using Moon._01.Script.Sounds;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static readonly List<Enemy> All = new List<Enemy>();

    public float maxHealth = 10;
    public float health;
    public float moveSpeed = 2f;

    public float attackDamage = 5;
    public float attackInterval = 1f;

    private float attackTimer;   // 이 적만의 개별 타이머

    void Awake()
    {
        health = maxHealth;
    }

    void Update()
    {
        // 왼쪽에서 오른쪽으로 직진
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    void OnEnable() { if (!All.Contains(this)) All.Add(this); }
    void OnDisable() { All.Remove(this); }

    void OnCollisionStay2D(Collision2D collision)
    {
        moveSpeed = 0f;

        Castle castle = collision.gameObject.GetComponent<Castle>();
        if (castle == null) return;

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;
            castle.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        SoundPlayer.Instance.Play("EnemyHit");
        if (health <= 0)
            Die();
    }

    void Die()
    {
        // 점수, 이펙트, 사운드, 드랍 등은 여기에
        Destroy(gameObject);   // OnDisable에서 All.Remove 자동 처리
    }

    public void SetEnemyStat(float multiply = 1.0f)
    {
        maxHealth *= multiply;
        attackDamage *= multiply;
    }
}
*/