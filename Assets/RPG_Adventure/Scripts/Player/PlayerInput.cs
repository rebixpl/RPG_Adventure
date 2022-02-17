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