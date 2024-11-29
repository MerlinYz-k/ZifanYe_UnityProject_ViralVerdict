using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostPageController : MonoBehaviour
{
    public static PostPageController instance;
    public Post post;
    public PostData postData;


    public Image senderHead;
    public TMP_Text senderNameText;
    public TMP_Text titleText;
    public TMP_Text contentText;
    public Image image;
    public TMP_Text timeText;
    public ScrollRect replyView;
    
    private ReplyBox replyBoxPrefab;
    private ReplyLine replyLinePrefab;
    private ReplyToLine replyToLinePrefab;

    private Color senderColor = new(0.9490197f, 0.6862745f, 0.372549f);

    private void Awake()
    {
        instance = this;
        replyBoxPrefab = Resources.Load<ReplyBox>("Prefabs/ReplyBox");
        replyLinePrefab = Resources.Load<ReplyLine>("Prefabs/ReplyLine");
        replyToLinePrefab = Resources.Load<ReplyToLine>("Prefabs/ReplyToLine");
    }

    private void OnEnable()
    {
        senderHead.sprite = postData.character.avatar;
        senderNameText.text = postData.character.displayName;
        titleText.text = postData.postTitle;
        contentText.text = postData.postContent;
        contentText.ForceMeshUpdate();
        int line = contentText.textInfo.lineCount;
        RectTransform parentRect = contentText.transform.parent.GetComponent<RectTransform>();
        parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, 200f + line * 25);
        timeText.text = postData.postTime;

        image.gameObject.SetActive(postData.sprite != null);
        if (postData.sprite != null)
        {
            image.sprite = postData.sprite;
            image.SetNativeSize();
            //FitSprite();
        }

        for (int i = 3; i < replyView.content.childCount; i++)
        {
            Destroy(replyView.content.GetChild(i).gameObject);
        }

        for (int i = 0; i < postData.comments.Length; i++)
        {
            Comment comment = postData.comments[i];
            ReplyBox replyBox = Instantiate(replyBoxPrefab, replyView.content);
            replyBox.parentPostId = postData.id;
            replyBox.comment = comment;

            CreateReplyLines(comment, replyBox.replyContent);
        }
    }

    private void Update()
    {
        contentText.ForceMeshUpdate();
        int line = contentText.textInfo.lineCount;
        RectTransform parentRect = contentText.transform.parent.GetComponent<RectTransform>();
        parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, 200f + line * 25);
    }

    public void FitSprite()
    {
        if (image.sprite == null) return;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = image.sprite.bounds.size;

        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = new Vector2(size.x, size.y);
    }

    /// <summary>
    /// 创建回复的回复
    /// </summary>
    private void CreateReplyLines(Comment reply, Transform content)
    {
        for (int i = 0; i < reply.replys.Length; i++)
        {
            CommentReply r = reply.replys[i];
            if (r.to == null)
            {
                ReplyLine replyLine;
                replyLine = Instantiate(replyLinePrefab, content);
                replyLine.Init(postData.id, r, r.from == postData.character, r.to == postData.character);
            }
            else
            {
                ReplyToLine replyToLine;
                replyToLine = Instantiate(replyToLinePrefab, content);
                replyToLine.Init(postData.id, r, r.from == postData.character, r.to == postData.character);
            }
        }
    }
}
