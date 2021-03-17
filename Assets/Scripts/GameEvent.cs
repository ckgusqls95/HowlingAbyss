using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Object/GameEvent", order = int.MaxValue-5)]
[System.Serializable]
public class GameEvent : ScriptableObject
{
    private List<GameEventListner> listners = new List<GameEventListner>();

    public void Raise()
    {
        for(int index = listners.Count -1; index >=0; index --)
        {
            listners[index].OnEventRaised();
        }
    }
    public void RegisterListener(GameEventListner listener)
    {
        listners.Add(listener);
    }
    public void UnRegisterListener(GameEventListner listener)
    {
        listners.Remove(listener);
    }

}