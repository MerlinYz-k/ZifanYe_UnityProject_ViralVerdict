using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class ReplyLine : MonoBehaviour
{
    public string parentPostId;
    public Image head;
    public TMP_Text senderNameText;
    public TMP_Text replyContentText;
    public CommentReply reply;

    protected Color senderColor = new(0.9490197f, 0.6862745f, 0.372549f);

    public virtual void Init(string postId, CommentReply reply, bool isPostSender, bool isToPostSender)
    {
        parentPostId = postId;
        this.reply = reply;
        head.sprite = reply.to.avatar;
        string senderName = reply.to.displayName;
        if (isPostSender)
        {
            senderNameText.color = senderColor;
        }
        senderNameText.text = senderName + ":";
        replyContentText.text = reply.message;

        replyContentText.ForceMeshUpdate();
        int line = replyContentText.textInfo.lineCount;
        RectTransform rect = transform.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 25f + line * 25);

        if (reply.IsBaned)
        {
            Baned();
            return;
        }
    }

    public void ShowChoiceBox()
    {
        //Instantiate(Resources.Load<GameObject>("Prefabs/ChoiceBox"), GameObject.Find("Within Post").transform);
        PlayerController.Instance.ShowActionBox(parentPostId, this);
    }

    public virtual void Baned()
    {
        if (reply.canNotBan) return;
        senderNameText.color = Color.white;
        senderNameText.text = "This reply has be baned!";
        replyContentText.text = string.Empty;
    }

    public void Promote(float value)
    {
        if (value > 0)
        {
            replyContentText.color = new(88f / 255f, 166f / 255f, 92f / 255f);
        }
    }
}
