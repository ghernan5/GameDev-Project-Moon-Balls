using UnityEngine;
using UnityEngine.UI;

public class StarScroll : MonoBehaviour
{
    public RawImage image;
    public float scrollX = 0.005f;
    public float scrollY = 0.001f;

    void Update()
    {
        image.uvRect = new Rect(
            image.uvRect.x + scrollX * Time.deltaTime,
            image.uvRect.y + scrollY * Time.deltaTime,
            image.uvRect.width,
            image.uvRect.height
        );
    }
}
