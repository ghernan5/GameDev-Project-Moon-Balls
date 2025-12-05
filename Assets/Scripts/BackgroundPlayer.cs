using UnityEngine;

public class UIFloatAndDrift : MonoBehaviour
{
    public float driftSpeed = 20f;
    public float resetX = -200f;
    public float startX = 1200f;
    public float floatAmplitude = 10f; 
    public float floatSpeed = 1f;
    public float rotateAmplitude = 3f;
    public float rotateSpeed = 0.5f;

    private RectTransform rect;
    private Vector2 startPos;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        rect.anchoredPosition += Vector2.left * driftSpeed * delta;

        if (rect.anchoredPosition.x < resetX)
        {
            rect.anchoredPosition = new Vector2(startX, rect.anchoredPosition.y);
            startPos = rect.anchoredPosition;  // reset float anchor
        }

        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        rect.anchoredPosition = new Vector2(
            rect.anchoredPosition.x,
            startPos.y + floatOffset
        );

        float rot = Mathf.Sin(Time.time * rotateSpeed) * rotateAmplitude;
        rect.localRotation = Quaternion.Euler(0, 0, rot);
    }
}
