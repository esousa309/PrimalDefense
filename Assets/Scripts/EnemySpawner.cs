using System.Collections;
using System.Reflection;
using UnityEngine;
using PrimalDefense.Enemies;

namespace PrimalDefense.Spawners
{
    /// <summary>
    /// Spawns enemies along a Transform[] path. Compatible with:
    /// - Legacy EnemyMovement without Init (sets its 'path' field if it exists)
    /// - EnemyMovement with Init(Transform[]) (invokes it)
    /// - Our EnemyPathFollower (adds and initializes if no compatible mover is present)
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Setup")]
        public GameObject enemyPrefab;   // assign your enemy prefab
        public Transform[] path;         // assign Waypoint_0..N

        [Header("Timing")]
        [Min(0f)] public float spawnDelay = 2f; // interval between spawns
        [Min(1)]  public int count = 5;         // how many to spawn

        private Coroutine routine;

        private void OnEnable()
        {
            routine = StartCoroutine(SpawnRoutine());
        }

        private void OnDisable()
        {
            if (routine != null) StopCoroutine(routine);
            routine = null;
        }

        private IEnumerator SpawnRoutine()
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

            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(enemyPrefab);

                // Ensure core component
                if (go.GetComponent<Enemy>() == null) go.AddComponent<Enemy>();

                // Try to find a legacy component by name without compile-time dependency
                Component legacy = go.GetComponent("EnemyMovement");
                bool initialized = false;

                if (legacy != null)
                {
                    // Prefer a method 'Init(Transform[])' if available
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
                        // Fallback: set a field named 'path'
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

                if (!initialized)
                {
                    // Use our guaranteed mover
                    var follower = go.GetComponent<EnemyPathFollower>();
                    if (follower == null) follower = go.AddComponent<EnemyPathFollower>();
                    follower.Init(path);
                }

                if (spawnDelay > 0f && i < count - 1)
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
