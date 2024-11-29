using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostsPageController : MonoBehaviour
{
    public static PostsPageController Instance;
    public ScrollRect postView;
    public PostDatas postDatas;

    private PostBox postBoxPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadPosts(PostDatas postDatas)
    {
        postBoxPrefab = Resources.Load<PostBox>("Prefabs/PostBox");
        this.postDatas = postDatas;
        for (int i = 0; i < postView.content.childCount; i++)
        {
            Destroy(postView.content.GetChild(i).gameObject);
        }
        for (int i = 0; i < this.postDatas.posts.Length; i++)
        {
            PostData postData = this.postDatas.posts[i];
            PostBox postBox = Instantiate(postBoxPrefab, postView.content);
            postBox.titleText.text = postData.postTitle;
            postBox.senderNameText.text = postData.character.displayName;
            postBox.timeText.text = postData.postTime;
            postBox.postContentText.text = postData.postContent;
            postBox.talkNumText.text = postData.comments.Length.ToString();
            postBox.hotNumText.text = postData.hotNum.ToString();
            postBox.postData = postData;
        }
    }
}
