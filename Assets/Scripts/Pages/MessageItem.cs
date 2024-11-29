using TMPro;
using UnityEngine;

public class MessageItem : MonoBehaviour
{
	[SerializeField]
	private TMP_Text senderName;

	[SerializeField]
	private TypewriterEffect msgText;

	private bool bTextRevealed = false;

	private void Awake()
	{
		// senderName = GameObject.Find("SenderName").GetComponent<TMP_Text>();
		// msgText = GetComponentInChildren<TypewriterEffect>();
	}

	public void SetData(string sender, string msg, bool skip = false)
	{
		Debug.Log("set data " + sender);
		senderName.text = sender;
		msgText.SetText(msg, skip);
		if(skip)
		{
			CompleteTextRevealed();
		}
		else
		{
			TypewriterEffect.CompleteTextRevealed += CompleteTextRevealed;
		}
	}

	void CompleteTextRevealed()
	{
		bTextRevealed = true;
		TypewriterEffect.CompleteTextRevealed -= CompleteTextRevealed;
	}

	public bool TextRevealed()
	{
		return bTextRevealed;
	}
}
