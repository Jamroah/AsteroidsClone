using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[Obsolete("Use TimerManager instead.")]
public class StopWatch: Singleton<StopWatch>
{
    protected StopWatch() { }

    /*IEnumerator Start()
    {
        SetupTimer("Test Timer", 120f);
        StartTimer("Test Timer");

        yield return new WaitForSeconds(4);

        StopTimer("Test Timer");
        SetupTimer("Test Timer2", 10f);
        StartTimer("Test Timer2");

        yield return new WaitForSeconds(15);
        StartTimer("Test Timer");
    }*/

    class Timer
    {
        public string Name
        {
            get { return m_uid; }
            private set { }
        }

        public bool Activated
        {
            get { return m_activated; }
            set
            {
                m_activated = value;

                if(value == false)
                {
                    Reset();
                    m_numOfActiveTimers--;
                }
                else
                {
                    m_numOfActiveTimers++;
                }
            }
        }

        //public bool Paused
        //{
        //    get { return m_paused; }
        //    set { m_paused = value; }
        //}

        public float CurrentTime
        {
            get { return m_curT; }
            set { m_curT = value; }
        }

        public void Reset()
        {
            m_curT = m_t;
        }

        public Timer(string uid, float t)
        {
            m_uid = uid;
            m_t = t;
            m_curT = t;
        }

        public Timer() { }

        public UnityAction callback = null;

        bool m_activated = false;
        //bool m_paused = false;
        float m_t = 0f;
        float m_curT = 0f;
        string m_uid = "";
    }

    private static Dictionary<string, Timer> m_timers = new Dictionary<string, Timer>();
    private Dictionary<string, IEnumerator> m_countdownInstances = new Dictionary<string, IEnumerator>();

    /// <summary>
    /// Gets number of timers that are currently active, regardless of paused state.
    /// </summary>
    public static int NumOfActiveTimers
    {
        get { return m_numOfActiveTimers; }
        private set { }
    }

    private static int m_numOfActiveTimers = 0;

    /// <summary>
    /// Set up a new Timer
    /// </summary>
    /// <param name="uid">Unique identifier</param>
    /// <param name="t">The starting timer value</param>
    public static void SetupTimer(string uid, float t, UnityAction callback = null)
    {
        Timer T = new Timer(uid, t);
        T.callback = callback;

        if(m_timers.ContainsKey(uid))
        {
            Debug.LogWarning("StopWatch already contains a timer called '" + uid + "'. Please call it something else.");
            return;
        }

        m_timers.Add(uid, T);
        instance.m_countdownInstances.Add(uid, instance._StartWatch(uid));
    }

    public static void StartTimer(string uid)
    {
        if (!instance.m_countdownInstances.ContainsKey(uid))
        {
            Debug.Log("err404 IEnumerator doesn't exist");
            return;
        }

        Debug.Log("Starting Timer: " + uid);
        instance.StartCoroutine(instance.m_countdownInstances[uid]);
    }

    public static void StopTimer(string uid)
    {
        if (!instance.m_countdownInstances.ContainsKey(uid))
        {
            return;
        }

        instance.StopCoroutine(instance.m_countdownInstances[uid]);
    }

    public static void ResetTimer(string uid)
    {
        if(!m_timers.ContainsKey(uid))
        {
            return;
        }

        m_timers[uid].Reset();
    }

    public static void RemoveTimer(string uid)
    {
        StopTimer(uid);

        if (m_timers.ContainsKey(uid))
        {
            m_timers.Remove(uid);
        }

        if (instance.m_countdownInstances.ContainsKey(uid))
        {
            instance.m_countdownInstances.Remove(uid);
        }
    }

    /// <summary>
    /// Returns the current time of a timer.
    /// </summary>
    /// <param name="uid">Unique Identifier</param>
    /// <param name="formatted">True if you want the returned time in mm:ss format</param>
    /// <returns></returns>
    public static string GetTime(string uid, bool formatted)
    {
        if(!m_timers.ContainsKey(uid))
        {
            return "error404";
        }

        if(formatted)
        {
            //return TimeSpan.FromSeconds(m_timers[uid].CurrentTime).ToString().Remove(0,3).Remove(5);
            return String.Format("{0}:{1}", ((int)m_timers[uid].CurrentTime / 60).ToString().PadLeft(2, '0'), ((int)m_timers[uid].CurrentTime % 60).ToString().PadLeft(2, '0'));
        }

        return m_timers[uid].CurrentTime.ToString();
    }

    private IEnumerator _StartWatch(string uid)
    {
        //Check if timer exists
        if(!m_timers.ContainsKey(uid))
        {
            yield break;
        }

        m_timers[uid].Activated = true;

        while (m_timers[uid].Activated)
        {
            //if (!m_timers[uid].Paused)
            //{
                m_timers[uid].CurrentTime -= Time.deltaTime;
                //Debug.Log(uid + " " + GetTime(uid, true));
            //}

            yield return null;
           
            if(m_timers[uid].CurrentTime <= 0)
            {
                m_timers[uid].Activated = false;
            }
        }

        if (m_timers[uid].callback != null)
        {
            m_timers[uid].callback();
        }
    }
}
