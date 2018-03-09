// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using System.Collections;

public class ShipMotor : MonoBehaviour
{
    private ShipController controller;

    // Use this for initialization
    void OnEnable()
    {
        controller = gameObject.AddOrGetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.canControl)
        {
            Debug.Log("Ship control is disabled");
            return;
        }

        // We send a reversed value because of Unity. Grr.
        controller.Rotate(-InputManager.GetAxisRaw("Horizontal", INPUT_CONTEXT.GAME));

        float accelAxis = InputManager.GetAxisRaw("Accelerate", INPUT_CONTEXT.GAME);

        if(InputManager.GetButtonDown("Fire", INPUT_CONTEXT.GAME))
        {
            controller.Fire();
        }

        if (InputManager.GetButton("Accelerate", INPUT_CONTEXT.GAME) || accelAxis > 0)
        {
            controller.Accelerate();
        }

        if (InputManager.GetButtonUp("Accelerate", INPUT_CONTEXT.GAME) || accelAxis < 1)
        {
            controller.Stopping();
        }
    }
}
