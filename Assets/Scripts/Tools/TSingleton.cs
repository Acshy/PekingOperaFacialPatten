using UnityEngine;


public class TSingleton<T>  where T : TSingleton<T>, new()
{
    static T instance = null;
    public static T Inst
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}
