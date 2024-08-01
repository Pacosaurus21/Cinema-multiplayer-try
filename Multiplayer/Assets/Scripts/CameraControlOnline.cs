using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlOnline : MonoBehaviour
{

    public Transform target;

    private float finalRot, currentRot, finalHeight, currentHeight;
    public float speedRot, speedHeight, distance, height;

    public void SetTransform(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {

        if (!target) return;

        currentRot = transform.eulerAngles.y;
        finalRot = target.eulerAngles.y;

        currentHeight = transform.position.y;
        finalHeight = target.position.y + height;

        currentRot = Mathf.LerpAngle(currentRot, finalRot, speedRot * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, finalHeight, speedHeight * Time.deltaTime);

        Quaternion tempRot = Quaternion.Euler(0, currentRot, 0);
        transform.position = target.position;
        transform.position -= tempRot * Vector3.forward * distance;

        Vector3 tempPos = transform.position;
        tempPos.y = currentHeight;
        transform.position = tempPos;

        transform.LookAt(target);



    }
}
