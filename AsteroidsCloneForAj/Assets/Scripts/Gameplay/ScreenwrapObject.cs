using UnityEngine;
using System.Collections;

public class ScreenwrapObject : MonoBehaviour
{
    private Camera m_camera;
    private Renderer[] m_renderers;
    private Vector3 m_viewportPosition = new Vector3();
    private bool wrapX, wrapY;

    protected Transform m_transform;
    protected Vector2 m_position;

    public virtual void Start()
    {
        m_camera = Camera.main;
        // Create a reference to stop from incessant GetComponent calls.
        m_transform = transform;
    }

    public virtual void OnEnable()
    {
        m_renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        DoScreenWrap();
    }

    void DoScreenWrap()
    {
        m_position = m_transform.localPosition;
        m_viewportPosition = m_camera.WorldToViewportPoint(m_position);

        if(CheckIfVisible())
        {
            wrapX = false;
            wrapY = false;
            return;
        }

        if(!wrapX && (m_viewportPosition.x > 1 || m_viewportPosition.x < 0))
        {
            m_position.x = -m_position.x;
            wrapX = true;
        }

        if (!wrapY && (m_viewportPosition.y > 1 || m_viewportPosition.y < 0))
        {
            m_position.y = -m_position.y;
            wrapY = true;
        }

        m_transform.localPosition = m_position;
    }

    bool CheckIfVisible()
    {
        if (m_renderers.Length <= 0)
            return false;

        for(int i = 0; i < m_renderers.Length; i++)
        {
            if (m_renderers[i].isVisible)
                return true;
        }

        return false;
    }
}
