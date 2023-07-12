using UnityEngine;

public class Victory : MonoBehaviour
{
    public GameObject BattleCanvas,VictoryCanvas,Events;
    
    
    public void MoveAwayCanvas()
    {
        BattleCanvas.transform.position = new Vector3(-20f, 0, 0);
        VictoryCanvas.transform.position = new Vector3(10f, 10f, 0);
        Events.GetComponent<EventsLoadonMap>().SetGameMode(3);
    }

}
