using UnityEngine;

namespace RPG_Adventure
{
    public partial class Damageable : MonoBehaviour
    {
        public int maxHitPoints;

        [Range(0, 360.0f)]
        public float hitAngle = 360.0f;

        public void ApplyDamage()
        {
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);

            Vector3 rotatedForward =
                Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                transform.up,
                rotatedForward, // from
                hitAngle, // angle
                1.0f
                );
        }

#endif
    }
}