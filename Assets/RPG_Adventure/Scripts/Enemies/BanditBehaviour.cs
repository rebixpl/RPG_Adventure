using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG_Adventure
{
    public class BanditBehaviour : MonoBehaviour
    {
        public float detectionRadius = 10.0f;
        public float detectionAngle = 90.0f;
        public float timeToStopPursuit = 2.0f;
        public float timeToWaitOnPursuit = 2.0f;

        private PlayerController m_Target;
        private NavMeshAgent m_NavMeshAgent;
        private float m_TimeSinceLostTarget = 0.0f;
        private Vector3 m_OriginPosition;
        private Animator m_Animator;

        private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
        private readonly int m_HashNearBase = Animator.StringToHash("NearBase");

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
            m_OriginPosition = transform.position;
        }

        private void Update()
        {
            var target = LookForPlayer();

            if (m_Target == null)
            {
                if (target != null)
                {
                    m_Target = target;
                }
            }
            else
            {
                // Set the destination of AI to follow player
                m_NavMeshAgent.SetDestination(m_Target.transform.position);
                m_Animator.SetBool(m_HashInPursuit, true);

                if (target == null)
                {
                    // Player is not inside enemy range, count the time
                    m_TimeSinceLostTarget += Time.deltaTime;

                    // If player is not inside enemy range for specified time,
                    // set target to null and thus stop following the target(player)
                    if (m_TimeSinceLostTarget >= timeToStopPursuit)
                    {
                        m_Target = null;
                        m_NavMeshAgent.isStopped = true;
                        m_Animator.SetBool(m_HashInPursuit, false);
                        StartCoroutine(WaitOnPursuit());
                    }
                }
                else
                {
                    m_TimeSinceLostTarget = 0;
                }
            }

            Vector3 toBase = m_OriginPosition - transform.position;
            toBase.y = 0;

            // Inform animator, that the enemy is near it's base
            m_Animator.SetBool(m_HashNearBase, toBase.magnitude < 0.01f);
        }

        private IEnumerator WaitOnPursuit()
        {
            // After timeToWaitOnPursuit seconds, the enemy should be able to move again
            yield return new WaitForSeconds(timeToWaitOnPursuit);
            m_NavMeshAgent.isStopped = false;

            // Move to the origin position
            m_NavMeshAgent.SetDestination(m_OriginPosition);
        }

        private PlayerController LookForPlayer()
        {
            // Check if player does exist in the scene
            if (PlayerController.Instance == null)
            {
                return null;
            }

            Vector3 enemyPosition = transform.position;

            // toPlayer is a distance from enemy to a player
            Vector3 toPlayer = PlayerController.Instance.transform.position - enemyPosition;
            toPlayer.y = 0;

            if (toPlayer.magnitude <= detectionRadius)
            {
                // Player detected in the specified range
                if (Vector3.Dot(toPlayer.normalized, transform.forward) >
                    Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
                {
                    // return player instance
                    return PlayerController.Instance;
                }
            }
            else
            {
                // Player is not in specified range
            }

            return null;
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
            UnityEditor.Handles.color = c;

            Vector3 rotatedForward = Quaternion.Euler(
                0,
                -detectionAngle * 0.5f,
                0
            ) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                detectionAngle,
                detectionRadius
            );
        }

#endif
    }
}