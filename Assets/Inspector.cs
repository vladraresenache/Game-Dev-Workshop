using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : MonoBehaviour
{
    private Camera _camera;

    private Transform _inspectObjectTransform;

    public float deltaRotationX;
    public float deltaRotationY;

    public float rotateSpeed = 2;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit RayHit;
            if (CameraToMouseRay(Input.mousePosition, out RayHit))
            {
                if (RayHit.transform.gameObject.tag == "Inspectable")
                    _inspectObjectTransform = RayHit.transform;
            }
        }

        deltaRotationX = Input.GetAxis("Mouse X");
        deltaRotationX = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButtonDown(1) && _inspectObjectTransform)
        {
            _inspectObjectTransform.rotation =
                Quaternion.AngleAxis(deltaRotationX * rotateSpeed, transform.up) *
                Quaternion.AngleAxis(deltaRotationY * rotateSpeed, transform.right) *
                _inspectObjectTransform.rotation;
        }
    }
    private bool CameraToMouseRay(Vector2 mouseposition, out RaycastHit RayHit)
    {
        Ray ray = _camera.ScreenPointToRay(mouseposition);
        return Physics.Raycast(ray, out RayHit);
    }
}