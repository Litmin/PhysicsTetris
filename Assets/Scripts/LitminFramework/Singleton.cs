using System;

public class Singleton<T> where T : new()
{
    public static T instance
    {
        get
        {
            return Singleton<T>._GetInstance();
        }
    }

    public static bool exists
    {
        get
        {
            return !object.Equals(Singleton<T>._instance, default(T));
        }
    }

    private static T _GetInstance()
    {
        if (object.Equals(Singleton<T>._instance, default(T)))
        {
            Singleton<T>._instance = Activator.CreateInstance<T>();
        }
        return Singleton<T>._instance;
    }

    protected static T _instance;
}
