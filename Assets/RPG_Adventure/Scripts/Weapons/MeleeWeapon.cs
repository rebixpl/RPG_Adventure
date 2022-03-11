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

        public LayerMask targetLayers;
        public int damage = 10;
        public AttackPoint[] attackPoints = new AttackPoint[0];

        private bool m_IsAttack = false;
        private Vector3[] m_OriginAttackPos;
        private RaycastHit[] m_RayCastHitCache = new RaycastHit[32];

        private void FixedUpdate()
        {
            if (m_IsAttack)
            {
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoint ap = attackPoints[i];
                    Vector3 worldPos = ap.rootTransform.position
                        + ap.rootTransform.TransformDirection(ap.offset);

                    Vector3 attackVector = (worldPos - m_OriginAttackPos[i]).normalized;

                    // Draw a ray
                    Ray ray = new Ray(worldPos, attackVector);
                    Debug.DrawRay(worldPos, attackVector, Color.red, 4.0f);

                    int contacts = Physics.SphereCastNonAlloc(
                        ray,
                        ap.radius,
                        m_RayCastHitCache,
                        attackVector.magnitude,
                        ~0, // we would like to collide with all of the layers "negation 0" "~0" ~ means we are flipping every binary number of int32 
                        QueryTriggerInteraction.Ignore
                        );

                    for (int k = 0; k < contacts; k++)
                    {
                        Collider collider = m_RayCastHitCache[k].collider;

                        if (collider != null)
                        {
                            CheckDamage(collider, ap);
                        }
                    }

                    m_OriginAttackPos[0] = worldPos;
                }
            }
        }

        private void CheckDamage(Collider other, AttackPoint ap)
        {
            if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            {
                // We are not hitting the correct layer, return
                return;
            }

            // Proceed with checking damage, we are hitting correct layer

            Damageable damageable = other.GetComponent<Damageable>();

            if (damageable != null)
            {
                // The object sword has collided with, has Damageable script, so we can cause damage to it
                damageable.ApplyDamage();
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

        public void EndAttack()
        {
            m_IsAttack = false;
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