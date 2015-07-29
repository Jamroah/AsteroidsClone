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
        
        controller.Rotate(Input.GetAxisRaw("Horizontal"));
    }
}
