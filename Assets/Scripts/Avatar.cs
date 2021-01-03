using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    [SerializeField] private PlayerID player = default;

    private Text scoreText;
    
    private void Awake()
    {
        Image avatar = transform.Find("Avatar").GetComponent<Image>();
        avatar.sprite = AvatarManager.GetInstance().GetAvatar(player);

        scoreText = transform.Find("ScoreWrapper").Find("ScoreText").GetComponent<Text>();
        UpdateScore();
        
        ScoreManager.OnScoreChange += ScoreManagerOnScoreChange;
    }

    private void ScoreManagerOnScoreChange(object sender, EventArgs e)
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = ScoreManager.GetScore(player).ToString();
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
