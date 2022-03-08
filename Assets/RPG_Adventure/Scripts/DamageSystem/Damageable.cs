using UnityEngine;

namespace RPG_Adventure
{
    public class Damageable : MonoBehaviour
    {
        public int maxHitPoints;

        public void ApplyDamage()
        {
            Debug.Log("Applying damage");
        }
    }
}