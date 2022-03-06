using UnityEngine;
using UnityEngine.AI;

namespace RPG_Adventure
{
    public class EnemyController : MonoBehaviour
    {
        private NavMeshAgent m_NavMeshAgent;
        private Animator m_Animator;
        private float m_SpeedModifier = 0.9f;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnAnimatorMove()
        {
            if (m_NavMeshAgent.enabled)
            {
                m_NavMeshAgent.speed = (m_Animator.deltaPosition / Time.fixedDeltaTime).magnitude
                   * m_SpeedModifier;
            }
        }

        public bool FollowTarget(Vector3 position)
        {
            if (!m_NavMeshAgent.enabled) { m_NavMeshAgent.enabled = true; }

            // Follow the player (his position)
            return m_NavMeshAgent.SetDestination(position);
        }

        public void StopFollowTarget()
        {
            // Stop following the player
            m_NavMeshAgent.enabled = false;
        }
    }
}