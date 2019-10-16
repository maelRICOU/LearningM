using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    public Transform firstCheckpoint;

    private void Awake()
    {
        instance = this;
    }
}
