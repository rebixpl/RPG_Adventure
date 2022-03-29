using System.Collections.Generic;
using UnityEngine;

namespace RPG_Adventure
{
    public partial class Damageable : MonoBehaviour
    {
        [Range(0, 360.0f)]
        public float hitAngle = 360.0f;

        public int maxHitPoints;
        public int CurrentHitPoints { get; private set; } // set is private so we can only change the hp amount in this class "Damageable"
        public List<MonoBehaviour> onDamageMessageReceivers;

        private void Awake()
        {
            CurrentHitPoints = maxHitPoints;
        }

        public void ApplyDamage(DamageMessage data)
        {
            if (CurrentHitPoints <= 0)
            {
                // We don't have more hit points, so there is no reason to apply more damage
                return;
            }

            Vector3 positionToDamager = data.damageSource - transform.position;
            positionToDamager.y = 0;

            if (Vector3.Angle(transform.forward, positionToDamager) > hitAngle * 0.5)
            {
                // Not Hitting
                return;
            }
            // Hitting
            CurrentHitPoints -= data.amount;

            var messageType = CurrentHitPoints <= 0 ? MessageType.DEAD : MessageType.DAMAGED;

            for (int i = 0; i < onDamageMessageReceivers.Count; i++) 
            { 
                var receiver = onDamageMessageReceivers[i] as IMessageReceiver;
                //Debug.Log(messageType);
                //Debug.Log(receiver);
                receiver.OnReceiveMessage(messageType);
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);

            Vector3 rotatedForward =
                Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                transform.up,
                rotatedForward, // from
                hitAngle, // angle
                1.0f
                );
        }

#endif
    }
}