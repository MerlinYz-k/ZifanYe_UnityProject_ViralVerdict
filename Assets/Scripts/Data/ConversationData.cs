using System;
using UnityEngine;

[Serializable]
public struct MessageAction
{
	public string actionName;
	public string playerReply;
	public int nextMessageId;
}

[Serializable]
public struct Message
{
	[ReadOnly] public int id;

	public string body;
	public string time;
	public ESender sender;
	public MessageAction[] actions;
	public bool isLastMessage;
}

[CreateAssetMenu(fileName = "DefaultConversationData", menuName = "BeiFenVV/ConversationData", order = 2)]
public class ConversationData : ScriptableObject
{
	[ReadOnly] public string id;
	public CharacterData character;
	public int priority;
	public Message[] messages;

	void OnValidate()
	{
		if(string.IsNullOrEmpty(id))
		{
			id = Guid.NewGuid().ToString();
		}

		for(int i = 0; i < messages.Length; ++i)
		{
			messages[i].id = i;
		}
	}
}

[Serializable]
public struct MessageReplyRecord
{
	public int messageId;
	public int replyAction; // 0-Kind
}

public enum ESender
{
    Other,
	You
}