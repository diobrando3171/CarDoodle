using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlByTouch : MonoBehaviour {
    public static bool IsTouching = false;
    //
    public Transform CameraPivort;
    public Transform MainCamera;
    //touch swipe 移动最小距离 小于此距离认为没有移动
    public float MinGesPosition = 5;
    //main camera z distance
    public float MainCameraDistance = 5.5f;
    //缩放 相机z轴最大值
    public float MainCameraDistanceMax = -2.5f;
    //缩放 相机z轴最小值
    public float MainCameraDistanceMin = -7f;
    //待绘制模型
    public GameObject goPaintModel;
    //
    public float Sensitivity = 0.04f;
    private float minAngle = 0;
    private float maxAngle = 80;
    private float xDegree;
    private float yDegree;
    private Quaternion desireRotation;
    private Quaternion rotation;
    private Vector3 position;
    //
    private float gestureX;
    private float gestureY;
    private float closeDistance = 0f;
    private Vector3 cameraOri;

    // Use this for initialization
    void Start () {
        //EasyTouch.On_SwipeStart += On_SwipeStart;
        EasyTouch.On_Swipe += On_Swipe;
        //
        EasyTouch.On_TouchStart += OnTouchStart;
        EasyTouch.On_TouchUp += OnTouchUp;
        //
        //
        cameraOri = MainCamera.transform.position;
        StartCoroutine("StartGetClose");
        //
        EasyTouch.On_TouchStart2Fingers += On_TouchStart2Fingers;
        EasyTouch.On_PinchIn += On_PinchIn;
        EasyTouch.On_PinchOut += On_PinchOut;
        EasyTouch.On_PinchEnd += On_PinchEnd;
        EasyTouch.On_Cancel2Fingers += On_Cancel2Fingers;
    }

    private void OnTouchStart(Gesture gesture){
        if(gesture.pickObject != null && gesture.pickObject.Equals(goPaintModel)){
            //touch on car
            IsTouching = false;
        }else{
            IsTouching = true;
        }
    }

    private void OnTouchUp(Gesture gesture){
        IsTouching = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private IEnumerator StartGetClose(){
        while(true){
            if (closeDistance >= 1f) {
                yield break;
            }
            closeDistance += 0.05f;
            MainCamera.position = new Vector3(cameraOri.x, cameraOri.y, cameraOri.z + closeDistance);
            yield return null;
        }
    }

    //void On_SwipeStart(Gesture gesture){
    //}

    void On_Swipe(Gesture gesture)
    {
        gestureX = gesture.deltaPosition.x;
        gestureY = gesture.deltaPosition.y;
        if(Mathf.Abs(gestureX) >= MinGesPosition) xDegree -= gestureX * Sensitivity*2;
        if(Mathf.Abs(gestureY) >= MinGesPosition) yDegree -= gestureY * Sensitivity;
        yDegree = ClampAngle(yDegree, minAngle, maxAngle);
        desireRotation = Quaternion.Euler(yDegree, xDegree, 0);
        rotation = Quaternion.Lerp(CameraPivort.rotation, desireRotation, 0.1f);
        CameraPivort.rotation = rotation;
        //
        //position = CameraPivort.position - rotation * Vector3.forward * 5.5f;
        //Vector3 lerpedPos = Vector3.Lerp(MainCamera.transform.position, position, 0.05f);
        //MainCamera.position = lerpedPos;

    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        else if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    //-------------------------
    // At the 2 fingers touch beginning
    private void On_TouchStart2Fingers(Gesture gesture)
    {
        EasyTouch.SetEnablePinch(true);
    }

    // At the pinch in
    private void On_PinchIn(Gesture gesture)
    {
        float zoom = Time.deltaTime * gesture.deltaPinch;
        float localZ = MainCamera.transform.localPosition.z - zoom;
        localZ = Mathf.Clamp(localZ, MainCameraDistanceMin, MainCameraDistanceMax);
        MainCamera.localPosition = new Vector3(0,0,localZ);
    }

    // At the pinch out
    private void On_PinchOut(Gesture gesture)
    {
        float zoom = Time.deltaTime * gesture.deltaPinch;
        float localZ = MainCamera.transform.localPosition.z + zoom;
        localZ = Mathf.Clamp(localZ, MainCameraDistanceMin, MainCameraDistanceMax);
        MainCamera.localPosition = new Vector3(0,0, localZ);
    }

    // At the pinch end
    private void On_PinchEnd(Gesture gesture)
    {

        if (gesture.pickObject == gameObject)
        {
            //transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            //EasyTouch.SetEnableTwist(true);
            //textMesh.text = "Pinch me";
        }

    }


    // If the two finger gesture is finished
    private void On_Cancel2Fingers(Gesture gesture)
    {
        /*
        transform.localScale =new Vector3(1.7f,1.7f,1.7f);
        EasyTouch.SetEnableTwist( true);
        textMesh.text="Pinch me";
        */
    }
}
