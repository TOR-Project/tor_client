using UnityEngine;
using UnityEngine.Events;

public class QuaterEventTriggerInvoker : MonoBehaviour
{
    public UnityEvent firstToTrigger;
    public UnityEvent secondToTrigger;
    public UnityEvent thirdToTrigger;
    public UnityEvent fourthToTrigger;

    public void FirstTrigger()
    {
        firstToTrigger.Invoke();
    }

    public void SecondTrigger()
    {
        secondToTrigger.Invoke();
    }

    public void thirdTrigger()
    {
        thirdToTrigger.Invoke();
    }

    public void FourthTrigger()
    {
        fourthToTrigger.Invoke();
    }
}

