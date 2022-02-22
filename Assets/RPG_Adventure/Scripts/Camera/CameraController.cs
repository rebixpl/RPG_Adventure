using Cinemachine;
using UnityEngine;

namespace RPG_Adventure
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineFreeLook freeLookCamera;

        private readonly float m_MaxCameraFOV = 40.0f;
        private readonly float m_MinCameraFOV = 10.0f;

        public CinemachineFreeLook PlayerCam
        {
            get
            {
                return freeLookCamera;
            }
        }

        private void Start()
        {
            // Set the default FOV
            freeLookCamera.m_Lens.FieldOfView = m_MaxCameraFOV;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                // The user is holding the right mouse button
                freeLookCamera.m_XAxis.m_MaxSpeed = 400;
                freeLookCamera.m_YAxis.m_MaxSpeed = 10;
            }

            if (Input.GetMouseButtonUp(1))
            {
                // The user has released the right mouse button
                freeLookCamera.m_XAxis.m_MaxSpeed = 0;
                freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            }

            ChangeFOVwithScroll();
        }

        private void ChangeFOVwithScroll()
        {
            float currentFOV = freeLookCamera.m_Lens.FieldOfView;

            // Player is zooming in
            if ((Input.GetAxis("Mouse ScrollWheel") > 0) &&
                (currentFOV > m_MinCameraFOV))
            {
                freeLookCamera.m_Lens.FieldOfView--;
            }

            // Player is zooming out
            if ((Input.GetAxis("Mouse ScrollWheel") < 0) &&
                (currentFOV < m_MaxCameraFOV))
            {
                freeLookCamera.m_Lens.FieldOfView++;
            }
        }
    }
}