/*using DG.Tweening;
using UnityEngine;

public class KunaiPrefab : MonoBehaviour
{
    private float timer = 0;

    private void Awake()
    {
        transform.DOMove(Vector3.zero, 0.3f)
            .OnUpdate(() =>
            {
                transform.DOScale(Vector3.zero, 0.4f);
            });
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.4f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Target target))
        {
            if (timer < 0.3f)
            {
                target.CheckDistance();

                if (target.CanDestroy())
                    Destroy(gameObject);
            }
        }

    }
}*/