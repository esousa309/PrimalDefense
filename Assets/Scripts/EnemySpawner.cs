using System.Collections;
using System.Reflection;
using UnityEngine;
using PrimalDefense.Enemies;

namespace PrimalDefense.Spawners
{
    /// <summary>
    /// Spawns enemies along a Transform[] path with support for multiple waves.
    /// Compatible with:
    ///  - Legacy EnemyMovement (no Init): sets its 'path' field via reflection if present
    ///  - EnemyMovement with Init(Transform[]): invokes it if present
    ///  - Our EnemyPathFollower (added automatically if no compatible mover exists)
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Setup")]
        public GameObject enemyPrefab;        // assign your enemy prefab
        public Transform[] path;              // Waypoint_0..N, in order

        [Header("Per-Unit Timing")]
        [Min(0f)] public float spawnDelay = 2f; // time between units within a wave

        [Header("Wave Settings")]
        [Min(1)]  public int countPerWave = 5;  // enemies per wave
        [Min(0f)] public float firstWaveDelay = 0f;
        [Min(0f)] public float interWaveDelay = 5f;
        [Min(1)]  public int numberOfWaves = 3; // if loopForever is false
        public bool loopForever = true;         // set false to run a finite number of waves

        private Coroutine routine;

        private void OnEnable()
        {
            routine = StartCoroutine(Run());
        }

        private void OnDisable()
        {
            if (routine != null) StopCoroutine(routine);
            routine = null;
        }

        private IEnumerator Run()
        {
            if (enemyPrefab == null)
            {
                Debug.LogWarning("[EnemySpawner] enemyPrefab not assigned.");
                yield break;
            }
            if (path == null || path.Length == 0)
            {
                Debug.LogWarning("[EnemySpawner] path is empty.");
                yield break;
            }

            if (firstWaveDelay > 0f) yield return new WaitForSeconds(firstWaveDelay);

            if (loopForever)
            {
                while (true)
                {
                    yield return SpawnWave();
                    if (interWaveDelay > 0f) yield return new WaitForSeconds(interWaveDelay);
                }
            }
            else
            {
                for (int wave = 0; wave < numberOfWaves; wave++)
                {
                    yield return SpawnWave();
                    if (wave < numberOfWaves - 1 && interWaveDelay > 0f)
                        yield return new WaitForSeconds(interWaveDelay);
                }
            }
        }

        private IEnumerator SpawnWave()
        {
            for (int i = 0; i < countPerWave; i++)
            {
                GameObject go = Instantiate(enemyPrefab);

                // Ensure Enemy exists
                if (go.GetComponent<Enemy>() == null) go.AddComponent<Enemy>();

                // Try to use your existing EnemyMovement if present
                Component legacy = go.GetComponent("EnemyMovement");
                bool initialized = false;

                if (legacy != null)
                {
                    // Prefer Init(Transform[]) if it exists
                    MethodInfo init = legacy.GetType().GetMethod(
                        "Init",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null,
                        new[] { typeof(Transform[]) },
                        null
                    );
                    if (init != null)
                    {
                        init.Invoke(legacy, new object[] { path });
                        initialized = true;
                    }
                    else
                    {
                        // Otherwise set a field named 'path' if present
                        FieldInfo pathField = legacy.GetType().GetField(
                            "path",
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                        );
                        if (pathField != null)
                        {
                            pathField.SetValue(legacy, path);
                            initialized = true;
                        }
                    }
                }

                // If legacy didn’t initialize, fall back to our guaranteed mover
                if (!initialized)
                {
                    var follower = go.GetComponent<EnemyPathFollower>();
                    if (follower == null) follower = go.AddComponent<EnemyPathFollower>();
                    follower.Init(path);
                }

                if (spawnDelay > 0f && i < countPerWave - 1)
                    yield return new WaitForSeconds(spawnDelay);
            }
        }

        private void OnDrawGizmos()
        {
            if (path == null || path.Length == 0) return;

            for (int i = 0; i < path.Length; i++)
            {
                var t = path[i];
                if (t == null) continue;

                Gizmos.DrawSphere(t.position, 0.2f);

                if (i < path.Length - 1 && path[i + 1] != null)
                    Gizmos.DrawLine(t.position, path[i + 1].position);
            }
        }
    }
}
