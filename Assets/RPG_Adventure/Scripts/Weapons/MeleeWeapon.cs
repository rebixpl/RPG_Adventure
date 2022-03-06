using UnityEngine;

namespace RPG_Adventure
{
    public class MeleeWeapon : MonoBehaviour
    {
        [System.Serializable]
        public class AttackPoint
        {
            public float radius = 0.1f;
            public Vector3 offset;
            public Transform rootTransform;
        }

        public int damage = 10;
        public AttackPoint[] attackPoints = new AttackPoint[0];

        private bool m_IsAttack = false;
        private Vector3[] m_OriginAttackPos;

        private void FixedUpdate()
        {
            if (m_IsAttack)
            {
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoint ap = attackPoints[i];
                    Vector3 worldPos = ap.rootTransform.position
                        + ap.rootTransform.TransformDirection(ap.offset);

                    Vector3 attackVector = worldPos - m_OriginAttackPos[i];

                    // Draw a ray
                    Ray r = new Ray(worldPos, attackVector);
                    Debug.DrawRay(worldPos, attackVector, Color.red, 4.0f);
                }
            }
        }

        public void BeginAttack()
        {
            m_IsAttack = true;
            m_OriginAttackPos = new Vector3[attackPoints.Length];

            // Save original positions of attack points before sword swings
            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint ap = attackPoints[i];
                m_OriginAttackPos[i] = ap.rootTransform.position +
                    ap.rootTransform.TransformDirection(ap.offset);
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Color gizmosGrayColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);
            Gizmos.color = gizmosGrayColor;

            // Draw attack points on the sword in the editor when sword is selected
            foreach (AttackPoint attackPoint in attackPoints)
            {
                if (attackPoint.rootTransform != null)
                {
                    Vector3 worldPosition = attackPoint.rootTransform.TransformVector(attackPoint.offset);
                    Gizmos.DrawSphere(attackPoint.rootTransform.position + worldPosition,
                        attackPoint.radius);
                }
            }
        }

#endif
    }
}