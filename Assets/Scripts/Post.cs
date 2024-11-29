using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Post
{
    public string id;
    public Sprite head;
    public string title;
    public string senderName;
    public string time;
    public string postContent;
    public List<Reply> replies;
    public string hotNum;
}
