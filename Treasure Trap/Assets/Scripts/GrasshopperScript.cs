using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrasshopperScript : MonoBehaviour
{
    static int idNum = 1;
    public string id;

    // Start is called before the first frame update
    void Start()
    {
        id = id + idNum;
        idNum++;
        //Debug.Log(id);
    }

    public void SetId(string newId) {
        id = newId;
    }

    public string GetId() {
        return id;
    }
}