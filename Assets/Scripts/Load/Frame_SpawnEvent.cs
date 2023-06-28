using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame_SpawnEvent : MonoBehaviour
{
    public GameObject[] Event;
    public void SpawnEvent(int r)
    {
        GameObject blockEvent= Instantiate(Event[r], transform);
    }
}
