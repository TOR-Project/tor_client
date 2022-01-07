using UnityEngine;
using UnityEngine.Events;

public class DualEventTriggerInvoker : MonoBehaviour
{
    public UnityEvent firstToTrigger;
    public UnityEvent secondToTrigger;

    public void FirstTrigger()
    {
        firstToTrigger.Invoke();
    }

    public void SecondTrigger()
    {
        secondToTrigger.Invoke();
    }
}

