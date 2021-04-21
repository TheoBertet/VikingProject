using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCameraManager : MonoBehaviour
{
    public float minDistance = 1;
    public float maxDistance = 5;
    public float zoomSpeed = 2;

    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = transform.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        Debug.Log(Input.GetAxis("Mouse ScrollWheel"));

        if(Input.GetAxis("Mouse ScrollWheel") >= 0 && virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance >= minDistance
            || Input.GetAxis("Mouse ScrollWheel") <= 0 && virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance <= maxDistance)
        {
            virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

            if(virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance > maxDistance)
            {
                virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = maxDistance;
            }

            if(virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance < minDistance)
            {
                virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = minDistance;
            }
        }
    }
}
