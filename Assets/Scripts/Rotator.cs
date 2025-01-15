using System;
using UnityEngine;
using Unity.Mathematics;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed = 10f;
    public bool3 rotateAxis;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 eulerAngle = new Vector3(rotateAxis.x ? 1 : 0, rotateAxis.y ? 1 : 0, rotateAxis.z ? 1 : 0);
        transform.Rotate(eulerAngle * Time.deltaTime * rotateSpeed);
    }
}
