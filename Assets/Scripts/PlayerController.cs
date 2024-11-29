using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance;
	public event Action<string> OnCommentOrReplyBan;
	public event Action<string> OnCommentOrReplyPromote;

	[SerializeField] private float MaxScore = 100.0f;
	[SerializeField] private float InitialScore;
	[ReadOnly] public float currentScore;
	[ReadOnly] public Dictionary<string, List<string>> postActionRecords; // <postId, commentOrReplyId>
	[SerializeField] private GameObject actionBoxPrefab;
	[SerializeField] private Canvas rootCanvas;
	public ActionBox actionBox = null;
	[SerializeField] private Slider scoreSlider1;
	[SerializeField] private Slider scoreSlider2;
	[SerializeField] private GameObject addElement;
	[SerializeField] private GameObject minusElement;
	[SerializeField] private TMP_Text scoreText1;
	[SerializeField] private TMP_Text scoreText2;
	[SerializeField] private PostsPageController postsPage;
    [SerializeField] private Color HighPrestigeColor;
    [SerializeField] private Color LowPrestigeColor;

    public int dayCounter = 1;
	public int correctlyCounter = 0;
	public float increaseScore = 0;
	public int incorrectlyCounter = 0;
    public float decreaseScore = 0;
	public int psychologicalCounter = 0;
	public int reportCounter = 0;
    public PostDatas[] allDaysPosts;
	public ConversationDatas[] allConversationDatas;

	void Start()
	{
		Instance = this;
		currentScore = InitialScore;
		postActionRecords = new Dictionary<string, List<string>>();

		scoreSlider1.maxValue = MaxScore;
		scoreSlider2.maxValue = MaxScore;
        float animTarget = currentScore / MaxScore;
        scoreSlider1.value = currentScore;
        scoreSlider1.fillRect.GetComponent<Image>().color = Color.Lerp(LowPrestigeColor, HighPrestigeColor, animTarget);
        scoreSlider2.value = currentScore;
        scoreSlider2.fillRect.GetComponent<Image>().color = Color.Lerp(LowPrestigeColor, HighPrestigeColor, animTarget);
        scoreText1.text = currentScore.ToString();
        scoreText2.text = currentScore.ToString();

		// 加载所有posts列表
        allDaysPosts = Resources.LoadAll<PostDatas>("Data/Post");
        allConversationDatas = Resources.LoadAll<ConversationDatas>("Data/Conversation");
        PostsPageController.Instance.LoadPosts(allDaysPosts[0]);
        MessagePageController.Instance.LoadPrivateMessage(allConversationDatas[0]);
		Parallel.ForEach(allDaysPosts, dayPosts =>
        {
            foreach (var post in dayPosts.posts)
            {
                foreach (var comment in post.comments)
                {
					comment.IsBaned = false;
                    Parallel.ForEach(comment.replys, item =>
                    {
                        item.IsBaned = false;
                    });
                }
            }
        });
    }

	public void BanCommentOrReply(string postId, string commentOrReplyId)
	{
		Comment comment = null;
		CommentReply commentReply = null;

		if(!PostDataManager.Instance.FindCommentOrReply(postId, commentOrReplyId, out comment, out commentReply))
		{
			return;
		}

		float rewards = 0.0f;
		if (comment != null)
		{
			if (comment.canNotBan) return;
			float raw = comment.actionInfo.rewards;
			rewards = comment.actionInfo.action == ActionType.Ban ? raw : -1.0f * raw;
		}
		else if(commentReply != null)
        {
            if (commentReply.canNotBan) return;
            float raw = commentReply.actionInfo.rewards;
			rewards = commentReply.actionInfo.action == ActionType.Ban ? raw : -1.0f * raw;
        }
        if (rewards > 0.0f)
        {
            correctlyCounter++;
			increaseScore += rewards;
        }
        else if (rewards < 0.0f)
        {
            incorrectlyCounter++;
			decreaseScore -= rewards;
        }
        AddScore(rewards);
		RecordAction(postId, commentOrReplyId);
		HideActionBox();

		OnCommentOrReplyBan?.Invoke(commentOrReplyId);
	}

	public float PromoteCommentOrReply(string postId, string commentOrReplyId)
	{
		Comment comment = null;
		CommentReply commentReply = null;

		if (!PostDataManager.Instance.FindCommentOrReply(postId, commentOrReplyId, out comment, out commentReply))
		{
			return 0;
		}

		float rewards = 0.0f;
		if (comment != null)
		{
			float raw = comment.actionInfo.rewards;
			rewards = comment.actionInfo.action == ActionType.Promote ? raw : -1.0f * raw;
		}
		else if (commentReply != null)
		{
			float raw = commentReply.actionInfo.rewards;
			rewards = commentReply.actionInfo.action == ActionType.Promote ? raw : -1.0f * raw;
		}

        if (rewards > 0.0f)
        {
            correctlyCounter++;
            increaseScore += rewards;
        }
        else if (rewards < 0.0f)
        {
            incorrectlyCounter++;
            decreaseScore -= rewards;
        }
        AddScore(rewards);
		RecordAction(postId, commentOrReplyId);
		HideActionBox();

		OnCommentOrReplyPromote?.Invoke(commentOrReplyId);
		return rewards;

    }

	public void AddScore(float score)
	{
		currentScore += score;
		float animTarget = currentScore / MaxScore;
        scoreSlider1.value = currentScore;
		scoreSlider1.fillRect.GetComponent<Image>().color = Color.Lerp(LowPrestigeColor, HighPrestigeColor, animTarget);
        scoreSlider2.value = currentScore;
        scoreSlider2.fillRect.GetComponent<Image>().color = Color.Lerp(LowPrestigeColor, HighPrestigeColor, animTarget);
        scoreText1.text = currentScore.ToString();
        scoreText2.text = currentScore.ToString();

		if (score > 0)
		{
			GameObject g1 = Instantiate(addElement, scoreSlider1.handleRect);
			Destroy(g1, 1.5f);
			GameObject g2 = Instantiate(addElement, scoreSlider2.handleRect);
			Destroy(g2, 1.5f);
		}
		else
        {
            GameObject g1 = Instantiate(minusElement, scoreSlider1.handleRect);
            Destroy(g1, 1.5f);
            GameObject g2 = Instantiate(minusElement, scoreSlider2.handleRect);
            Destroy(g2, 1.5f);
        }

        if (currentScore <= 0)
		{
			MenuController.Instance.OpenPage("GAME OVER");
		}
    }

	void Update()
	{
		if(Input.GetMouseButtonDown(1))
		{
			HideActionBox();
		}
	}

	public void RecordAction(string postId, string targetId)
	{
		if(postActionRecords.ContainsKey(postId))
		{
			postActionRecords[postId].Add(targetId);
		}
		else
		{
			List<string> newRecord = new List<string>();
			newRecord.Add(targetId);
			postActionRecords.Add(postId, newRecord);
		}
	}

	public float GetPrestigePercent()
	{
		return currentScore / MaxScore;
	}

	public void ShowActionBox(string postId, ReplyLine replyLine)
	{
		if(postActionRecords.ContainsKey(postId) && postActionRecords[postId].Contains(replyLine.reply.id))
		{
			return;
		}

		if(actionBox == null)
		{
			GameObject actionBoxObj = Instantiate(actionBoxPrefab, rootCanvas.transform);
			actionBox = actionBoxObj.GetComponent<ActionBox>();
		}

		actionBox.SetData(postId, replyLine);
		actionBox.gameObject.SetActive(true);
		actionBox.gameObject.transform.position = Input.mousePosition;
	}

	public void ShowActionBox(string postId, ReplyBox replyBox)
	{
		if(postActionRecords.ContainsKey(postId) && postActionRecords[postId].Contains(replyBox.comment.id))
		{
			return;
		}

		if(actionBox == null)
		{
			GameObject actionBoxObj = Instantiate(actionBoxPrefab, rootCanvas.transform);
			actionBox = actionBoxObj.GetComponent<ActionBox>();
		}

		actionBox.SetData(postId, replyBox);
		actionBox.gameObject.SetActive(true);
		actionBox.gameObject.transform.position = Input.mousePosition;
	}

	public void HideActionBox()
	{
		if(actionBox)
		{
			actionBox.gameObject.SetActive(false);
		}
	}

	public void PsychologicalCounsel()
	{
		psychologicalCounter++;
        AddScore(10);
	}

	public void Report()
	{
		reportCounter++;
		AddScore(8);
    }

    public void EndOfDay()
    {
		PostData[] datas = PostsPageController.Instance.postDatas.posts;
		for(int i = 0; i < datas.Length; i++)
		{
			if (!datas[i].isViewed) return;
        }
		MenuController.Instance.OpenPage("DayEnd");
    }

	public void ToNextDay()
    {
        PostsPageController.Instance.LoadPosts(allDaysPosts[dayCounter]);
        MessagePageController.Instance.LoadPrivateMessage(allConversationDatas[dayCounter]);
        MessagePageController.Instance.CleanChat();
        dayCounter++;
        correctlyCounter = 0;
		increaseScore = 0;
        incorrectlyCounter = 0;
		decreaseScore = 0;
		psychologicalCounter = 0;
		reportCounter = 0;
        MenuController.Instance.OpenPage("Community select");
    }

	public void EndOfGame()
	{
		if (currentScore >= 80)
        {
            MenuController.Instance.OpenPage("Good END 1");
        }
		else if (currentScore >= 50)
        {
            MenuController.Instance.OpenPage("Normal END 1");
        }
		else
		{
            MenuController.Instance.OpenPage("Hard END 1");
        }
	}

	public void Restart()
	{
        string currentSceneName = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(currentSceneName);
    }
}
