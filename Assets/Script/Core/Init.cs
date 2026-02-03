using System.Collections.Generic;
using UnityEngine;
public interface IStart
{
    public void OnStart();
}
public interface IUpdate
{
    public void OnUpdate();
}
public interface IDestroy
{
    public void Destroy();
}
public abstract class Base : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        Init.Register(this);
    }

    protected virtual void OnDisable()
    {
        Init.Unregister(this);
    }
}
public class Init : MonoBehaviour
{
    private static Init instance;
    private readonly List<IStart> starts = new List<IStart>();
    private readonly List<IUpdate> updates = new List<IUpdate>();
    private readonly List<IDestroy> destroys = new List<IDestroy>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        foreach (var start in starts)
        {
            start.OnStart();
        }
    }
    private void Update()
    {
        foreach (var update in updates)
        {
            update.OnUpdate();
        }
    }
    private void OnDestroy()
    {
        foreach (var destroy in destroys)
        {
            destroy.Destroy();
        }
    }

    public static void Register<T>(T t)
    {
        switch (t)
        {
            case IStart iStart:
                instance.starts.Add(iStart);
                break;
            case IUpdate iUpdate:
                instance.updates.Add(iUpdate);
                break;
            case IDestroy iDestroy:
                instance.destroys.Add(iDestroy);
                break;
        }
    }

    public static void Unregister<T>(T t)
    {
        switch (t)
        {
            case IStart iStart:
                instance.starts.Remove(iStart);
                break;
            case IUpdate iUpdate:
                instance.updates.Remove(iUpdate);
                break;
            case IDestroy iDestroy:
                instance.destroys.Remove(iDestroy);
                break;
        }
    }
}
