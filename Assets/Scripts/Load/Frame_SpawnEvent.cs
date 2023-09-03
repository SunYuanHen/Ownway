using UnityEngine;

public class Frame_SpawnEvent : MonoBehaviour
{
    public GameObject[] Event;
    int x,y;
    public void SpawnEvent(int r)
    {
        GameObject blockEvent= Instantiate(Event[r], transform);
    }
    public void ClearEvent()
    {
        Debug.Log("EventChanged!");
        GetComponentInParent<EventsLoadonMap>().ClearEvent(x, y);
    }
}
