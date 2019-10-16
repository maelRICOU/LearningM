using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform nextCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Agent>() != null)
        {
            if(other.GetComponent<Agent>().nextCheckpoint == transform)
            {
                other.GetComponent<Agent>().CheckpointReached(nextCheckpoint);
            }
        }
    }

}
