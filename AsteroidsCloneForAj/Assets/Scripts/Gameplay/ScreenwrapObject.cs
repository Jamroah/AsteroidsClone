using UnityEngine;
using System.Collections;

public class ScreenwrapObject : MonoBehaviour
{
    public bool startOffScreen;
    public bool canWrap = true;

    protected Camera MainCamera
    {
        get
        {
            if(m_camera == null)
                m_camera = Camera.main;
            return m_camera;
        }
    }
    protected Transform m_transform;
    protected Vector2 m_position;
    protected Renderer[] m_renderers;

    private Vector3 m_viewportPosition = new Vector3();
    private bool wrapX, wrapY, hasAppeared = true;
    private Camera m_camera;

    public virtual void Awake()
    {
        m_transform = transform;
    }

    public virtual void OnEnable()
    {
        m_renderers = GetComponentsInChildren<Renderer>();

        if (startOffScreen)
            hasAppeared = false;
    }

    public virtual void Update()
    {
        if(canWrap)
            DoScreenWrap();
    }

    public void DisableScreenWrap()
    {
        canWrap = false;
    }

    public void EnableScreenWrap()
    {
        canWrap = true;
    }

    void DoScreenWrap()
    {
        m_position = m_transform.localPosition;
        m_viewportPosition = MainCamera.WorldToViewportPoint(m_position);

        if(CheckIfVisible())
        {
            wrapX = false;
            wrapY = false;
            hasAppeared = true;
            return;
        }

        if (!hasAppeared)
            return;

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
