using UnityEngine;
using UnityEngine.AI;

namespace RPG_Adventure
{
    public class BanditBehaviour : MonoBehaviour
    {
        public float detectionRadius = 10.0f;
        public float detectionAngle = 90.0f;

        private PlayerController m_Target;
        private NavMeshAgent m_NavMeshAgent;

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            m_Target = LookForPlayer();

            if (!m_Target) { return; }

            Vector3 targetPosition = m_Target.transform.position;
            Debug.Log(targetPosition);

            // Set the destination of AI to follow player
            m_NavMeshAgent.SetDestination(targetPosition);
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