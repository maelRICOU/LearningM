using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public CarController carController;

    private void Update()
    {
        carController.horizontalInput = Input.GetAxis("Horizontal");
        carController.verticalInput = Input.GetAxis("Vertical");
    }
}
