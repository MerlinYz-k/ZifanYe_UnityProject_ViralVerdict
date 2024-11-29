using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class ReplyBox : MonoBehaviour
{
    public string parentPostId;
    public Image head;
    public TMP_Text senderNameText;
    public TMP_Text timeText;
    public TMP_Text replyContentText;
    public Transform replyContent;
    public Comment comment;

    private void Start()
    {
        head.sprite = comment.author.avatar;
        senderNameText.text = comment.author.displayName;
        timeText.text = comment.time;
        replyContentText.text = comment.content;

        replyContentText.ForceMeshUpdate();
        int line = replyContentText.textInfo.lineCount;
        RectTransform rect = transform.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 400f + line * 25);

        if (comment.IsBaned)
        {
            Baned();
            return;
        }
    }

    public void ShowChoiceBox()
    {
        //if (comment.canNotBan) return;
        //Instantiate(Resources.Load<GameObject>("Prefabs/ChoiceBox"), GameObject.Find("Within Post").transform);
        PlayerController.Instance.ShowActionBox(parentPostId, this);
    }

    public void Promote(float value)
    {
        if (value > 0)
        {
            replyContentText.color = new(88f / 255f, 166f / 255f, 92f / 255f);
        }
    }

    public void Baned()
    {
        if (comment.canNotBan) return;
        replyContentText.text = "This reply has be baned!";
    }
}
