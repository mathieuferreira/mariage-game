using System;
using CodeMonkey;
using UnityEngine;

public class QuestPosition : MonoBehaviour
{
    public event EventHandler<QuestCompleteEvent> questComplete;

    private float timer;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        BaseRPGPlayer player = other.GetComponent<BaseRPGPlayer>();
        if (player != null && UserInput.isKeyDown(player.GetPlayerId(), UserInput.Key.Action))
        {
            CMDebug.TextPopup("Click", player.GetPosition());
            
            if (questComplete != null)
                questComplete(this, new QuestCompleteEvent() { player = player });
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseRPGPlayer player = other.GetComponent<BaseRPGPlayer>();
        if (player != null)
        {
            player.ShowAdviceButton();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        BaseRPGPlayer player = other.GetComponent<BaseRPGPlayer>();
        if (player != null)
        {
            player.HideAdviceButton();
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

public class QuestCompleteEvent : EventArgs
{
    public BaseRPGPlayer player;
}
