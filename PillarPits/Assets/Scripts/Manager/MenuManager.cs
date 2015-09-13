using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	[SerializeField] private GameManager GM_Script;
	[SerializeField] private LevelManager LM_Script;

	[Header("UI Items")]
    [SerializeField] private GameObject LevelButtons;
	[SerializeField] private GameObject CursorIcon;
    [SerializeField] private Text[] LevelTitleText;
	[SerializeField] private Text[] TimeText;

    [Header("Level Preview Attributes")]
    [SerializeField] private float RotationalSpeed;

    [Header("Level Preview Items")]
    private GameObject[] Targets;
    private int CurrentLevelId;
    private GameObject LevelLayout;
    [SerializeField] private GameObject CameraPreview;
    [SerializeField] private GameObject LevelPreview;
    [SerializeField] private Text LevelTitle;
    [SerializeField] private Text LevelDescription;
    [SerializeField] private Text BestTime;
    [SerializeField] private Text[] StarRequirements;

	[Header("Menu Items")]
	[SerializeField] private GameObject MenuCanvas;
	[SerializeField] private GameObject MenuCamera;

	[Header("Gameplay Items")]
	[SerializeField] private GameObject PC;
	[SerializeField] private GameObject GameplayCanvas;

	// Use this for initialization
	void Start () {

		Cursor.visible = false;

        BestTimeUpdates();

        LevelTitles();
	
	}

    void LevelTitles()
    {
        for (int i = 0; i < LevelTitleText.Length; i++)
        {
            LevelTitleText[i].text = LM_Script.Levels[i].LevelTitle;
        }
    }

    void BestTimeUpdates()
    {
        for (int i = 0; i < TimeText.Length; i++)
        {
            
            TimeText[i].text = PlayerPrefs.GetFloat("BestTime" + i).ToString("F2");

            if (PlayerPrefs.GetFloat("BestTime" + i) == 0)
            {
                TimeText[i].text = "-- --";
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        

		if(GM_Script.CurrentGameState == GameManager.GameState.Menu)
		{
			MoveCursor();
		}

        if(GM_Script.CurrentGameState == GameManager.GameState.LevelPreview)
        {
            MoveCursor();

            LevelPreviewControls();
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

    void LevelPreviewControls()
    {
        float RotationSpeed = Input.GetAxis("Horizontal") * RotationalSpeed;       

        CameraPreview.transform.RotateAround(Vector3.zero, Vector3.up, RotationSpeed);
    }

    public void ShowLevelPreview(int LevelId)
    {
        CurrentLevelId = LevelId;

        LevelButtons.GetComponent<Animation>().Play("Up");

        //ChangeItems
        LevelTitle.text = LM_Script.Levels[LevelId].LevelTitle;

        BestTime.text = PlayerPrefs.GetFloat("BestTime" + LevelId).ToString("F2");

        if (PlayerPrefs.GetFloat("BestTime" + LevelId) == 0)
        {
            BestTime.text = "-- --";
        }
        
        for(int i = 0; i < StarRequirements.Length; i++)
        {
            StarRequirements[i].text = LM_Script.Levels[LevelId].StarTimes[i].ToString("F2");
        }      

        LevelPreview.GetComponent<Animation>().Play("Up");
       

        StartCoroutine(LoadInPreviewLevel(LevelId));

        GM_Script.CurrentGameState = GameManager.GameState.LevelPreview;

    }

    IEnumerator LoadInPreviewLevel(int LevelId)
    {
        yield return new WaitForSeconds(0.5F);
        LevelLayout = Instantiate(LM_Script.Levels[LevelId].LevelLayout, LM_Script.Levels[LevelId].LevelLayout.transform.position, LM_Script.Levels[LevelId].LevelLayout.transform.rotation) as GameObject;
        CameraPreview.SetActive(true);

        Targets = GameObject.FindGameObjectsWithTag("Target");
        foreach(GameObject Target in Targets)
        {
            Target.tag = "Untagged";
        }
    }

    public void Back()
    {
        Destroy(LevelLayout);

        LevelPreview.GetComponent<Animation>().Play("Down");

        LevelButtons.GetComponent<Animation>().Play("Down");

        CameraPreview.SetActive(false);
        

        GM_Script.CurrentGameState = GameManager.GameState.Menu;
    }

    public void Play()
    {

        Destroy(LevelLayout);

        

        TurnOffMenu();

        LM_Script.InitialiseLevelData(CurrentLevelId);

        GM_Script.CurrentGameState = GameManager.GameState.Gameplay;

    }

    public void TurnOnMenu()
    {
        

        BestTimeUpdates();
        ResetTOLevelMenu();
        
        MenuCanvas.SetActive(true);
        MenuCamera.SetActive(true);

        TurnOffGameplayItems();
    }

    void ResetTOLevelMenu()
    {



        CameraPreview.SetActive(false);

        LevelButtons.GetComponent<Animation>().Play("Down");
        LevelPreview.GetComponent<Animation>().Play("Down");
    }

    void TurnOffGameplayItems()
    {
        PC.SetActive(false);
        GameplayCanvas.SetActive(false);

        GM_Script.CurrentGameState = GameManager.GameState.Menu;
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
        LM_Script.CurrentGameState = LevelManager.GameState.End_Game;
        LM_Script.SwitchUI();
	}


}
