using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    [SerializeField] private PlayerID player;

    private Text scoreText;
    
    private void Awake()
    {
        Image avatar = transform.Find("Avatar").GetComponent<Image>();
        avatar.sprite = AvatarManager.GetInstance().GetAvatar(player);

        scoreText = transform.Find("ScoreWrapper").Find("ScoreText").GetComponent<Text>();
        UpdateScore();
        
        ScoreManager.GetInstance().OnScoreChange += ScoreManagerOnOnScoreChange;
    }

    private void ScoreManagerOnOnScoreChange(object sender, EventArgs e)
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = ScoreManager.GetInstance().GetScore(player).ToString();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
