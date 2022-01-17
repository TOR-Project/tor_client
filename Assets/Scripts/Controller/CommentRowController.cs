using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentRowController : MonoBehaviour
{
    [SerializeField]
    Text nickNameText;
    [SerializeField]
    Text blockText;
    [SerializeField]
    Text commentText;

    public void updateCommentData(CommentData _commentData)
    {
        nickNameText.text = _commentData.nickname;
        blockText.text = string.Format("({0:#,###} block)", _commentData.block);
        commentText.text = _commentData.comment;
    }
}
