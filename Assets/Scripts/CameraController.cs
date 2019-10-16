using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    public Transform target;

    public Vector3 decalLook;
    public Vector3 decalPos;

    public float posLerpSpeed = 0.02f;
    public float lookLerpSpeed = 0.1f;


    Vector3 wantedPos;



    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            wantedPos = target.TransformPoint(decalPos);

            wantedPos.y = decalPos.y;

            transform.position = Vector3.Lerp(transform.position, wantedPos, posLerpSpeed);


            Quaternion wantedLookk = Quaternion.LookRotation(target.TransformPoint(decalLook) - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, wantedLookk, lookLerpSpeed);

        }
    }
}
