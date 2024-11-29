using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PostDataManager : MonoBehaviour
{
	public static PostDataManager Instance { get; private set; }
	private Dictionary<string, PostData> allPostData = new Dictionary<string, PostData>();

	private void Awake()
    {
        Instance = this;
    }

	private void Start()
	{
        PostData[] rawData = Resources.LoadAll<PostData>("Data/Post");
		foreach(var obj in rawData)
		{
			PostData postData = (PostData)obj;
			Debug.Log("Loaded Post Data " + postData.name);
			allPostData.Add(postData.id, postData);
		}
	}

	public PostData FindPostById(string postId)
	{
		if(allPostData.ContainsKey(postId))
		{
			return allPostData[postId];
		}
		return null;
	}

	public bool FindCommentOrReply(string postId, string commentOrReplyId, out Comment comment, out CommentReply reply)
	{
		comment = null;
		reply = null;

		PostData postData = FindPostById(postId);
		if(postData == null)
		{
			return false;
		}

		if(!commentOrReplyId.Contains("_reply_"))
		{
			comment = postData.comments.First(c => c.id == commentOrReplyId);
		}
		else
		{
			foreach(var obj in postData.comments)
			{
                reply = obj.replys.First(r => r.id == commentOrReplyId);
				if(reply != null)
				{
					break;
				}
			}
		}

		return comment != null || reply != null;
	}
}
