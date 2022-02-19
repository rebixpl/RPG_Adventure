using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Adventure
{

    public class PlayerInput : MonoBehaviour
    {
        private Vector3 m_Movement;

        public Vector3 MoveInput
        {
            get
            {
                return m_Movement;
            }
        }

        public bool IsMoveInput
        {
            get
            {
                return !Mathf.Approximately(MoveInput.magnitude, 0);
            }
        }

        void Update()
        {
            m_Movement.Set(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            );
        }
    }

}