using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CustomCoroutine : MonoBehaviour
{
    public static System.DateTime StartTime;

    private IEnumerator test;

    public void StartCustom(IEnumerator test)
    {
        this.test = test;
        ThreadStart start = new ThreadStart(Do);
        Thread thread = new Thread(start);
        thread.Start();
    }

    private void Do()
    {
        StartTime = System.DateTime.Now;
        while (test.MoveNext())
        {
            while ((test.Current as IEnumerator).MoveNext())
            {
                Debug.LogError("Move Next:" + (int)(System.DateTime.Now - CustomCoroutine.StartTime).TotalSeconds);
            }
        }
    }

    private void OnEnable()
    {
        StartCustom(TestCustom());
    }

    public IEnumerator TestCustom()
    {
        Debug.LogError("Start:" + (int)(System.DateTime.Now - CustomCoroutine.StartTime).TotalSeconds);
        yield return new CustomWaitTime(5);
        Debug.LogError("End:" + (int)(System.DateTime.Now - CustomCoroutine.StartTime).TotalSeconds);
    }
}

public class CustomWaitTime : IEnumerator
{
    public CustomWaitTime(float time)
    {
        waitTime = time;
    }
    public float waitTime
    {
        get;
        set;
    }

    private float m_WaitUntilTime = -1f;

    object IEnumerator.Current => null;

    bool IEnumerator.MoveNext()
    {
        if (m_WaitUntilTime < 0f)
        {
            m_WaitUntilTime = (int)(System.DateTime.Now - CustomCoroutine.StartTime).TotalSeconds + waitTime;
        }
        bool flag = (int)(System.DateTime.Now - CustomCoroutine.StartTime).TotalSeconds < m_WaitUntilTime;
        if (!flag)
        {
            m_WaitUntilTime = -1f;
        }
        return flag;
    }

    void IEnumerator.Reset()
    {
    }
}
