using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public enum ActionType
{
    Ban,
    Promote,
    None
}

[Serializable]
public class ActionInfo
{
    public ActionType action = ActionType.None;
    public float rewards;
}

[Serializable]
public class CommentReply
{
	[ReadOnly] public string id;
    public CharacterData from;
	public CharacterData to;
    public string time;
    public string message;
    public ActionInfo actionInfo;
    public bool canNotBan = true;

    private bool isBaned = false;
    public bool IsBaned { get => isBaned; set => isBaned = value; }
}

[Serializable]
public class Comment
{
	[ReadOnly] public string id;
    public CharacterData author;
    public string time;
    public string content;
    public CommentReply[] replys;
    public ActionInfo actionInfo;
    public bool canNotBan = true;

    private bool isBaned = false;
    public bool IsBaned { get => isBaned; set => isBaned = value; }
}

[CreateAssetMenu(fileName = "DefaultPostData", menuName = "BeiFenVV/PostData", order = 3)]
public class PostData : ScriptableObject
{
	[ReadOnly] public string id;
    [ReadOnly] public string postName;
    public CharacterData character;
	public string postTitle;
	public string postContent;
    public Sprite sprite;
    public string postTime;
	public Comment[] comments;
    public int hotNum;
    public bool isViewed { get; set; } = false;

    void OnValidate()
	{
		if(string.IsNullOrEmpty(id))
		{
			id = Guid.NewGuid().ToString();
        }

        postName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));

        for (int idx = 0; idx < comments.Length; ++idx)
        {
            comments[idx].id = "comment_" + idx.ToString();
            for (int id = 0; id < comments[idx].replys.Length; ++id)
            {
                comments[idx].replys[id].id = comments[idx].id + "_reply_" + id.ToString();
            }
        }
    }
}