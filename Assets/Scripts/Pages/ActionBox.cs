using UnityEngine;

public class ActionBox : MonoBehaviour
{
	private string parentPostId;
	private string commentOrReplyId;
	private ReplyLine replyLine;
	private ReplyBox replyBox;

	public void SetData(string postId, ReplyLine replyLine)
	{
		parentPostId = postId;
		this.replyLine = replyLine;
		commentOrReplyId = replyLine.reply.id;
        replyLine.reply.IsBaned = true;
    }

	public void SetData(string postId, ReplyBox replyBox)
	{
		parentPostId = postId;
		this.replyBox = replyBox;
		commentOrReplyId = replyBox.comment.id;
		replyBox.comment.IsBaned = true;
    }

	public void OnBan()
	{
		if (replyLine != null)
		{
			if (replyLine.reply.canNotBan)
				return;

			replyLine.Baned();
		}
		else if (replyBox != null)
		{
            replyBox.Baned();

        }
		PlayerController.Instance.BanCommentOrReply(parentPostId, commentOrReplyId);
	}

	public void OnPromote()
    {
        float value = PlayerController.Instance.PromoteCommentOrReply(parentPostId, commentOrReplyId);
        if (replyLine != null)
        {
            replyLine.Promote(value);
        }
		else if (replyBox != null)
        {
            replyBox.Promote(value);
        }
    }

	public void OnCancel()
	{
		PlayerController.Instance.HideActionBox();
	}
}
