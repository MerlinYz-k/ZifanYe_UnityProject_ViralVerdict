using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplyToLine : ReplyLine
{
    public TMP_Text centerText;
    public TMP_Text replyToNameText;

    public override void Init(string postId, CommentReply reply, bool isPostSender, bool isToPostSender)
    {
        base.Init(postId, reply, isPostSender, isToPostSender);
        if (reply.IsBaned)
        {
            Baned();
            return;
        }
        string toName = reply.to.name;
        if (isToPostSender)
        {
            replyToNameText.color = senderColor;
        }
        replyToNameText.text = toName + ":";
    }

    public override void Baned()
    {
        if (reply.canNotBan) return;
        base.Baned();
        centerText.text = string.Empty;
        replyToNameText.text = string.Empty;
    }
}
