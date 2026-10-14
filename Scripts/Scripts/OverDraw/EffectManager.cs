//using System.Collections.Generic;
//using UnityEngine;

//public class EffectManager : MonoBehaviour
//{
//    public static EffectManager Instance;

//    // 프리팹 종류별로 큐(Queue)를 생성하여 보관하는 딕셔너리
//    private Dictionary<GameObject, Queue<GameObject>> effectPools = new Dictionary<GameObject, Queue<GameObject>>();

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    // 외부(EnemyManager 등)에서 이펙트 생성을 요청할 때 호출하는 함수
//    public GameObject SpawnEffect(GameObject prefab, Vector3 position)
//    {
//        if (prefab == null) return null;

//        // 해당 프리팹 전용 보관함이 없다면 새로 만듦
//        if (!effectPools.ContainsKey(prefab))
//        {
//            effectPools[prefab] = new Queue<GameObject>();
//        }

//        GameObject effectInstance = null;

//        // 보관함에 남은 이펙트가 있는지 확인하고 꺼냄
//        while (effectPools[prefab].Count > 0)
//        {
//            GameObject obj = effectPools[prefab].Dequeue();
//            // 파괴되지 않았고 현재 꺼져있는 오브젝트만 재사용
//            if (obj != null && !obj.activeInHierarchy)
//            {
//                effectInstance = obj;
//                break;
//            }
//        }

//        // 보관함이 비어있다면 새로 인스턴스화
//        if (effectInstance == null)
//        {
//            effectInstance = Instantiate(prefab);

//            // 이펙트가 스스로 풀에 돌아갈 수 있도록 자동 반납 스크립트 부착
//            PooledEffect returner = effectInstance.GetComponent<PooledEffect>();
//            if (returner == null)
//            {
//                returner = effectInstance.AddComponent<PooledEffect>();
//            }
//            returner.originalPrefab = prefab; // 자기가 어떤 프리팹에서 나왔는지 기억시킴
//        }

//        // 위치 적용 및 활성화
//        effectInstance.transform.position = position;
//        effectInstance.SetActive(true);

//        return effectInstance;
//    }

//    // 재생이 끝난 이펙트를 다시 보관함으로 넣는 함수 (PooledEffect 스크립트가 알아서 호출함)
//    public void ReturnToPool(GameObject prefab, GameObject instance)
//    {
//        if (prefab == null || instance == null) return;

//        instance.SetActive(false);

//        if (!effectPools.ContainsKey(prefab))
//        {
//            effectPools[prefab] = new Queue<GameObject>();
//        }
//        effectPools[prefab].Enqueue(instance);
//    }
//}
