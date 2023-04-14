using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonRotation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum RotationDirection
    {
        Left,
        Right
    }

    [SerializeField] private float rotationAngle = 10f;
    [SerializeField] private float rotationDuration = 0.25f;
    [SerializeField] private RotationDirection rotationDirection = RotationDirection.Left;
    private Quaternion initialRotation;
    private Coroutine rotationCoroutine;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        float signedAngle = rotationDirection == RotationDirection.Left ? -rotationAngle : rotationAngle;
        rotationCoroutine = StartCoroutine(RotateButton(transform.rotation, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, signedAngle)), rotationDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        rotationCoroutine = StartCoroutine(RotateButton(transform.rotation, initialRotation, rotationDuration));
    }

    private IEnumerator RotateButton(Quaternion startRotation, Quaternion targetRotation, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
