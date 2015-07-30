using UnityEngine;
using System.Collections;

public class ShipMotor : MonoBehaviour
{
    private ShipController controller;

    // Use this for initialization
    void Start()
    {
        controller = this.AddOrGetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        // We send a reversed value because of Unity. Grr.
        controller.Rotate(-Input.GetAxisRaw("Horizontal"));

        if(Input.GetButtonDown("Fire1"))
        {
            controller.Fire();
        }

        if(Input.GetButton("Accelerate"))
        {
            controller.Accelerate();
        }

        if (Input.GetButtonUp("Accelerate"))
        {
            controller.Stopping();
        }
    }
}
