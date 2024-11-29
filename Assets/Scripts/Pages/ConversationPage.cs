using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ConversationPage : MonoBehaviour
{
	[SerializeField] private ConversationData[] conversationDatas;

	[SerializeField] private GameObject conversationPrefab;
	[SerializeField] private GameObject npcMessagePrefab;
	[SerializeField] private GameObject playerMessagePrefab;

	private ScrollRect conversationListView;
	private ScrollRect chatListView;

	public Dictionary<string, MessageReplyRecord[]> ConversationHistory = new Dictionary<string, MessageReplyRecord[]>();

	private void Awake()
	{
		conversationListView = GameObject.Find("ConversationList").GetComponent<ScrollRect>();
		chatListView = GameObject.Find("ChatList").GetComponent<ScrollRect>();
	}

	private void Start()
	{
		RefreshConversationList();
	}

	private void RefreshConversationList()
	{
		Array.Sort(conversationDatas, (a, b) => a.priority.CompareTo(b.priority));

		foreach(var data in conversationDatas)
		{
			GameObject obj = Instantiate(conversationPrefab, conversationListView.content);
			ConversationItem conversation = obj.GetComponent<ConversationItem>();
			conversation.SetData(data);
			conversation.OnConversationSelectedEvent += OnConversationSelected;
		}
	}

	private void OnConversationSelected(string conversationId)
	{
		ConversationData conversation = Array.Find(conversationDatas, item => item.id == conversationId);
		Debug.Log("OnConversationSelected " + conversation.character.displayName);

		ShowConversationHistory(conversationId);

		StartCoroutine(ShowConversationNewMessages(conversationId, 0));
	}

	private void ShowConversationHistory(string conversationId)
	{
		ConversationData conversation = Array.Find(conversationDatas, item => item.id == conversationId);
		if(ConversationHistory.ContainsKey(conversationId))
		{
			MessageReplyRecord[] replyRecords = ConversationHistory[conversationId];
			foreach(var record in replyRecords)
			{
				Message npcMsg = conversation.messages[record.messageId];
				GameObject obj = Instantiate(npcMessagePrefab, chatListView.content);
				obj.GetComponent<MessageItem>().SetData(conversation.character.displayName, npcMsg.body, true);

				if(record.replyAction >= 0)
				{
					MessageAction action = npcMsg.actions[record.replyAction];
					GameObject replyObj = Instantiate(playerMessagePrefab, chatListView.content);
					replyObj.GetComponent<MessageItem>().SetData("You", action.playerReply, true);
				}
			}
		}
	}

	private IEnumerator ShowConversationNewMessages(string conversationId, int startId)
	{
		ConversationData conversation = Array.Find(conversationDatas, item => item.id == conversationId);

		int nextMessageId = startId;
		while(nextMessageId >= 0 && nextMessageId < conversation.messages.Length)
		{
			Message npcMsg = conversation.messages[nextMessageId];
			GameObject obj = Instantiate(npcMessagePrefab, chatListView.content);
			MessageItem messageItem = obj.GetComponent<MessageItem>();
			messageItem.SetData(conversation.character.displayName, npcMsg.body);

			Canvas.ForceUpdateCanvases();
			chatListView.verticalNormalizedPosition = 0f;
			Canvas.ForceUpdateCanvases();

			while(messageItem.TextRevealed() == false)
			{
				yield return null;
			}

			if(npcMsg.isLastMessage)
			{
				yield break;
			}

			if(npcMsg.actions.Length < 3)
			{
				nextMessageId++;
			}
			else
			{
				yield break; // waiting for player input
			}
		}
	}
}
