using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class MenuController : MonoBehaviour
{
	public static MenuController Instance;
	private Canvas RootCanvas;

	[SerializeField]
	private List<PageController> ManagedPages;

	[SerializeField]
	private PageController InitialPage;

	private Stack<PageController> PageStack = new Stack<PageController>();
	public Dictionary<string, PageController> PagesDictionary = new Dictionary<string, PageController>();

	private void Awake()
	{
		Instance = this;

		RootCanvas = GetComponent<Canvas>();

        foreach (var Page in ManagedPages)
        {
            Page.InitPage(this);
            Page.Enter();
            PagesDictionary.Add(Page.GetPageName(), Page);
        }
	}

	private void Start()
    {
        foreach (var Page in ManagedPages)
        {
            //Page.InitPage(this);
            Page.Exit();
            //PagesDictionary.Add(Page.GetPageName(), Page);
        }

        if (InitialPage != null)
		{
			OpenPage(InitialPage.GetPageName());
		}
	}

	public void OpenPage(string PageName)
	{
		if(!PagesDictionary.ContainsKey(PageName))
		{
			Debug.LogWarning(string.Format("Failed to find Page with name {}", PageName));
			return;
		}

		if(PageStack.Count > 0)
		{
			PageController CurrentTopPage = PageStack.Peek();
			if(CurrentTopPage.GetPageName() == PageName)
			{
				Debug.LogWarning(string.Format("{} is already opened", PageName));
				return;
			}

			CurrentTopPage.Exit();
		}

		PageController NewPage = PagesDictionary[PageName];
		NewPage.Enter();

		PageStack.Push(NewPage);
	}

	public void Back()
	{
		if(PageStack.Count > 1)
		{
			PageController CurrentTopPage = PageStack.Pop();
			CurrentTopPage.Exit();

			CurrentTopPage = PageStack.Peek();
			CurrentTopPage.Enter();
		}
		else
		{
			Debug.LogWarning("No Page in Stack");
		}
	}
}
