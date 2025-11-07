using UnityEngine;

namespace PrimalDefense.Enemies
{
    /// <summary>
    /// Single, standard enemy mover for the project.
    /// Does NOT require a path in the Inspector; the spawner calls Init(path).
    /// Safe if left on the prefab without Init (it stays idle until initialized).
    /// </summary>
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Transform[] path;               // set by Init()
        [SerializeField, Range(0.01f, 0.5f)] private float reach = 0.05f;

        private Enemy enemy;
        private int index = 0;
        private bool hasPath = false;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
        }

        /// <summary>
        /// Called by the spawner right after Instantiate.
        /// Snaps to first waypoint and begins traversal.
        /// </summary>
        public void Init(Transform[] waypoints)
        {
            path = waypoints;
            hasPath = (path != null && path.Length > 0 && path[0] != null);
            index = 0;

            if (!hasPath)
            {
                Debug.LogWarning("[EnemyMovement] Init called with null/empty path. Disabling.");
                enabled = false;
                return;
            }

            transform.position = path[0].position;
            enabled = true;
        }

        private void Start()
        {
            // Back-compat: if someone assigned path via Inspector, use it.
            if (!hasPath && path != null && path.Length > 0 && path[0] != null)
            {
                hasPath = true;
                index = 0;
                transform.position = path[0].position;
                enabled = true;
            }
        }

        private void Update()
        {
            if (!hasPath || path == null || path.Length == 0 || index >= path.Length) return;

            var target = path[index];
            if (target == null) { index++; return; }

            Vector3 toTarget = target.position - transform.position;
            float dist = toTarget.magnitude;

            // Arrived at waypoint?
            if (dist <= Mathf.Max(0.0001f, Mathf.Max(reach, enemy.reachThreshold)))
            {
                index++;
                if (index >= path.Length)
                {
                    // TODO: hook into base HP/lives when reaching goal.
                    Destroy(gameObject);
                }
                return;
            }

            // Move and face direction
            float step = enemy.moveSpeed * Time.deltaTime;
            transform.position += toTarget.normalized * step;

            if (toTarget.sqrMagnitude > 0.0001f)
            {
                transform.forward = Vector3.Lerp(transform.forward, toTarget.normalized, 0.25f);
            }
        }
    }
}
