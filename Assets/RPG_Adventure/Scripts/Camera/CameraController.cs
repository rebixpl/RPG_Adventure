using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RPG_Adventure
{
    public class CameraController : MonoBehaviour
    {
        public CinemachineFreeLook freeLookCamera;

        // Update is called once per frame
        void Update()
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