using UnityEngine;
using UnityEngine.Events;

public class OnEnableOnDisableEvent : MonoBehaviour
{
    [Header("On Enable")]
    public UnityEvent onEnableEvent;

    [Header("On Disable")]
    public UnityEvent onDisableEvent;

    private void OnEnable()
    {
        onEnableEvent?.Invoke();
    }

    private void OnDisable()
    {
        onDisableEvent?.Invoke();
    }
}
