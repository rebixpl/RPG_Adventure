using System.Collections;
using UnityEngine;

namespace RPG_Adventure
{
    public class BanditBehaviour : MonoBehaviour, IMessageReceiver
    {
        public PlayerScanner playerScanner;
        public float timeToStopPursuit = 2.0f;
        public float timeToWaitOnPursuit = 2.0f;
        public float attackDistance = 1.1f;

        public bool HasFollowTarget
        {
            get
            {
                return m_FollowTarget != null;
            }
        }

        private PlayerController m_FollowTarget;
        private EnemyController m_EnemyController;
        private Vector3 m_OriginPosition;
        private Quaternion m_OriginRotation;

        private float m_TimeSinceLostTarget = 0.0f;

        private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
        private readonly int m_HashNearBase = Animator.StringToHash("NearBase");
        private readonly int m_HashAttack = Animator.StringToHash("Attack");
        private readonly int m_HashHurt = Animator.StringToHash("Hurt");

        private void Awake()
        {
            m_EnemyController = GetComponent<EnemyController>();
            m_OriginPosition = transform.position;
            m_OriginRotation = transform.rotation;
        }

        private void Update()
        {
            GuardPosition();
        }

        private void GuardPosition()
        {
            var detectedTarget = playerScanner.Detect(transform);
            bool hasDetectedTarget = detectedTarget != null;

            if (hasDetectedTarget) { m_FollowTarget = detectedTarget; }

            if (HasFollowTarget)
            {
                AttackOrFollowTarget();

                if (hasDetectedTarget)
                {
                    m_TimeSinceLostTarget = 0;
                }
                else
                {
                    StopPursuit();
                }
            }

            CheckIfNecarBase();
        }

        // Receives message from damageable after it detected that enemy was hit
        public void OnReceiveMessage(MessageType type)
        {
            switch (type)
            {
                case MessageType.DEAD:
                    Debug.Log("Should play animation dead");
                    break;

                case MessageType.DAMAGED:
                    OnReceiveDamage();
                    break;

                default:
                    break;
            }
        }

        private void OnReceiveDamage()
        {
            m_EnemyController.Animator.SetTrigger(m_HashHurt);
        }

        private void CheckIfNecarBase()
        {
            Vector3 toBase = m_OriginPosition - transform.position;
            toBase.y = 0;

            bool nearBase = toBase.magnitude < 0.01f;

            // Inform animator, that the enemy is near it's base
            m_EnemyController.Animator.SetBool(m_HashNearBase, nearBase);

            if (nearBase)
            {
                // If enemy is near it's original location, rotate to original rotation
                Quaternion targetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    m_OriginRotation,
                    360 * Time.deltaTime);

                transform.rotation = targetRotation;
            }
        }

        private void StopPursuit()
        {
            // Player is not inside enemy range, count the time
            m_TimeSinceLostTarget += Time.deltaTime;

            // If player is not inside enemy range for specified time,
            // set target to null and thus stop following the target(player)
            if (m_TimeSinceLostTarget >= timeToStopPursuit)
            {
                m_FollowTarget = null;
                m_EnemyController.Animator.SetBool(m_HashInPursuit, false);
                StartCoroutine(WaitBeforeReturn());
            }
        }

        private void AttackOrFollowTarget()
        {
            // Check the distance from enemy to player
            Vector3 toTarget = m_FollowTarget.transform.position - transform.position;

            if (toTarget.magnitude <= attackDistance)
            {
                // ATTACKING THE PLAYER
                AttackTarget(toTarget: toTarget);
            }
            else
            {
                // Keep following player
                FollowTarget();
            }
        }

        private void AttackTarget(Vector3 toTarget)
        {
            var toTargetRotation = Quaternion.LookRotation(toTarget);
            // always rotate towards the player
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toTargetRotation,
                360.0f * Time.deltaTime
            );

            m_EnemyController.StopFollowTarget();
            // Attack if distance is close enough
            m_EnemyController.Animator.SetTrigger(m_HashAttack);
        }

        private void FollowTarget()
        {
            m_EnemyController.Animator.SetBool(m_HashInPursuit, true);
            m_EnemyController.FollowTarget(m_FollowTarget.transform.position);
        }

        private IEnumerator WaitBeforeReturn()
        {
            // After timeToWaitOnPursuit seconds, the enemy should be able to move again
            yield return new WaitForSeconds(timeToWaitOnPursuit);

            // Move to the origin position
            m_EnemyController.FollowTarget(m_OriginPosition);
        }

        // This if UNITY_EDITOR means, that the method inside will be only a part of the editor,
        // When you compile the production game, this code will not be shipped in the final
        // product.
#if UNITY_EDITOR

        // This method is executed not only on a runtime, but also in a editor
        // it runs only when you select the object in hierarchy
        private void OnDrawGizmosSelected()
        {
            // Drawing representation of enemy circle detection range
            Color c = new Color(0.8f, 0, 0, 0.4f);
            Color m = new Color(0.0f, 0.8f, 0.9f, 0.4f);
            UnityEditor.Handles.color = c;

            Vector3 rotatedForward = Quaternion.Euler(
                0,
                -playerScanner.detectionAngle * 0.5f,
                0
            ) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                playerScanner.detectionAngle,
                playerScanner.detectionRadius
            );

            UnityEditor.Handles.color = m;

            UnityEditor.Handles.DrawSolidArc(
               transform.position,
               Vector3.up,
               rotatedForward,
               360,
               playerScanner.meleeDetectionRadius
           );
        }

#endif
    }
}