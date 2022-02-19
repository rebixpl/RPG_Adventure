using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Adventure
{
    public class PlayerController : MonoBehaviour
    {
        const float k_Acceleration = 20.0f;
        const float k_Deceleration = 35.0f;


        public float maxForwardSpeed = 8.0f;
        public float rotationSpeed;


        private PlayerInput m_PlayerInput;
        private CharacterController m_CharController;
        private Animator m_Animator;
        private Camera m_MainCamera;

        private float m_DesiredForwardSpeed;
        private float m_ForwardSpeed;

        private readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");

        private void Awake()
        {
            m_CharController = GetComponent<CharacterController>();
            m_MainCamera = Camera.main; // Gets a camera gameobject with "MainCamera" tag
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
        }
        private void FixedUpdate()
        {
            ComputeMovement();
        }

        private void ComputeMovement()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;

            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            float acceleration = m_PlayerInput.IsMoveInput ? k_Acceleration : k_Deceleration;

            m_ForwardSpeed = Mathf.MoveTowards(
                m_ForwardSpeed,
                m_DesiredForwardSpeed,
                Time.fixedDeltaTime * acceleration
            );

            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }


        // void FixedUpdate()
        // {
        //     Vector3 moveInput = m_PlayerInput.MoveInput;
        //     Quaternion camRotation = m_MainCamera.transform.rotation;
        //     Vector3 targetDirection = camRotation * moveInput;

        //     // Animation Root Movement is moving the player instead of this code
        //     // m_CharController.Move(
        //     // targetDirection.normalized * speed * Time.fixedDeltaTime
        //     // );
        //     m_CharController.transform.rotation = Quaternion.Euler(0, camRotation.eulerAngles.y, 0);

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
        // }
    }
}