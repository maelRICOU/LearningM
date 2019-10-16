using UnityEngine;

public class CarController : MonoBehaviour
{

    public WheelCollider frontDriverW;
    public WheelCollider backDriverW;
    public WheelCollider frontPassengerW;
    public WheelCollider backPassengerW;

    public Transform frontDriverT;
    public Transform backDriverT;
    public Transform frontPassengerT;
    public Transform backPassengerT;

    public float horizontalInput;
    public float verticalInput;
    public float streeringAngle;


    public float maxSteerAngle = 42;
    public float motorForce = 800;




    private void FixedUpdate()
    {

        Steer();
        Accelerate();
        UpdateWheelPos();

    }

    void Steer()
    {
        streeringAngle = horizontalInput * maxSteerAngle;
        frontDriverW.steerAngle = streeringAngle;
        frontPassengerW.steerAngle = streeringAngle;
    }

    void Accelerate()
    {
        backDriverW.motorTorque = verticalInput * motorForce;
        backPassengerW.motorTorque = verticalInput * motorForce;
    }

    void UpdateWheelPos()
    {
        UpdateThisWheel(frontDriverW, frontDriverT);
        UpdateThisWheel(backDriverW, backDriverT);
        UpdateThisWheel(frontPassengerW, frontPassengerT);
        UpdateThisWheel(frontPassengerW, frontPassengerT);
    }

    Vector3 pos;
    Quaternion quat;

    void UpdateThisWheel(WheelCollider col, Transform tr)
    {
        pos = tr.position;
        quat = tr.rotation;

        col.GetWorldPose(out pos, out quat);

        tr.position = pos;
        tr.rotation = quat;
    }


}

