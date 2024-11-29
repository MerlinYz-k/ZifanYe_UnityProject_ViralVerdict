using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    public RectTransform rectTransform;
    private void Start()
    {
        Vector2 mousePosition = Input.mousePosition;

        // ��UI��ê������Ϊ��Ļ���½�
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        //rectTransform.pivot = Vector2.one * 0.5f; // ʹUI�����ĵ�λ�����λ��

        // ����UI��λ��Ϊ���λ��
        rectTransform.position = mousePosition;
    }

    public void Hide()
    {
        Destroy(gameObject);
    }
}
