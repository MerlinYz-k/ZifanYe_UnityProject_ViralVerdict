using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationItem : MonoBehaviour
{
	private TMP_Text displayName;
	private TMP_Text jobTitle;
	private Image avatar;
	private GameObject status;

	private Button background;
	private string conversationId;

	public delegate void OnConversationSelectedDelegate(string id);

	public event OnConversationSelectedDelegate OnConversationSelectedEvent;

	private void Awake()
	{
		displayName = GameObject.Find("Name").GetComponent<TMP_Text>();
		jobTitle = GameObject.Find("Title").GetComponent<TMP_Text>();
		avatar = GameObject.Find("Avatar").GetComponent<Image>();
		status = GameObject.Find("Circle");
		background = GameObject.Find("Background").GetComponent<Button>();

		background.onClick.AddListener(OnButtonClicked);
	}

	public void SetData(ConversationData data)
	{
		displayName.text = data.character.displayName;
		jobTitle.text = data.character.jobTitle;
		avatar.sprite = data.character.avatar;
		status.SetActive(true);
		conversationId = data.id;
	}

	private void OnButtonClicked()
	{
		status.SetActive(false);
		if(OnConversationSelectedEvent != null)
		{
			OnConversationSelectedEvent.Invoke(conversationId);
		}
	}
}
