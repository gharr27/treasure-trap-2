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

    public void OpenRule1()
    {
        RulePages[0].SetActive(true);
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(false);
    }

    public void OpenRule2()
    {
        RulePages[1].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[2].SetActive(false);
    }

    public void OpenRule3()
    {
        RulePages[2].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
    }
}
