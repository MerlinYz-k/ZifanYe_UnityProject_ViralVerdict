using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserBox : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text userTypeText;
    public TMP_Text nameText;
    public TMP_Text informationText;

    public ConversationData conversationData;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MessagePageController.Instance.currentUser != null) 
        {
            MessagePageController.Instance.currentUser.GetComponent<Image>().color = new(0, 0, 0, 0f);
        }
        GetComponent<Image>().color = new(0, 0, 0, 0.3f);
        MessagePageController.Instance.currentUser = this;
        MessagePageController.Instance.LoadChat(conversationData);
    }
}
