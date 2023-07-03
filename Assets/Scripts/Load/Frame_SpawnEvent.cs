using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame_SpawnEvent : MonoBehaviour
{
    public GameObject[] Event;
    int x,y;
    public void SpawnEvent(int r)
    {
        GameObject blockEvent= Instantiate(Event[r], transform);
    }
    public void ChangeEvent()
    {
        Debug.Log("EventChanged!");
        GetComponentInParent<EventsLoadonMap>().ChangeEvent(x, y);
    }
}
