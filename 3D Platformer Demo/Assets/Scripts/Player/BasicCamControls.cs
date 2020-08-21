using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamControls : MonoBehaviour
{
    [SerializeField] Transform subject = null;
    [SerializeField] float mouseSensitivity = 0;
    [HideInInspector] public float angle = Mathf.PI * 3f/2f;
    float offsetDistance;
    float lookAtHeight = 0;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 initialOffset = transform.position - subject.transform.position;
        Vector3 initialOffsetXZ = initialOffset - new Vector3(0, initialOffset.y, 0);
        Vector3 subjectXZ = subject.transform.position - new Vector3(0, subject.transform.position.y, 0);
        offsetDistance = Vector3.Distance(initialOffsetXZ, subjectXZ);
        lookAtHeight = subject.transform.position.y;
    }

    void ModifyAngle()
    {
        angle -= Input.GetAxis("Mouse X") * mouseSensitivity;
    }

    Vector3 GetOffset()
    {
        float x = Mathf.Cos(angle) * offsetDistance;
        float z = Mathf.Sin(angle) * offsetDistance;
        Vector3 offset = new Vector3(x, 3, z);
        return offset;
    }

    void Update()
    {
        ModifyAngle();
    }

    void LateUpdate()
    {
        Vector3 offset = GetOffset();
        transform.position = subject.transform.position + offset;
        Vector3 subjectXZ = subject.transform.position - new Vector3(0, subject.transform.position.y, 0);
        transform.LookAt(subjectXZ + new Vector3(0, subject.transform.position.y, 0));
    }
}
