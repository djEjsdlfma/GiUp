/*using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public int damage = 2;
    public int PentrationValue = 0;

    private Vector3 direction;

    // 발사 순간 적 위치로 방향 고정
    public void SetTarget(Enemy enemy)
    {
        direction = (enemy.transform.position - transform.position).normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().TakeDamage(damage);

            if(PentrationValue > 0)
                PentrationValue--;
             else
               Destroy(gameObject);
        }
        if (other.CompareTag("Destoryer"))
        {
            Destroy(gameObject);
        }
    }

    public void SetBullet(int dmg, int Penetration = 0)
    {
        PentrationValue = Penetration;
        damage = dmg;
    }
}
*/