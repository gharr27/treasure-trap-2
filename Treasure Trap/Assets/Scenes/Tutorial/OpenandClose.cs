using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenandClose : MonoBehaviour
{
    public GameObject RulesPanel;
    public GameObject []RulePages;
    bool active;
    void Start()
    {
        RulesPanel.SetActive(false);
    }

    public void Open()
    {
        if (active == false)
        {
            RulesPanel.SetActive(true);
            active = true;
        }
    }
    public void Close()
    {
        if (active == true)
        {
            RulesPanel.SetActive(false);
            active = false;
        }
    }

    public void OpenRules()
    {
        RulePages[0].SetActive(true);
    }

    public void OpenRule1()
    {
        RulePages[1].SetActive(true);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
    }

    public void OpenRule2()
    {
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(true);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
    }

    public void OpenRule3()
    {
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(true);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
    }

    public void OpenRule4()
    {
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(true);
        RulePages[5].SetActive(false);
    }

    public void OpenRule5()
    {
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(true);
    }

    public void OpenRules2()
    {
        RulePages[0].SetActive(true);
    }

    public void OpenRule01()
    {
        RulePages[1].SetActive(true);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
    }

    public void OpenRule02()
    {
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(true);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
    }

    public void OpenRule03()
    {
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(true);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
    }

    public void OpenRule04()
    {
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(true);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
    }

    public void OpenRule05()
    {
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(true);
        RulePages[6].SetActive(false);
    }

    public void OpenRule06()
    {
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[5].SetActive(true);
    }
 
}
