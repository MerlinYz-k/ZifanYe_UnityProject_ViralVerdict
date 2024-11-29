using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostBox : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text senderNameText;
    public TMP_Text timeText;
    public TMP_Text postContentText;
    public TMP_Text talkNumText;
    public TMP_Text hotNumText;
    public Image circle;

    public PostData postData;

    //private bool isVisited = false;

    public void OpenPostPage()
    {
        postData.isViewed = true;
        PostPageController.instance.postData = postData;
        PostPageController.instance.gameObject.SetActive(true);
        circle.gameObject.SetActive(false);
    }
}
