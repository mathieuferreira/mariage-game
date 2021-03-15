using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    private static String UNSELECTED_COLOR = "FAF2D6";
    private static String SELECTED_COLOR = "FFDF64";
    
    private Image background;
    private Text playerText;
    private Animator anim;

    public Sprite selectedSprite;
    public Sprite unselectedSprite;
    private static readonly int NbPlayerButtonClick = Animator.StringToHash("NbPlayerButtonClick");

    private void Awake()
    {
        background = transform.Find("Background").GetComponent<Image>();
        playerText = transform.Find("Text").GetComponent<Text>();
        playerText.color = Utils.GetColorFromString(UNSELECTED_COLOR);
        anim = transform.GetComponent<Animator>();
    }

    public void Select()
    {
        background.sprite = selectedSprite;
        playerText.color = Utils.GetColorFromString(SELECTED_COLOR);
    }

    public void Unselect()
    {
        background.sprite = unselectedSprite;
        playerText.color = Utils.GetColorFromString(UNSELECTED_COLOR);
    }

    public void Click()
    {
        anim.SetTrigger(NbPlayerButtonClick);
    }
}
