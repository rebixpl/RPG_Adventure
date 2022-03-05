using UnityEngine;

namespace RPG_Adventure
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance
        {
            get
            {
                return s_Instance;
            }
        }

        public float maxForwardSpeed = 8.0f;
        public float rotationSpeed;
        public float m_MaxRotationSpeed = 1200;
        public float m_MinRotationSpeed = 700;
        public float gravity = 9.8f;
        public MeleeWeapon meleeWeapon;

        private static PlayerController s_Instance;
        private PlayerInput m_PlayerInput;
        private CharacterController m_CharController;
        private Animator m_Animator;
        private CameraController m_CameraController;
        private Quaternion m_TargetRotation;

        private float m_DesiredForwardSpeed;
        private float m_ForwardSpeed;
        private float m_VerticalSpeed;

        private readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");

        private const float k_Acceleration = 20.0f;
        private const float k_Deceleration = 35.0f;

        private void Awake()
        {
            m_CharController = GetComponent<CharacterController>();
            //m_MainCamera = Camera.main; // Gets a camera gameobject with "MainCamera" tag
            m_CameraController = Camera.main.GetComponent<CameraController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();

            s_Instance = this;
        }

        private void FixedUpdate()
        {
            ComputeForwardMovement();
            ComputeVerticalMovement();
            ComputeRotation();

            if (m_PlayerInput.IsMoveInput)
            {
                float rotationSpeed = Mathf.Lerp(
                    m_MaxRotationSpeed,
                    m_MinRotationSpeed,
                    m_ForwardSpeed / m_DesiredForwardSpeed
                );

                m_TargetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    m_TargetRotation,
                    Time.fixedDeltaTime * rotationSpeed
                );

                transform.rotation = m_TargetRotation;
            }

            m_Animator.ResetTrigger(m_HashMeleeAttack);
            if (m_PlayerInput.IsAttack)
            {
                m_Animator.SetTrigger(m_HashMeleeAttack);
                meleeWeapon.BeginAttack();
            }
        }

        private void OnAnimatorMove()
        {
            // Apply Root Motion handled by script
            Vector3 movement = m_Animator.deltaPosition;
            movement += m_VerticalSpeed * Vector3.up * Time.fixedDeltaTime;
            m_CharController.Move(movement);
        }

        private void ComputeVerticalMovement()
        {
            m_VerticalSpeed = -gravity;
        }

        private void ComputeForwardMovement()
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

        private void ComputeRotation()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;

            Vector3 cameraDirection = Quaternion.Euler(
                0,
                m_CameraController.PlayerCam.m_XAxis.Value,
                0
            ) * Vector3.forward;

            Quaternion targetRotation;

            if (Mathf.Approximately(Vector3.Dot(moveInput, Vector3.forward), -1.0f))
            {
                // Player is walking backwards
                targetRotation = Quaternion.LookRotation(-cameraDirection);
            }
            else
            {
                // Player is walking normally
                Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
                targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);
            }

            m_TargetRotation = targetRotation;
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