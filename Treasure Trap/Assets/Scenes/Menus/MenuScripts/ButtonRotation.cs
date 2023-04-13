using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonRotation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float rotationAngle = 10f;
    [SerializeField] private float rotationDuration = 0.25f;
    private Quaternion initialRotation;
    private bool isRotating;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isRotating)
        {
            StartCoroutine(RotateButton(transform.rotation, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, rotationAngle)), rotationDuration));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isRotating)
        {
            StartCoroutine(RotateButton(transform.rotation, initialRotation, rotationDuration));
        }
    }

    private IEnumerator RotateButton(Quaternion startRotation, Quaternion targetRotation, float duration)
    {
        isRotating = true;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }
}
