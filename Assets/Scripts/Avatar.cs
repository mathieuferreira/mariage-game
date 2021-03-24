using System;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    private static float ALPHA_SPEED = 2f;
    
    [SerializeField] private PlayerID player = default;
    [SerializeField] private Text scoreText;
    [SerializeField] private Image avatar;
    [SerializeField] private Image border;
    [SerializeField] private Image scoreImage;

    private float currentAlpha = 1f;
    private float targetAlpha = 1f;
    
    private void Awake()
    {
        avatar.sprite = AvatarManager.GetInstance().GetAvatar(player);
        
        UpdateScore();
        
        ScoreManager.OnScoreChange += ScoreManagerOnScoreChange;
    }

    private void FixedUpdate()
    {
        if (Math.Abs(currentAlpha - targetAlpha) < .01f)
            return;

        float alphaDirection = targetAlpha - currentAlpha;
        float alphaDirectionNormalized = alphaDirection / Mathf.Abs(alphaDirection);
        float deltaAlpha = alphaDirectionNormalized * ALPHA_SPEED * Time.fixedDeltaTime;

        if (Mathf.Abs(deltaAlpha) > Mathf.Abs(alphaDirection))
            deltaAlpha = alphaDirection;
        
        currentAlpha += deltaAlpha;
        SetAlphaForImage(avatar, currentAlpha);
        SetAlphaForImage(border, currentAlpha);
        SetAlphaForImage(scoreImage, currentAlpha);
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

    public void SetAlpha(float alpha)
    {
        targetAlpha = alpha;
    }

    private void SetAlphaForImage(Image image, float alpha)
    {
        Color initialColor = image.color;
        initialColor.a = alpha;
        image.color = initialColor;
    }
}
