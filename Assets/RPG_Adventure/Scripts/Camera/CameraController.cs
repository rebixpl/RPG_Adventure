using Cinemachine;
using UnityEngine;

namespace RPG_Adventure
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineFreeLook freeLookCamera;

        public CinemachineFreeLook PlayerCam
        {
            get
            {
                return freeLookCamera;
            }
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
        }
    }
}