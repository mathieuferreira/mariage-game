using UnityEngine;

public class Tutorial : MonoBehaviour
{
	private bool complete;
	
    public void Hide()
    {
	    if (complete)
		    return;
	    
	    gameObject.SetActive(false);
    }
	
    public void Show()
    {
	    if (complete)
		    return;
	    
	    gameObject.SetActive(true);
    }

    public void Complete()
    {
	    Hide();
	    complete = true;
    }
}
