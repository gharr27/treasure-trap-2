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
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule2()
    {
        RulePages[1].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule3()
    {
        RulePages[2].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule4()
    {
        RulePages[3].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule5()
    {
        RulePages[4].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule6()
    {
        RulePages[5].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule7()
    {
        RulePages[6].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule8()
    {
        RulePages[7].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule9()
    {
        RulePages[8].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule10()
    {
        RulePages[9].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[2].SetActive(false);
        RulePages[10].SetActive(false);
    }

    public void OpenRule11()
    {
        RulePages[10].SetActive(true);
        RulePages[0].SetActive(false);
        RulePages[1].SetActive(false);
        RulePages[3].SetActive(false);
        RulePages[4].SetActive(false);
        RulePages[5].SetActive(false);
        RulePages[6].SetActive(false);
        RulePages[7].SetActive(false);
        RulePages[8].SetActive(false);
        RulePages[9].SetActive(false);
        RulePages[2].SetActive(false);
    }
}
