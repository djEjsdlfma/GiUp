/*using LSW._02._Scripts.So;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Test")]
    [Range(1f, 5f)]
    public float TimerMultiply;

    [Header("Enemy Prefabs")]
    public Enemy[] enemyPrefab;
    public Vector2 spawnAreaSize = new Vector2(10f, 6f);  // 스폰 범위 (가로, 세로)

    [Header("Data Source")]
    [SerializeField] private StageData StageStat;

    [Header("Runtime Game Progress")]
    [SerializeField] private float gameTime = 0f; // 매 프레임 누적되는 게임 시간 (초 단위)
    private int currentProgress = -1; // 현재 진행도 (분 단위)

    // SO에서 실시간으로 가져와 적용할 스탯 변수들
    private float spawnInterval = 1f;
    private int spawnCount = 1;
    private int typeNProb = 100;
    private int typeSProb = 0;
    private int typeTProb = 0;
    private float enemyMultiply = 1.0f;

    private float timer;
    private bool canSpawn;

    void Start()
    {
        // 게임 시작 시 초기 스탯(Progress 0) 적용
        UpdateStageStats();

        if (GameModeManager.CurrentMode != GameMode.Tutorial)
            EnemySpawn.onSpawn += ChangeSpawn;
    }

    void Update()
    {
        if (GameModeManager.CurrentMode != GameMode.Tutorial)
        {
            // 1. 시간이 흐름에 따라 gameTime 누적 (초 단위)
            gameTime += Time.deltaTime * TimerMultiply;

            // 수정된 부분: 누적된 초를 60으로 나누어 '분(Minute)' 단위의 Progress 계산
            int calculatedProgress = Mathf.FloorToInt(gameTime / 60f);

            // 2. 분 단위 Progress 조건이 바뀔 때만 SO에서 스탯 새로 가져오기
            if (calculatedProgress != currentProgress)
            {
                currentProgress = calculatedProgress;
                UpdateStageStats();
            }

            // 3. SO에서 실시간 갱신된 spawnInterval을 기준으로 타이머 작동
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                // SO에서 실시간 갱신된 spawnCount 적용
                for (int i = 1; i <= spawnCount; i++)
                {
                    Spawn();
                }
            }
        }
        else
        {
            if (canSpawn == false)
                return;

            // 1. 시간이 흐름에 따라 gameTime 누적 (초 단위)
            gameTime += Time.deltaTime * TimerMultiply;

            // 수정된 부분: 누적된 초를 60으로 나누어 '분(Minute)' 단위의 Progress 계산
            int calculatedProgress = Mathf.FloorToInt(gameTime / 60f);

            // 2. 분 단위 Progress 조건이 바뀔 때만 SO에서 스탯 새로 가져오기
            if (calculatedProgress != currentProgress)
            {
                currentProgress = calculatedProgress;
                UpdateStageStats();
            }

            // 3. SO에서 실시간 갱신된 spawnInterval을 기준으로 타이머 작동
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                // SO에서 실시간 갱신된 spawnCount 적용
                for (int i = 1; i <= spawnCount; i++)
                {
                    Spawn();
                }
            }
        }
    }

    /// <summary>
    /// 현재 Progress(분)에 알맞은 Stage 행의 데이터를 가져와 스포너 스탯을 갱신합니다.
    /// </summary>
    private void UpdateStageStats()
    {
        if (StageStat == null)
        {
            Debug.LogWarning("StageStat(StageData SO)이 할당되지 않았습니다.");
            return;
        }

        Stage currentStage = GetStageFromSO(currentProgress);

        // Progress를 제외한 모든 스탯 실시간 반영
        spawnInterval = currentStage.delay;
        spawnCount = currentStage.count;
        typeNProb = currentStage.typeNProb;
        typeSProb = currentStage.typeSProb;
        typeTProb = currentStage.typeTProb;
        enemyMultiply = currentStage.multiply;

    }

    void Spawn()
    {
        // 스폰 범위 안의 랜덤 좌표 계산
        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float y = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        Vector2 spawnPos = (Vector2)transform.position + new Vector2(x, y);

        // 확률 누적합 난수 생성
        int totalWeight = typeNProb + typeSProb + typeTProb;
        int rand = Random.Range(0, totalWeight);

        // 시트에서 가져온 확률 데이터 기반으로 가중치 스폰 확률 적용
        if (rand < typeNProb)
        {
            Enemy enemy = Instantiate(enemyPrefab[0], spawnPos, Quaternion.identity);
            enemy.SetEnemyStat(enemyMultiply);
        }
        else if (rand < typeNProb + typeSProb)
        {
            Enemy enemy = Instantiate(enemyPrefab[1], spawnPos, Quaternion.identity);
            enemy.SetEnemyStat(enemyMultiply);
        }
        else
        {
            Enemy enemy = Instantiate(enemyPrefab[2], spawnPos, Quaternion.identity);
            enemy.SetEnemyStat(enemyMultiply);
        }
    }


    /// <summary>
    /// StageData SO 내부 리스트에서 현재 Progress '이상'인 행 중 가장 가까운 데이터를 반환합니다.
    /// </summary>
    private Stage GetStageFromSO(int progress)
    {
        var stages = StageStat.stagesContainer.stages;
        if (stages == null || stages.Count == 0) return default;

        Stage matched = stages[0];
        foreach (var stage in stages)
        {
            if (progress >= stage.progress)
            {
                matched = stage;
            }
            else
            {
                break;
            }
        }
        return matched;
    }

    private void ChangeSpawn() => canSpawn = !canSpawn;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
*/