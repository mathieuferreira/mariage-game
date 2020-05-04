using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBulletLaunch : MonoBehaviour
{
    private const float InitialScale = 4f;
    private const float MaxTimer = .1f;
    private const float RotationSpeed = 20f;

    private float timer;
    private CubicBezierCurve cubic;
    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    
    private void Awake()
    {
        transform.localScale = new Vector3(1f, 1f, 0f) * InitialScale;
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        timer = MaxTimer;
        cubic = new CubicBezierCurve(0f, .3f, .4f , 1f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.localEulerAngles += new Vector3(0f, 0f, 360f * RotationSpeed * Time.deltaTime);
        float factor = 1 - cubic.Ease(MaxTimer - timer, 0f, 1f, MaxTimer);
        transform.localScale = new Vector3(1f, 1f, 0f) * InitialScale * factor;
        initialColor.a = factor;
        spriteRenderer.color = initialColor;
    }
}
