using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenandClose : MonoBehaviour
{
    public GameObject RulesPanel;
    bool active;
    public void Open()
    {
        if (active == false)
        {
            RulesPanel.transform.RulesPanel.SetActive(true);
            active = true;
        }
    }
    public void Close()
    {
        if (active == true)
        {
            RulesPanel.transform.RulesPanel.SetActive(false);
            active = false;
        }
    }
}
