using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class PlayerButton : MonoBehaviour
{
    private static String UNSELECTED_COLOR = "FAF2D6";
    private static String SELECTED_COLOR = "FFDF64";
    
    private Image background;
    private Text playerText;
    private bool isSelected;
    private Animator anim;

    public Sprite selectedSprite;
    public Sprite unselectedSprite;

    private void Awake()
    {
        isSelected = false;
        background = transform.Find("Background").GetComponent<Image>();
        playerText = transform.Find("Text").GetComponent<Text>();
        playerText.color = UtilsClass.GetColorFromString(UNSELECTED_COLOR);
        anim = transform.GetComponent<Animator>();
    }

    public void Select()
    {
        isSelected = true;
        background.sprite = selectedSprite;
        playerText.color = UtilsClass.GetColorFromString(SELECTED_COLOR);
    }

    public void Unselect()
    {
        isSelected = false;
        background.sprite = unselectedSprite;
        playerText.color = UtilsClass.GetColorFromString(UNSELECTED_COLOR);
    }

    public void Click()
    {
        anim.SetTrigger("NbPlayerButtonClick");
    }
}
