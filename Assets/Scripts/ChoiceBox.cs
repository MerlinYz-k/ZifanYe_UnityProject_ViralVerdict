using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    public RectTransform rectTransform;
    private void Start()
    {
        Vector2 mousePosition = Input.mousePosition;

        // 将UI的锚点设置为屏幕左下角
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        //rectTransform.pivot = Vector2.one * 0.5f; // 使UI的中心点位于鼠标位置

        // 设置UI的位置为鼠标位置
        rectTransform.position = mousePosition;
    }

    public void Hide()
    {
        Destroy(gameObject);
    }
}
