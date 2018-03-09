using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Utility found on https://code.google.com/p/spacepuppy-unity-framework/source/browse/trunk/SpacepuppyUnityFramework/Utils/?r=48
/// This utility script is free to use. Do not attach our license to this script!
/// </summary>

public static class ComponentUtil
{
    //#region Component Methods

    //public static bool IsComponentSource(object obj)
    //{
    //    return (obj is GameObject || obj is Component);
    //}

    //#region HasComponent

    //public static bool HasComponent<T>(this GameObject obj) where T : Component
    //{
    //    return (obj.GetComponent<T>() != null);
    //}
    //public static bool HasComponent<T>(this Component obj) where T : Component
    //{
    //    return (obj.GetComponent<T>() != null);
    //}

    //public static bool HasComponent(this GameObject obj, string tp)
    //{
    //    return (obj.GetComponent(tp) != null);
    //}
    //public static bool HasComponent(this Component obj, string tp)
    //{
    //    return (obj.GetComponent(tp) != null);
    //}

    //public static bool HasComponent(this GameObject obj, System.Type tp)
    //{
    //    return (obj.GetComponent(tp) != null);
    //}
    //public static bool HasComponent(this Component obj, System.Type tp)
    //{
    //    return (obj.GetComponent(tp) != null);
    //}

    //public static bool HasComponentOfInterface<T>(this GameObject obj)
    //{
    //    foreach (var comp in obj.GetComponents<Component>())
    //    {
    //        if (comp is T) return true;
    //    }

    //    return false;
    //}

    //public static bool HasComponentOfInterface<T>(this Component obj)
    //{
    //    foreach (var comp in obj.GetComponents<Component>())
    //    {
    //        if (comp is T) return true;
    //    }

    //    return false;
    //}

    //public static bool HasComponentOfInterface(this GameObject obj, System.Type tp)
    //{
    //    foreach (var comp in obj.GetComponents<Component>())
    //    {
    //        if (ObjUtil.IsType(comp.GetType(), tp)) return true;
    //    }

    //    return false;
    //}

    //public static bool HasComponentOfInterface(this Component obj, System.Type tp)
    //{
    //    foreach (var comp in obj.GetComponents<Component>())
    //    {
    //        if (ObjUtil.IsType(comp.GetType(), tp)) return true;
    //    }

    //    return false;
    //}

    //#endregion

    #region AddComponent

    public static T AddOrGetComponent<T>(this GameObject obj) where T : Component
    {
        if (obj == null) return null;

        T comp = obj.GetComponent<T>();
        if (comp == null)
        {
            comp = obj.AddComponent<T>();
        }

        return comp;
    }

    public static T AddOrGetComponent<T>(this Component obj) where T : Component
    {
        if (obj == null) return null;

        T comp = obj.GetComponent<T>();
        if (comp == null)
        {
            comp = obj.gameObject.AddComponent<T>();
        }

        return comp;
    }

    //public static Component AddOrGetComponent(this GameObject obj, System.Type tp)
    //{
    //    if (obj == null) return null;
    //    if (!ObjUtil.IsType(tp, typeof(ComponentUtil))) return null;

    //    var comp = obj.GetComponent(tp);
    //    if (comp == null)
    //    {
    //        comp = obj.AddComponent(tp);
    //    }

    //    return comp;
    //}

    //public static Component AddOrGetComponent(this Component obj, System.Type tp)
    //{
    //    if (obj == null) return null;
    //    if (!ObjUtil.IsType(tp, typeof(ComponentUtil))) return null;

    //    var comp = obj.GetComponent(tp);
    //    if (comp == null)
    //    {
    //        comp = obj.gameObject.AddComponent(tp);
    //    }

    //    return comp;
    //}

    #endregion

    #region GetComponent

    // ############
    // GetComponent
    // #########

    public static bool GetComponent<T>(this GameObject obj, out T comp) where T : Component
    {
        comp = obj.GetComponent<T>();
        return (comp != null);
    }
    public static bool GetComponent<T>(this Component obj, out T comp) where T : Component
    {
        comp = obj.GetComponent<T>();
        return (comp != null);
    }

    public static bool GetComponent(this GameObject obj, string tp, out Component comp)
    {
        comp = obj.GetComponent(tp);
        return (comp != null);
    }
    public static bool GetComponent(this Component obj, string tp, out Component comp)
    {
        comp = obj.GetComponent(tp);
        return (comp != null);
    }

    public static bool GetComponent(this GameObject obj, System.Type tp, out Component comp)
    {
        comp = obj.GetComponent(tp);
        return (comp != null);
    }
    public static bool GetComponent(this Component obj, System.Type tp, out Component comp)
    {
        comp = obj.GetComponent(tp);
        return (comp != null);
    }


    //// ############
    //// GetFirstComponent
    //// #########

    //public static T GetFirstComponent<T>(this GameObject obj) where T : Component
    //    {
    //        if (obj == null) return default(T);
 
    //        var arr = obj.GetComponents<T>();
    //        if (arr != null && arr.Length > 0)
    //            return arr[0];
    //        else
    //            return default(T);
    //    }

    //public static T GetFirstComponent<T>(this Component obj) where T : Component
    //    {
    //        if (obj == null) return default(T);
 
    //        var arr = obj.GetComponents<T>();
    //        if (arr != null && arr.Length > 0)
    //            return arr[0];
    //        else
    //            return default(T);
    //    }

    //public static T GetFirstComponentOfInterface<T>(this GameObject obj)
    //{
    //    if (obj == null) return default(T);

    //    foreach (object comp in obj.GetComponents<Component>())
    //    {
    //        if (comp is T) return (T)comp;
    //    }

    //    return default(T);
    //}

    //public static T GetFirstComponentOfInterface<T>(this Component obj)
    //{
    //    if (obj == null) return default(T);

    //    foreach (object comp in obj.GetComponents<Component>())
    //    {
    //        if (comp is T) return (T)comp;
    //    }

    //    return default(T);
    //}

    //public static bool GetFirstComponentOfInterface<T>(this GameObject obj, out T comp)
    //{
    //    comp = default(T);
    //    if (obj == null) return false;

    //    foreach (object c in obj.GetComponents<Component>())
    //    {
    //        if (comp is T)
    //        {
    //            comp = (T)c;
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public static bool GetFirstComponentOfInterface<T>(this Component obj, out T comp)
    //{
    //    comp = default(T);
    //    if (obj == null) return false;

    //    foreach (object c in obj.GetComponents<Component>())
    //    {
    //        if (c is T)
    //        {
    //            comp = (T)c;
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //// ############
    //// GetComponents
    //// #########

    //public static T[] GetComponentsOfInterface<T>(this GameObject obj)
    //{
    //    if (obj == null) return null;

    //    var lst = new List<T>();
    //    foreach (object comp in obj.GetComponents<Component>())
    //    {
    //        if (comp is T) lst.Add((T)comp);
    //    }
    //    return lst.ToArray();
    //}

    //public static T[] GetComponentsOfInterface<T>(this Component obj)
    //{
    //    if (obj == null) return null;

    //    var lst = new List<T>();
    //    foreach (object comp in obj.GetComponents<Component>())
    //    {
    //        if (comp is T) lst.Add((T)comp);
    //    }
    //    return lst.ToArray();
    //}

    //#endregion

    //#region RemoveComponent

    //public static void RemoveComponent<T>(this GameObject obj) where T : Component
    //{
    //    var comp = obj.GetComponent<T>();
    //    if (comp != null) Object.Destroy(comp);
    //}

    //public static void RemoveComponent<T>(this Component obj) where T : Component
    //{
    //    var comp = obj.GetComponent<T>();
    //    if (comp != null) Object.Destroy(comp);
    //}

    //public static void RemoveComponent(this GameObject obj, string tp)
    //{
    //    var comp = obj.GetComponent(tp);
    //    if (comp != null) Object.Destroy(comp);
    //}

    //public static void RemoveComponent(this Component obj, string tp)
    //{
    //    var comp = obj.GetComponent(tp);
    //    if (comp != null) Object.Destroy(comp);
    //}

    //public static void RemoveComponent(this GameObject obj, System.Type tp)
    //{
    //    var comp = obj.GetComponent(tp);
    //    if (comp != null) Object.Destroy(comp);
    //}

    //public static void RemoveComponent(this Component obj, System.Type tp)
    //{
    //    var comp = obj.GetComponent(tp);
    //    if (comp != null) Object.Destroy(comp);
    //}

    //public static void RemoveFromOwner(this Component comp)
    //{
    //    Object.Destroy(comp);
    //}

    //#endregion

    #endregion
}