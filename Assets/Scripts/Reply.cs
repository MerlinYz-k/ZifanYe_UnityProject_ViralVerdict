using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reply
{
    public string id;
    public Sprite head;
    public string senderName;
    public string time;
    public string replyContent;
    public List<Reply> replies;
}
