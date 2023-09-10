using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OpenEndedStack<T>
{
    [SerializeField]
    protected T[] ts;
    public OpenEndedStack(int length)
    {
        ts = new T[length];
    }

    public void Push(T t)
    {
        MoveDown(t, 0);
    }

    void MoveDown(T t, int i)
    {
        if (ts[i] == null || i + 1 >= ts.Length)
        {
            ts[i] = t;
            //and done!
        }
        else
        {
            MoveDown(ts[i], i + 1);
            ts[i] = t;
        }
    }
    public T GetByIndex(int i) => ts[i];
}
