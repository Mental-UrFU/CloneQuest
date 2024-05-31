using System;
using System.Collections;
using UnityEngine;

public sealed class Coroutines : MonoBehaviour
{
    public static Coroutines Instance { get => _instance != null ? _instance : CreateComponent(); }

    public static Coroutine Run(IEnumerator enumerator) => Instance.StartCoroutine(enumerator);
    public static Coroutine Run(Action action, YieldInstruction yieldInstruction) => Instance.StartCoroutine(ActionRoutine(action, yieldInstruction));
    public static void Stop(IEnumerator enumerator) => Instance.StopCoroutine(enumerator);
    public static void Stop(Coroutine coroutine) => Instance.StopCoroutine(coroutine);

    private static IEnumerator ActionRoutine(Action action, YieldInstruction yieldInstruction)
    {
        yield return yieldInstruction;
        action();
    }

    private static Coroutines _instance = null;
    private static Coroutines CreateComponent()
    {
        var globalObject = new GameObject("[Coroutines]");
        _instance = globalObject.AddComponent<Coroutines>();
        DontDestroyOnLoad(globalObject);
        return _instance;
    }
}
