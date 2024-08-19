using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;

    [SerializeField] private bool FadeIn = false;

    [SerializeField] private bool FadeOut = false;

    public void Setup()
    {
        myUIGroup.alpha = 0.0f;
    }

    public void ShowUI()
    {
        StartCoroutine(DelayedShowUI());
    }

    private IEnumerator DelayedShowUI()
    {
        yield return new WaitForSeconds(3f);
        FadeIn = true;
    }

    public void HideUI()
    {
        FadeOut = true;
    }

    public void Update()
    {
        if (FadeIn)
        {
            if (myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime/5;
                if (myUIGroup.alpha > 1)
                {
                    myUIGroup.alpha = 1; 
                    FadeIn = false;
                }
            }
        }

        if (FadeOut)
        {
            if (myUIGroup.alpha > 0) 
            {
                myUIGroup.alpha -= Time.deltaTime/5;
                if (myUIGroup.alpha < 0)
                {
                    myUIGroup.alpha = 0;
                    FadeOut = false;
                }
            }
        }
    }

    void Start()
    {
        Setup();
        ShowUI();
    }
}
