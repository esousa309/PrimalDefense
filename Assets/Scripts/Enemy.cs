using UnityEngine;

namespace PrimalDefense.Enemies
{
    /// <summary>
    /// Minimal enemy data. Extend with HP/armor/types per the GDD later.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        [Header("Movement")]
        [Min(0.1f)] public float moveSpeed = 2.0f;

        [Tooltip("How close to a waypoint counts as 'arrived'.")]
        [Range(0.01f, 0.5f)] public float reachThreshold = 0.05f;
    }
}
