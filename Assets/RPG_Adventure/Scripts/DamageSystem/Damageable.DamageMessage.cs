using UnityEngine;

namespace RPG_Adventure
{
    // Partial class is like writing one class, but it's separated into multiple files
    public partial class Damageable : MonoBehaviour
    {
        public struct DamageMessage
        {
            public MonoBehaviour damager;
            public int amount;
            public Vector3 damageSource;
        }
    }
}