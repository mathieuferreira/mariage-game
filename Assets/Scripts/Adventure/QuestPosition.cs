using System;
using UnityEngine;

public class QuestPosition : MonoBehaviour
{
    public event EventHandler<QuestCompleteEvent> questComplete;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        
        if (other.CompareTag("Player"))
        {
            AdventurePlayer player = other.GetComponent<AdventurePlayer>();
            
            if (questComplete != null)
                questComplete(this, new QuestCompleteEvent() { player = player });
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

public class QuestCompleteEvent : EventArgs
{
    public AdventurePlayer player;
}
