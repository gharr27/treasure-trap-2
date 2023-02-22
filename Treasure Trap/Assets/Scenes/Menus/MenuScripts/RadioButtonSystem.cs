using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RadioButtonSystem : MonoBehaviour
{
    ToggleGroup toggleGroup;
    public string what;

    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    void Update()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        PlayerPrefs.SetString(what, toggle.name + " _ " + toggle.GetComponentInChildren<Text>().text);
    }

    public void Submit()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        Debug.Log(toggle.name + " _ " + toggle.GetComponentInChildren<Text>().text);
        PlayerPrefs.SetString(what, toggle.name + " _ " + toggle.GetComponentInChildren<Text>().text);
    }
}