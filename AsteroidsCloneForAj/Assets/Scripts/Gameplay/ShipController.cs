using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{
    public Sprite bullet;

    public float maxSpeed;
    public float acceleration;
    public float linearDrag;
    //public void angleDrag;
    [Range(5, 10)]
    public float rotSpeed;

    private float velocity;

    private Transform m_transform;

    // Use this for initialization
    void Start()
    {
        // Create a reference to stop from incessant GetComponent calls.
        m_transform = transform;
    }

    public void Fire()
    {

    }

    public void Accelerate()
    {

    }

    public void Rotate(float direction)
    {
        m_transform.Rotate((Vector3.forward * -direction) * rotSpeed);
    }
}
