using UnityEngine;

namespace RPG_Adventure
{
    public class FixedUpdateFollow : MonoBehaviour
    {
        public Transform toFollow;

        private void FixedUpdate()
        {
            transform.position = toFollow.position;
            transform.rotation = toFollow.rotation;
        }
    }
}