// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameObjectPool
{
    private GameObject m_pooledObject;
    private Transform m_parent;
    private int m_capacity;
    private bool m_willGrow;
    private List<GameObject> m_pool;

    public GameObjectPool(GameObject objectToPool, int capacity, bool willGrow)
    {
        m_pooledObject = objectToPool;
        m_capacity = capacity;
        m_willGrow = willGrow;
        Initialise();
    }

    public GameObjectPool(GameObject objectToPool, int capacity, bool willGrow, GameObject parent)
    {
        m_pooledObject = objectToPool;
        m_capacity = capacity;
        m_willGrow = willGrow;
        m_parent = parent.transform;
        Initialise();
    }

    public GameObjectPool(GameObject objectToPool, int capacity, bool willGrow, Transform parent)
    {
        m_pooledObject = objectToPool;
        m_capacity = capacity;
        m_willGrow = willGrow;
        m_parent = parent;
        Initialise();
    }

    /// <summary>
    /// Activates a game object stored in the pool. If pool is full, instantiates a new game object, adds it to the pool and activates it.
    /// </summary>
    /// <returns></returns>
    public GameObject Get(bool active = false)
    {
        for (int i = 0; i < m_pool.Count; i++)
        {
            if(!m_pool[i].activeInHierarchy)
            {
                m_pool[i].SetActive(active);
                return m_pool[i];
            }
        }

        if(m_willGrow)
        {
            GameObject go = (GameObject)MonoBehaviour.Instantiate(m_pooledObject);

            if (m_parent != null)
                go.transform.parent = m_parent;

            m_pool.Add(go);

            go.SetActive(active);

            return go;
        }

        return null;
    }

    private void Initialise()
    {
        m_pool = new List<GameObject>();

        for (int i = 0; i < m_capacity; i++)
        {
            GameObject go = (GameObject)MonoBehaviour.Instantiate(m_pooledObject);
            go.SetActive(false);

            if (m_parent != null)
                go.transform.parent = m_parent;

            m_pool.Add(go);
        }
    }
}
