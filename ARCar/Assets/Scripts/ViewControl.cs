using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewControl : MonoBehaviour {
    public Transform CameraPivort;
    public Transform MainCamera;
    //
    private float Sensitivity = 1.25f;
    private float minAngle = 0;
    private float maxAngle = 80;
    private float xDegree;
    private float yDegree;
    private Quaternion desireRotation;
    private Quaternion rotation;
    private Vector3 position;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CameraControl();
	}

    private void CameraControl(){
        xDegree -= Input.GetAxis("Horizontal") * Sensitivity; 
        yDegree += Input.GetAxis("Vertical") * Sensitivity; 
        yDegree = ClampAngle(yDegree, minAngle, maxAngle);
        desireRotation = Quaternion.Euler(yDegree,xDegree,0);
        rotation = Quaternion.Lerp(CameraPivort.rotation, desireRotation, 0.1f);
        CameraPivort.rotation = rotation;
        //
        position = CameraPivort.position - rotation * Vector3.forward * 5.5f;
        Vector3 lerpedPos = Vector3.Lerp(MainCamera.transform.position, position, 0.05f);
        MainCamera.position = lerpedPos;
    }

    private float ClampAngle(float angle,float min,float max){
        if (angle < -360) angle += 360;
        else if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
