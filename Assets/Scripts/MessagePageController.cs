using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePageController : MonoBehaviour
{
    public static MessagePageController Instance;

    //public List<oldMessage> allMessages = new();
    public ConversationDatas conversationDatas;
    public ConversationData currentConversation;
    public GameObject choiceBox;
    public ScrollRect usersView;
    public ScrollRect messagesView;
    public UserBox currentUser;

    private GameObject messageBoxPrefab;
    private GameObject messageBoxSelfPrefab;
    private GameObject canNotDoThisPrefab;

    private GameObject userBoxPrefab;

    [SerializeField]private bool canPsychologicalCounsel = false;
    [SerializeField]private bool canReport = false;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadPrivateMessage(ConversationDatas conversationDatas)
    {
        currentUser = null;
        userBoxPrefab = Resources.Load<GameObject>("Prefabs/UserBox");
        //usersView = GameObject.Find("UsersView").GetComponent<ScrollRect>();
        for (int i = 0; i < usersView.content.childCount; i++)
        {
            Destroy(usersView.content.GetChild(i).gameObject);
        }
        for (int i = 0; i < conversationDatas.datas.Length; i++)
        {
            GameObject uBox = Instantiate(userBoxPrefab, usersView.content);
            UserBox userBox = uBox.GetComponent<UserBox>();
            switch (conversationDatas.datas[i].character.userType)
            {
                case EUserType.NormalUsers:
                    userBox.userTypeText.text = "Normal Users";
                    break;
                case EUserType.SeniorModerator:
                    userBox.userTypeText.text = "Senior Moderator";
                    break;
                default:
                    break;
            }
            userBox.nameText.text = conversationDatas.datas[i].character.displayName;
            userBox.informationText.text = conversationDatas.datas[i].messages[^1].body;
            userBox.conversationData = conversationDatas.datas[i];
        }
    }

    public void CleanChat()
    {
        for (int i = 0; i < messagesView.content.childCount; i++)
        {
            Destroy(messagesView.content.GetChild(i).gameObject);
        }
    }

    public void LoadChat(ConversationData conversation)
    {
        messageBoxPrefab = Resources.Load<GameObject>("Prefabs/MessageBox");
        messageBoxSelfPrefab = Resources.Load<GameObject>("Prefabs/MessageBox_self");
        canNotDoThisPrefab = Resources.Load<GameObject>("Prefabs/CanNotDoThis");
        CleanChat();
        for (int i = 0; i < conversation.messages.Length; i++)
        {
            GameObject msgBox = Instantiate(conversation.messages[i].sender == ESender.You ? messageBoxSelfPrefab : messageBoxPrefab, messagesView.content);
            MessageBox messageBox = msgBox.GetComponent<MessageBox>();
            if (conversation.messages[i].sender == ESender.You)
            {
                messageBox.senderNameText.text = "You";
            }
            else
            {
                messageBox.senderNameText.text = conversation.character.displayName;
            }
            messageBox.timeText.text = conversation.messages[i].time;
            messageBox.messageContentText.text = conversation.messages[i].body;
            messageBox.messageContentText.ForceMeshUpdate();
            int line = messageBox.messageContentText.textInfo.lineCount;
            RectTransform rect = messageBox.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 56f + line * 13);
        }
    }

    public void OpenChoiceBox()
    {
        choiceBox.SetActive(!choiceBox.activeSelf);
    }

    public void PsychologicalCounsel()
    {
        if (!canPsychologicalCounsel)
        {
            Destroy(Instantiate(canNotDoThisPrefab, choiceBox.transform), 0.5f);
            return;
        }
        PlayerController.Instance.PsychologicalCounsel();
    }

    public void Report()
    {
        if (!canReport)
        {
            Destroy(Instantiate(canNotDoThisPrefab, choiceBox.transform), 0.5f);
            return;
        }
        PlayerController.Instance.Report();
    }
}
