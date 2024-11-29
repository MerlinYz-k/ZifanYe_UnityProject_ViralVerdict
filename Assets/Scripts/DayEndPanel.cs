using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DayEndPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private TMP_Text nextDayText;
    [SerializeField] private TMP_Text textPrefab;
    [SerializeField] private ScrollRect scrollRect;

    private void OnEnable()
    {
        if (PlayerController.Instance.IsUnityNull()) return;

        for (int i = 0; i < scrollRect.content.transform.childCount; i++)
        {
            GameObject child = scrollRect.content.transform.GetChild(i).gameObject;
            if (child != dayText.gameObject && child != totalScoreText.gameObject && child != nextDayText.gameObject)
            {
                Destroy(child);
            }
        }

        scrollRect.normalizedPosition = new Vector2(0, 1);
        dayText.text = $"Day <color=\"red\">{PlayerController.Instance.dayCounter}</color> Over";
        Instantiate(textPrefab, scrollRect.content).text = $"You have viewed a total of <color=\"red\">{PlayerController.Instance.allDaysPosts[PlayerController.Instance.dayCounter - 1].posts.Length}</color> post;";
        if (PlayerController.Instance.correctlyCounter > 0)
        {
            Instantiate(textPrefab, scrollRect.content).text = $"You have correctly dealt with <color=\"green\">{PlayerController.Instance.correctlyCounter}</color> comments,";
            Instantiate(textPrefab, scrollRect.content).text = $"reputation increase: <color=\"green\">{PlayerController.Instance.increaseScore}</color> points;";
        }
        else
        {
            Instantiate(textPrefab, scrollRect.content).text = $"You have correctly dealt with <color=\"red\">{PlayerController.Instance.correctlyCounter}</color> comments,";
            Instantiate(textPrefab, scrollRect.content).text = $"reputation increase: <color=\"red\">{PlayerController.Instance.increaseScore}</color> points;";
        }
        Instantiate(textPrefab, scrollRect.content).text = $"You have incorrectly dealt with <color=\"red\">{PlayerController.Instance.incorrectlyCounter}</color> comments,";
        Instantiate(textPrefab, scrollRect.content).text = $"reputation decrease: <color=\"red\">{PlayerController.Instance.decreaseScore}</color> points;";
        if (PlayerController.Instance.correctlyCounter + PlayerController.Instance.incorrectlyCounter == 0)
        {
            Instantiate(textPrefab, scrollRect.content).text = $"You did not deal with any comments today,";
            Instantiate(textPrefab, scrollRect.content).text = $"reputation decrease: 10 points;";
            PlayerController.Instance.AddScore(-10);
        }
        totalScoreText.text = $"Current total reputation:\n<color=\"red\">{PlayerController.Instance.currentScore}</color> points";
        if (PlayerController.Instance.psychologicalCounter > 0)
        {
            Instantiate(textPrefab, scrollRect.content).text = $"You provided psychological counselling to <color=\"green\">{PlayerController.Instance.psychologicalCounter}</color> users,";
            Instantiate(textPrefab, scrollRect.content).text = $"reputation up: <color=\"green\">{PlayerController.Instance.psychologicalCounter * 10}</color> points;";
        }
        if (PlayerController.Instance.reportCounter > 0)
        {
            Instantiate(textPrefab, scrollRect.content).text = $"You reported <color=\"green\">{PlayerController.Instance.reportCounter}</color> online incidents to the police,";
            Instantiate(textPrefab, scrollRect.content).text = $"reputation up: <color=\"green\">{PlayerController.Instance.psychologicalCounter * 8}</color> points;";
        }

        totalScoreText.transform.SetSiblingIndex(totalScoreText.transform.parent.childCount - 1);
        nextDayText.transform.SetSiblingIndex(totalScoreText.transform.parent.childCount - 1);



        if (PlayerController.Instance.dayCounter == 6)
        {
            this.GetComponent<PageController>().Exit();
            PlayerController.Instance.EndOfGame();
        }
    }
}
