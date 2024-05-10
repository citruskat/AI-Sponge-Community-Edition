using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraManager : MonoBehaviour
    {
        private CinemachineVirtualCamera virtualCamera;

        public void MoveCamera(Character character)
        {
            virtualCamera.Follow = character.Transform;
            virtualCamera.LookAt = character.Transform;
        }

        /// <summary>
        /// Set the camera tracking targets to a specified spawn point
        /// </summary>
        public void MoveCamera(Transform transform)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }

        public void Awake()
        {
            virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        }

        public void Start()
        {
            
        }
    }
}