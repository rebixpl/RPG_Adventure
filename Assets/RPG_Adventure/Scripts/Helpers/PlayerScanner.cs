using UnityEngine;

namespace RPG_Adventure

{
    // We need to mark this class as [System.Serializable], so it could be showed in the editor
    [System.Serializable]
    public class PlayerScanner
    {
        public float meleeDetectionRadius = 2.0f;
        public float detectionRadius = 10.0f;
        public float detectionAngle = 90.0f;

        public PlayerController Detect(Transform detector)
        {
            // Check if player does exist in the scene
            if (PlayerController.Instance == null)
            {
                return null;
            }

            // toPlayer is a distance from enemy to a player
            Vector3 toPlayer = PlayerController.Instance.transform.position - detector.position;
            toPlayer.y = 0; 

            if (toPlayer.magnitude <= detectionRadius)
            {
                // Player detected in the specified range
                if ((Vector3.Dot(toPlayer.normalized, detector.forward) >
                    Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad)) ||
                    (toPlayer.magnitude <= meleeDetectionRadius))
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
    }
}