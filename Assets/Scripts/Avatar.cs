using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    [SerializeField] private PlayerID player = default;
    [SerializeField] private Text scoreText;
    [SerializeField] private Image avatar;
    
    private void Awake()
    {
        avatar.sprite = AvatarManager.GetInstance().GetAvatar(player);

        UpdateScore();
        
        ScoreManager.OnScoreChange += ScoreManagerOnScoreChange;
    }

    private void OnDestroy()
    {
        ScoreManager.OnScoreChange -= ScoreManagerOnScoreChange;
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
