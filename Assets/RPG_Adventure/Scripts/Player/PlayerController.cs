using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Adventure
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        public float rotationSpeed;


        private Vector3 m_Movement;
        private Camera m_MainCamera;
        private CharacterController m_CharController;

        private void Awake()
        {
            m_CharController = GetComponent<CharacterController>();
            m_MainCamera = Camera.main; // Gets a camera gameobject with "MainCamera" tag
        }

        void FixedUpdate()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            m_Movement.Set(horizontalInput, 0, verticalInput);

            Quaternion camRotation = m_MainCamera.transform.rotation;
            Vector3 targetDirection = camRotation * m_Movement;

            m_CharController.Move(
            targetDirection.normalized * speed * Time.fixedDeltaTime
            );
            m_CharController.transform.rotation = Quaternion.Euler(0, camRotation.eulerAngles.y, 0);


            //* ----------------------- better movement with rotation (i'm not deleting this code because it might be helpful to look at it in the future)
            //     Vector3 desiredForward = Vector3.RotateTowards(
            //        transform.forward,
            //        m_Movement,
            //        Time.fixedDeltaTime * rotationSpeed,
            //        0
            //    );

            // Quaternion rotation = Quaternion.LookRotation(desiredForward);
            // m_Rb.MoveRotation(rotation);

            // Vector3 dir = Vector3.zero;
            // dir.x = Input.GetAxis("Horizontal");
            // dir.z = Input.GetAxis("Vertical");

            // We are not moving
            // if (dir == Vector3.zero)
            // {
            //     return;
            // }

            // Vector3 targetDirection = cam.transform.rotation * dir;
            // targetDirection.y = 0;

            // if (dir.z >= 0)
            // {
            //     // The player is moving forward
            //     transform.rotation = Quaternion.Slerp(
            //         transform.rotation,
            //         Quaternion.LookRotation(targetDirection),
            //         rotationSpeed // Player rotation speed
            //     );
            // }

            // m_Rotation = Quaternion.LookRotation(desiredForward);

            // m_Rb.MovePosition(
            //     m_Rb.position + targetDirection.normalized * speed * Time.fixedDeltaTime
            // );
            // m_Rb.MoveRotation(m_Rotation);
        }
    }
}