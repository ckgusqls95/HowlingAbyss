using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListner : MonoBehaviour
{
    public GameEvent EventSO;
    public UnityEvent Response;

    private void OnEnable()
    {
        EventSO.RegisterListener(this);
    }

    private void OnDisable()
    {
        EventSO.UnRegisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
