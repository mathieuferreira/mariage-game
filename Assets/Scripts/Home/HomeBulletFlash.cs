using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBulletFlash : MonoBehaviour
{
    private const float InitialScale = 2f;
    private const float MaxTimer = .2f;
    private const float RotationSpeed = 20f;

    private float timer;
    private CubicBezierCurve cubic;
    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    
    private void Awake()
    {
        cubic = new CubicBezierCurve(0f, .3f, .4f , 1f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            gameObject.SetActive(false);
            return;
        }
        
        transform.localEulerAngles += new Vector3(0f, 0f, 360f * RotationSpeed * Time.deltaTime);
        float factor = 1 - cubic.Ease(MaxTimer - timer, 0f, 1f, MaxTimer);
        transform.localScale = new Vector3(1f, 1f, 0f) * (InitialScale * factor);
        initialColor.a = factor;
        spriteRenderer.color = initialColor;
    }

    public void Start()
    {
        transform.localScale = new Vector3(1f, 1f, 0f) * InitialScale;
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        timer = MaxTimer;
        gameObject.SetActive(true);
    }
}
