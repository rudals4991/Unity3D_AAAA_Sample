using System;
using System.Collections;
using UnityEngine;

public class CountManager : MonoBehaviour, IManagerBase
{
    public int Priority => 1;
    public bool IsGameActive { get; private set; } = false;
    public bool IsCounting { get; private set; } = false;
    public event Action<int> OnCountDown;
    public event Action OnCountDownFin;

    float remain;
    bool isFlag;

    public void Exit()
    {
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
    }
    public void StartFirstCountDown(float second = 3f)
    {
        if (isFlag)
        {
            IsGameActive = true;
            IsCounting = false;
            return;
        }
        isFlag = true;
        StartCountDown(second);
    }
    public void StartResumeCountDown(float second = 3f)
    {
        StartCountDown(second);
    }
    void StartCountDown(float second)
    {
        remain = Mathf.Max(0.1f, second);
        IsGameActive = false;
        IsCounting = true;
        OnCountDown?.Invoke(Mathf.CeilToInt(remain));
    }
    public void Tick(float dt)
    {
        if (!IsCounting) return;
        float prev = remain;
        remain -= dt;
        int prevInt = Mathf.CeilToInt(prev);
        int curInt = Mathf.CeilToInt(Mathf.Max(remain,0));
        if (curInt != prevInt) OnCountDown?.Invoke(curInt);
        if (remain <= 0)
        {
            IsCounting = false;
            IsGameActive = true;
            OnCountDownFin?.Invoke();
        }
    }
}
