using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// This script was a collaborative effort with team members on City Block Cats.
// I change the input context enum per the games' needs.

public enum INPUT_CONTEXT
{
    NONE,
    MODAL,
    GAME,
    PAUSE
}

public class InputManager
{
    public static Stack<INPUT_CONTEXT> contextStack = new Stack<INPUT_CONTEXT>();

    /// <summary>
    /// Pushes an input context onto the stack. We don't push if it already exists.
    /// </summary>
    /// <param name="context"></param>
    public static void PushInputContext(INPUT_CONTEXT context)
    {
        if (context == contextStack.Peek())
        {
            Debug.Log("[InputManager.cs] Context is already " + contextStack.Peek().ToString());
            return;
        }

        contextStack.Push(context);
        Debug.Log("[InputManager.cs] Added context " + contextStack.Peek().ToString());
    }

    /// <summary>
    /// Pops an input context off of the stack. If it's not on the top, we don't pop.
    /// </summary>
    /// <param name="context"></param>
    public static void PopInputContext(INPUT_CONTEXT context)
    {
        if (contextStack.Count == 0)
        {
            contextStack.Push(INPUT_CONTEXT.NONE);
            return;
        }

        if (contextStack.Peek() == context)
        {
            INPUT_CONTEXT removedContext = contextStack.Pop();
            Debug.Log("InputManger: Removed context " + removedContext.ToString());
        } 
    }

    public static void RemoveAllContexts()
    {
        contextStack.Clear();
    }

    /// <summary>
    /// Determines whether the provided context is on the current
    /// input context stack.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static bool HasContext(INPUT_CONTEXT context)
    {
        return contextStack.Contains(context);
    }

    /// <summary>
    /// Gets the current game context
    /// </summary>
    /// <returns>InputManager.m_currentContext</returns>
    public static INPUT_CONTEXT GetCurrentContext()
    {
        return contextStack.Peek();
    }

    public static bool IsAboveUI(Vector3 cursorPosition)
    {
        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = cursorPosition;

        List<RaycastResult> pHits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, pHits);

        if (pHits.Count > 0)
        {
            foreach (RaycastResult hit in pHits)
            {
                if (hit.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static float GetAxis(string axis, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetAxis(axis);
        }
        return 0;
    }

    public static float GetAxisRaw(string axis, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetAxisRaw(axis);
        }
        return 0;
    }

    public static bool GetKey(KeyCode key, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetKey(key);
        }
        return false;
    }

    public static bool GetKeyDown(KeyCode key, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetKeyDown(key);
        }
        return false;
    }

    public static bool GetKeyUp(KeyCode key, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetKeyUp(key);
        }
        return false;
    }

    public static bool GetButton(string button, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetButton(button);
        }
        return false;
    }

    public static bool GetButtonDown(string button, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetButtonDown(button);
        }
        return false;
    }

    public static bool GetButtonUp(string button, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetButtonUp(button);
        }
        return false;
    }

    public static bool GetMouseButton(int button, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetMouseButton(button);
        }
        return false;
    }

    public static bool GetMouseButtonDown(int button, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetMouseButtonDown(button);
        }
        return false;
    }

    public static bool GetMouseButtonUp(int button, INPUT_CONTEXT context)
    {
        if (contextStack.Peek() == context)
        {
            return Input.GetMouseButtonUp(button);
        }
        return false;
    }
}
