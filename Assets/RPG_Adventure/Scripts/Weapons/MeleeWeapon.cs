using UnityEngine;

namespace RPG_Adventure
{
    public class MeleeWeapon : MonoBehaviour
    {
        public int damage = 10;

        public void BeginAttack()
        {
            Debug.Log("Weapon is attacing");
        }
    }
}