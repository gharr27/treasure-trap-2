using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;

    public void SwitchObjects()
    {
        objectA.SetActive(!objectA.activeSelf);
        objectB.SetActive(!objectB.activeSelf);
    }
}
