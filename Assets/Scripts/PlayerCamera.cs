using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : CameraController
{
    public override void Init()
    {
        Destroy(target.GetComponent<Agent>());
    }
    
}
