using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlOnline : MonoBehaviour
{

    public Transform target;
    public float speedRot;
    public float rotationX = 0;
    public float rotationY = 0;

    public void SetTransform(Transform _target)
    {
        target = _target;
    }
    void Start()
    {
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!target) return;
        transform.position = target.position;
        transform.LookAt(target);
        rotationX -= Input.GetAxis("Mouse Y") * speedRot * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse X") * speedRot * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);

    }
}
