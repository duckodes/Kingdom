using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
public interface IAwake
{
    public void OnAwake();
}
public interface IStart
{
    public void OnStart();
}
public interface IUpdate
{
    public void OnUpdate();
}
public interface IFixedUpdate
{
    public void OnFixedUpdate();
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
    private readonly List<IAwake> awakes = new List<IAwake>();
    private readonly List<IStart> starts = new List<IStart>();
    private readonly List<IUpdate> updates = new List<IUpdate>();
    private readonly List<IFixedUpdate> fixedUpdates = new List<IFixedUpdate>();
    private readonly List<IDestroy> destroys = new List<IDestroy>();

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            gameObject.AddComponent<KeyCodes>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        Addressables.LoadSceneAsync(nameof(SceneType.Lobby), LoadSceneMode.Single);
    }
    private void Start()
    {
        foreach (var awake in awakes)
        {
            awake.OnAwake();
        }
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
    private void FixedUpdate()
    {
        foreach (var fixedUpdate in fixedUpdates)
        {
            fixedUpdate.OnFixedUpdate();
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
        if (t is IAwake iAwake)
        {
            instance.awakes.Add(iAwake);
        }
        if (t is IStart iStart)
        {
            instance.starts.Add(iStart);
        }
        if (t is IUpdate iUpdate)
        {
            instance.updates.Add(iUpdate);
        }
        if (t is IFixedUpdate iFixedUpdate)
        {
            instance.fixedUpdates.Add(iFixedUpdate);
        }
        if (t is IDestroy iDestroy)
        {
            instance.destroys.Add(iDestroy);
        }
    }

    public static void Unregister<T>(T t)
    {
        if (t is IAwake iAwake)
        {
            instance.awakes.Remove(iAwake);
        }
        if (t is IStart iStart)
        {
            instance.starts.Remove(iStart);
        }
        if (t is IUpdate iUpdate)
        {
            instance.updates.Remove(iUpdate);
        }
        if (t is IFixedUpdate iFixedUpdate)
        {
            instance.fixedUpdates.Remove(iFixedUpdate);
        }
        if (t is IDestroy iDestroy)
        {
            instance.destroys.Remove(iDestroy);
        }
    }
}
