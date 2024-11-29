using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndPageController : MonoBehaviour
{
    public List<string> successfullyThings;
    public List<string> failedThings;

    public Transform success;
    public Transform failed;
    public GameObject linePrefab;

    private void Awake()
    {
        if (success != null)
        {
            for (int i = 0; i < successfullyThings.Count; i++)
            {
                GameObject line = Instantiate(linePrefab, success);
                line.GetComponent<TMP_Text>().text ="- " + successfullyThings[i];
            }
        }
        if (failed != null)
        {
            for(int i = 0;i < failedThings.Count; i++)
            {
                GameObject line = Instantiate(linePrefab, failed);
                line.GetComponent<TMP_Text>().text = "- " + failedThings[i];
            }
        }
    }
}
