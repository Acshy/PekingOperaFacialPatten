using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    static EventManager instance = null;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();
            }
            return instance;
        }
    }


    Dictionary<string, Action<object>> _events = new Dictionary<string, Action< object>>();
    List<Action<GameObject, string>> _animatorEvents = new List<Action<GameObject, string>>();

    /// <summary>
    /// 监听事件(游戏物体销毁时需要ReMove_Event)
    /// </summary>
    /// <param name="eventName">事件名字</param>
    /// <param name="callBack">事件</param>
    public void AddEvent(string eventName, Action<object> callBack)
    {
        if (!_events.ContainsKey(eventName))
        {
            _events.Add(eventName, delegate (object o) { });
        }

        _events[eventName] += callBack;
    }

    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="eventName">事件名字</param>
    /// <param name="callBack">事件</param>
    public void RemoveEvent(string eventName, Action<object> callBack)
    {
        if (_events.ContainsKey(eventName))
        {
            // ReSharper disable once DelegateSubtraction
            _events[eventName] -= callBack;
        }

    }

    /// <summary>
    /// 派发事件
    /// </summary>
    /// <param name="eventName">事件名字</param>
    /// <param name="value">事件参数(默认为null)</param>
    public void DistributeEvent(string eventName, object value = null)
    {
        if (!_events.ContainsKey(eventName))
        {
            return;
        }

        _events[eventName](value);
    }
}
