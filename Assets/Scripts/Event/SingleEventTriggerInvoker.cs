using UnityEngine;
using UnityEngine.Events;

public class SingleEventTriggerInvoker : MonoBehaviour
{
    public UnityEvent eventToTrigger;

    public void Trigger()
    {
        eventToTrigger.Invoke();
    }
}

