using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private string goText = "GO !";

    private static CountDown instance;

    public static CountDown GetInstance()
    {
        return instance;
    }
    
    private float timer;
    private int currentLabel;
    private Action callback;
    private bool active;
    private Transform baseTransform;
    private List<CountDownLabel> labelList;

    private void Awake()
    {
        gameObject.SetActive(false);
        active = false;
        baseTransform = transform.Find("BaseLabel");
        instance = this;
    }

    public void StartCounter(int counter, Action afterAction)
    {
        if (active)
            return;

        gameObject.SetActive(true);
        callback = afterAction;
        currentLabel = 0;
        timer = 0f;
        active = true;
        
        labelList = new List<CountDownLabel>();
        for (int i = counter; i > 0; i--)
        {
            CreateLabel(i.ToString());
        }

        CountDownLabel lastlabel = CreateLabel(goText);
        lastlabel.OnAnimationEnd += OnCountDownEnd;
        DisplayCurrentLabel();
    }

    public void OnCountDownEnd(object sender, EventArgs args)
    {
        active = false;
        gameObject.SetActive(false);
        callback();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!active)
            return;

        timer += Time.deltaTime;

        if (timer < 1f)
            return;

        timer -= 1f;

        if (currentLabel >= labelList.Count - 1)
            return;
        
        currentLabel++;
        DisplayCurrentLabel();
    }

    private CountDownLabel CreateLabel(string label)
    {
        Transform newTransform = Instantiate(baseTransform, transform);
        CountDownLabel newLabel = newTransform.GetComponent<CountDownLabel>();
        newLabel.Init(label);
        labelList.Add(newLabel);
        return newLabel;
    }

    private void DisplayCurrentLabel()
    {
        labelList[currentLabel].Show();
    }
}
