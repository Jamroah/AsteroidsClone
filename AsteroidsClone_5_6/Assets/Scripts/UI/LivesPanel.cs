// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using System.Collections;

public class LivesPanel : MonoBehaviour
{
    public GameObject livesIcon;

    private GameObjectPool m_iconPool = new GameObjectPool();

    void Start()
    {
        m_iconPool = new GameObjectPool(livesIcon, 10, false, transform, true);
    }

    void OnEnable()
    {
        Messenger.AddListener("Update UI", UpdateLives);
    }

    void OnDisable()
    {
        Messenger.RemoveListener("Update UI", UpdateLives);
    }

    void UpdateLives()
    {
        m_iconPool.DisableAll();

        for(int i = 0; i < GameManager.CurrentLives; i++)
        {
            m_iconPool.Get(true);
        }
    }
}
