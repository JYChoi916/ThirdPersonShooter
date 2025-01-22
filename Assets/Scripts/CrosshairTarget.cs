using Cinemachine;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{
    Camera mainCamera;
    Ray ray;
    RaycastHit hitInfo;

    public Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorToTarget = playerTransform.position - mainCamera.transform.position;
        Vector3 proj = Vector3.Project(vectorToTarget, mainCamera.transform.forward);
        ray.origin = mainCamera.transform.position + mainCamera.transform.forward * proj.magnitude;
        ray.direction = mainCamera.transform.forward;
        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, Time.deltaTime);
        }
        else
        {
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * 20f;
        }
    }
}
