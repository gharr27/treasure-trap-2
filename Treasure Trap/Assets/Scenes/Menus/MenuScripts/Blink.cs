using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public Image image;

    void Start()
    {
        InvokeRepeating("ToggleImage", 0.5f, 0.5f);
    }

    void ToggleImage()
    {
        image.enabled = !image.enabled;
    }
}
