using System;

public static class EventHandlerExtensions
{
    public static void SafeInvoke<T>(this EventHandler<T> instance, object sender, T e) where T : EventArgs
    {
        if(instance != null)
            instance(sender, e);
    }
}