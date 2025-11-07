using UnityEngine;

namespace PrimalDefense.Enemies
{
    /// <summary>
    /// Our standard path follower (no conflicts with legacy EnemyMovement).
    /// The spawner will use this when no compatible legacy mover is found.
    /// </summary>
    [RequireComponent(typeof(Enemy))]
    public class EnemyPathFollower : MonoBehaviour
    {
        [SerializeField] private Transform[] path;
        [SerializeField, Range(0.01f, 0.5f)] private float reach = 0.05f;

        private Enemy enemy;
        private int index;
        private bool active;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
        }

        /// <summary>Called by the spawner. Snaps to the first waypoint.</summary>
        public void Init(Transform[] waypoints)
        {
            path = waypoints;
            index = 0;
            active = (path != null && path.Length > 0 && path[0] != null);
            if (!active)
            {
                enabled = false;
                Debug.LogWarning("[EnemyPathFollower] Empty path.");
                return;
            }
            transform.position = path[0].position;
            enabled = true;
        }

        private void Update()
        {
            if (!active || path == null || index >= path.Length) return;

            var target = path[index];
            if (target == null) { index++; return; }

            Vector3 to = target.position - transform.position;
            float dist = to.magnitude;

            if (dist <= Mathf.Max(0.0001f, Mathf.Max(reach, enemy.reachThreshold)))
            {
                index++;
                if (index >= path.Length)
                {
                    // TODO: hook into base/lives.
                    Destroy(gameObject);
                }
                return;
            }

            float step = enemy.moveSpeed * Time.deltaTime;
            transform.position += to.normalized * step;

            if (to.sqrMagnitude > 0.0001f)
                transform.forward = Vector3.Lerp(transform.forward, to.normalized, 0.25f);
        }
    }
}
