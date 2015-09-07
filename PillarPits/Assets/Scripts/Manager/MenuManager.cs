using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	[SerializeField] private GameManager GM_Script;
	[SerializeField] private LevelManager LM_Script;

	[Header("UI Items")]
	[SerializeField] private GameObject CursorIcon;
	[SerializeField] private Text[] TimeText;

	[Header("Menu Items")]
	[SerializeField] private GameObject MenuCanvas;
	[SerializeField] private GameObject MenuCamera;

	[Header("Gameplay Items")]
	[SerializeField] private GameObject PC;
	[SerializeField] private GameObject GameplayCanvas;

	// Use this for initialization
	void Start () {

		Cursor.visible = false;

		for(int i = 0; i < TimeText.Length; i++)
		{
			Debug.Log(i);
			TimeText[i].text = "Best Time: " + PlayerPrefs.GetFloat("BestTime" + i).ToString("F2");

			if(PlayerPrefs.GetFloat("BestTime" + i) == 0)
			{
				TimeText[i].text = "Best Time: -- --"; 
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		if(GM_Script.CurrentGameState == GameManager.GameState.Menu)
		{
			MoveCursor();
		}
	
	}

	void MoveCursor()
	{
		Cursor.visible = false;

		if(GM_Script.CurrentControlState == GameManager.ControlState.MouseAndKeyboard)
		{
			CursorIcon.transform.position = Input.mousePosition;
		}
	}

	public void LoadInLevel(int LevelID)
	{
		GM_Script.CurrentGameState = GameManager.GameState.Gameplay;

		TurnOffMenu();

		LM_Script.InitialiseLevelData(LevelID);
	}

	void TurnOffMenu()
	{
		MenuCanvas.SetActive(false);
		MenuCamera.SetActive(false);

		TurnOnGameplayItems();
	}

	void TurnOnGameplayItems()
	{
		PC.SetActive(true);
		GameplayCanvas.SetActive(true);
	}


}
