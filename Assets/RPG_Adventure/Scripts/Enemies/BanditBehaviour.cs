using UnityEngine;

namespace RPG_Adventure
{
    public class BanditBehaviour : MonoBehaviour
    {
        public float detectionRadius = 10.0f;
        public float detectionAngle = 90.0f;

        private void Update()
        {
            LookForPlayer();
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
                Debug.Log("Detecting the Player");
            }
            else
            {
                Debug.Log("Player not detected");
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