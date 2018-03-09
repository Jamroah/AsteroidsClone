// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public enum TIMESCALE_OPTIONS
{
    TIMESCALE_0,
    TIMESCALE_1,
}

public class ModalPanel : Singleton<ModalPanel>
{
    protected ModalPanel() { }

    public GameObject modalPanel;
    public Text title;
    public Text subtitle;

    public GameObject bossPanel;
    public Text warning;
    public Text message;
    public Slider bossHealth;

    private static UnityAction action;
    private static string button;
    private static List<string> buttons;

    void Awake()
    {
        ClosePanel();
        bossPanel.SetActive(false);
    }

    void Update()
    {
        if (action == null || string.IsNullOrEmpty(button))
            return;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (Input.GetButtonDown(buttons[i]))
            {
                if (InputManager.HasContext(INPUT_CONTEXT.MODAL) && modalPanel.activeInHierarchy)
                {
                    action();
                }
            }
        }
    }

    /// <summary>
    /// Opens the modal panel and pushes INPUT_CONTEXT modal.
    /// </summary>
    /// <param name="options"></param>
    public static void OpenPanel(TIMESCALE_OPTIONS options)
    {
        Instance.modalPanel.SetActive(true);
        InputManager.PushInputContext(INPUT_CONTEXT.MODAL);

        switch (options)
        {
            case TIMESCALE_OPTIONS.TIMESCALE_0:
                Time.timeScale = 0;
                break;
            case TIMESCALE_OPTIONS.TIMESCALE_1:
                Time.timeScale = 1;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Closes the modal panel and pops the INPUT_CONTEXT modal.
    /// </summary>
    public static void ClosePanel()
    {
        Time.timeScale = 1;
        InputManager.PopInputContext(INPUT_CONTEXT.MODAL);
        Instance.modalPanel.SetActive(false);
    }

    // The modular function. The other functions are pretty much this only more focussed.
    public static void Message(string msg1, string msg2, string buttonString, UnityAction callback, TIMESCALE_OPTIONS options)
    {
        OpenPanel(options);

        Instance.title.text = msg1;
        Instance.subtitle.text = msg2;

        button = buttonString;

        action = () => { callback(); ClosePanel(); };
    }

    // Use if you want to open up a new panel in the callback.
    public static void Message(string msg1, string msg2, string buttonString, UnityAction callback, bool closePanel)
    {
        OpenPanel(TIMESCALE_OPTIONS.TIMESCALE_1);

        Instance.title.text = msg1;
        Instance.subtitle.text = msg2;

        button = buttonString;

        action = () =>
        {
            callback();
            if (closePanel)
                ClosePanel();
        };
    }

    public static void Menu(UnityAction callback)
    {
        OpenPanel(TIMESCALE_OPTIONS.TIMESCALE_1);

        Instance.title.text = "JAM'S ASTEROIDS";
        Instance.subtitle.text = "Press FIRE to play\n\nHighscore: " + GameManager.HighScore;

        button = "Fire";

        if(buttons != null) buttons.Clear();
        buttons = new List<string> { "Fire", "Start" };

        action = () =>
        {
            ClosePanel();
            callback();          
        };
    }

    public static void GameOver(UnityAction callback)
    {
        OpenPanel(TIMESCALE_OPTIONS.TIMESCALE_1);

        Instance.title.text = "GAME OVER";
        Instance.subtitle.text = "Press START to restart";

        button = "Start";

        if (buttons != null) buttons.Clear();
        buttons = new List<string> { "Fire", "Start" };

        action = () =>
        {
            ClosePanel();
            callback();             
        };
    }

    public static void PauseGame()
    {
        OpenPanel(TIMESCALE_OPTIONS.TIMESCALE_0);

        InputManager.PushInputContext(INPUT_CONTEXT.PAUSE);

        Instance.title.text = "PAUSED";
        Instance.subtitle.text = "";

        button = "";

        if (buttons != null) buttons.Clear();

        action = null;
    }

    public static void UnPauseGame()
    {
        InputManager.PopInputContext(INPUT_CONTEXT.PAUSE);
        ClosePanel();
    }

    public static void EnableBossPanel()
    {
        Instance.bossPanel.SetActive(true);
        Instance.message.enabled = true;
        Instance.warning.enabled = true;
    }

    public static void DisableBossMessage()
    {
        Instance.message.enabled = false;
        Instance.warning.enabled = false;
    }
}
