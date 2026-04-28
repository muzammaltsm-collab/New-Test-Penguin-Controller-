using UnityEngine;

public class CustomEventSender : MonoBehaviour
{
    public void CustomEventName(string eventName)
    {
        SessionTime.instance.CustomEvent(eventName);
    }

}
