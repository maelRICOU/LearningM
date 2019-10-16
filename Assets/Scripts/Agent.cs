using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Agent : MonoBehaviour, IComparable<Agent>
{

    public NeuralNetwork net;
    public CarController carController;

    public float fitness;
    public float rayRange = 1;
    public LayerMask layerMask;

    float[] inputs;

    public Transform nextCheckpoint;
    public float nextCheckpointList;
    public float distanceTraveled = 0;
    public float lastDisCheckpoint;

    public Rigidbody rb;

    float angleZ;



    public Material FirstMat;
    public Material mutateMat;
    public Material defaultMat;

    public Renderer render;
    public Renderer postRenderer;




    public void Start()
    {
        Init();
    }

    public void ResetAgent()
    {
        fitness = 0;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        distanceTraveled = 0;


        Init();
    }

    public void Init()
    {
   //     Debug.Log(net.layers.Length);
    //    Debug.Log(net.layers[0]);

        inputs = new float[net.layers[0]];

        nextCheckpoint = CheckpointManager.instance.firstCheckpoint;
        nextCheckpointList = (transform.position - nextCheckpoint.position).magnitude;

    }

    public void CheckpointReached(Transform _nextcheckpoint)
    {
        distanceTraveled += nextCheckpointList;
        nextCheckpoint = _nextcheckpoint;
        nextCheckpointList = (transform.position - nextCheckpoint.position).magnitude;
        lastDisCheckpoint = DisCheckPoint();

    }

    float DisCheckPoint()
    {
        return ClosestDisToPointOnLine(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position);
    }
    public void FixedUpdate()
    {
        OuputUpdate();
        UpdateFitness();

        InputUpdate();
        lastDisCheckpoint = disCheckpoint;
    }


    float disCheckpoint;
    Vector3 pos;
    void InputUpdate()
    {
        pos = transform.position;

        inputs[0] = RaySensor(pos + Vector3.up * 0.2f, transform.forward, 4);
        inputs[1] = RaySensor(pos + Vector3.up * 0.2f, transform.right, 1.5f);
        inputs[2] = RaySensor(pos + Vector3.up * 0.2f, -transform.right, 1.5f);
        inputs[3] = RaySensor(pos + Vector3.up * 0.2f, transform.right + transform.forward, 2f);
        inputs[4] = RaySensor(pos + Vector3.up * 0.2f, -transform.right + transform.forward, 2f);

        inputs[5] = 1 - (float)Math.Tanh(rb.velocity.magnitude / 20);
        disCheckpoint = DisCheckPoint();
        inputs[6] = (float)Math.Tanh(lastDisCheckpoint - disCheckpoint) * 5;
        inputs[7] = (float)Math.Tanh(rb.angularVelocity.z * 0.1f);

        inputs[8] = (float)Math.Tanh(transform.InverseTransformDirection(ClosestPointOnLine(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position)).x);
        inputs[9] = (float)Math.Tanh(transform.InverseTransformDirection(ClosestPointOnLine(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position)).z);
        inputs[10] = (float)Math.Tanh(rb.angularVelocity.y * 0.1f);

        angleZ = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;

        inputs[11] = (float)Math.Tanh(angleZ / 90);

    }

    RaycastHit hit;
    float RaySensor(Vector3 pos, Vector3 direction, float length)
    {
        if(Physics.Raycast(pos, direction, out hit, length, layerMask))
        {
            Debug.DrawRay(pos, direction * hit.distance, Color.green);

            return (rayRange * length - hit.distance) / (rayRange * length);
        }

        Debug.DrawRay (pos, direction * length *rayRange, Color.red);
        return 0;
    }

    public void OuputUpdate()
    {
        net.FeedForward(inputs);

        carController.horizontalInput = net.neurons[net.layers.Length - 1][0];
        carController.verticalInput = net.neurons[net.layers.Length - 1][1];
    }

     void UpdateFitness()
    {
        SetFitness(distanceTraveled + (nextCheckpointList -(transform.position - nextCheckpoint.position).magnitude));
    }

    void SetFitness(float _newFitness)
    {
        if(fitness < _newFitness)
        {
            fitness = _newFitness;
        }
    }

    float ClosestDisToPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        return (vA + (vB - vA).normalized * Vector3.Dot((vB - vA).normalized, vPoint - vA) -vPoint).magnitude;
    }

    Vector3 v;
    float d;
    float t;
    Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        v = (vB - vA).normalized;

        d = Vector3.Distance(vA, vB);
        t = Vector3.Dot(v, vPoint - vA);

        if (t <= 0)
            return vA;

        if (t >= d)
            return vB;

        return vA = v * t;
    }

    public int CompareTo(Agent other)
    {
        if(fitness < other.fitness)
        {
            return 1;
        }
        if(fitness > other.fitness)
        {
            return -1;
        }

        return 0;
    }


    public void SetDefaultColor()
    {
        render.material = defaultMat;
        postRenderer.material = defaultMat;
    }

    public void SetMutatedColor()
    {
        render.material = mutateMat;
        postRenderer.material = mutateMat;
    }

    public void SetFirstColor()
    {
        render.material = FirstMat;
        postRenderer.material = FirstMat;
    }

}
