using UnityEngine;

public class PageController : MonoBehaviour
{
	[SerializeField]
	private string PageName;

	private MenuController Menu;

	public string GetPageName()
	{
		return PageName;
	}

	public void InitPage(MenuController i_Menu)
	{
		Menu = i_Menu;
	}

	public void Enter()
	{
		gameObject.SetActive(true);
	}

	public void Exit()
	{
		gameObject.SetActive(false);
	}

	public void Close()
	{
		Menu.Back();
	}
}
