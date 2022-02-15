using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Adventure
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        public float rotationSpeed = 0.05f;
        public Camera cam;

        private Rigidbody m_Rb;
        // private Vector3 m_Movement;
        // private Quaternion m_Rotation;

        private void Start()
        {
            m_Rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            // float horizontalInput = Input.GetAxis("Horizontal");
            // float verticalInput = Input.GetAxis("Vertical");

            Vector3 dir = Vector3.zero;
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");

            // We are not moving
            if (dir == Vector3.zero)
            {
                return;
            }

            Vector3 targetDirection = cam.transform.rotation * dir;
            targetDirection.y = 0;

            if (dir.z >= 0)
            {
                // The player is moving forward
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(targetDirection),
                    rotationSpeed // Player rotation speed
                );
            }

            // m_Movement = new Vector3(horizontalInput, 0, verticalInput);
            // m_Movement.Normalize();

            // Vector3 desiredForward = Vector3.RotateTowards(
            //     transform.forward,
            //     m_Movement,
            //     Time.fixedDeltaTime * rotationSpeed,
            //     0
            // );

            // m_Rotation = Quaternion.LookRotation(desiredForward);

            m_Rb.MovePosition(
                m_Rb.position + targetDirection.normalized * speed
                * Time.fixedDeltaTime
            );
            // m_Rb.MoveRotation(m_Rotation);
        }
    }
}