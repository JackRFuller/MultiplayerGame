using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public enum GameState
    {
        In_Game,
        End_Game,
    }

    public GameState CurrentGameState;

    [SerializeField] private RigidbodyFirstPersonController RFPC_Script;

	[Header("Universal Level Attributes")]
	[SerializeField] private PC_Shoot PCS_Script;
	[SerializeField] GameObject PC;
	private Vector3 StartingPosition;
	private Quaternion StartingRotation;
	[SerializeField] Rigidbody PC_RB;
    private GameObject LevelLayout;
	public int CurrentLevelID;
	[SerializeField] private float LevelTimer;
	[SerializeField] private bool LevelStarted;
	[SerializeField] private int TargetCount = 0;

	[Header("In Game UI Items")]
    [SerializeField] private MenuManager MM_Script;
    [SerializeField] private GameObject In_GameUI;   
	[SerializeField] private Text Timer;
	[SerializeField] private Text NumofTargets;
	[SerializeField] private Text BestTimeText;

    [Header("End Game UI Items")]
    [SerializeField] private GameObject End_GameUI;
    [SerializeField] private Text EndLevelTime;
    [SerializeField] private Text EndLevelMessage;
    [SerializeField] private string[] EndLevelStrings;
    [SerializeField] private Text NextLevelText;
    private bool NextLevelAvailable;
    private bool NewLevel = true;
    private bool NewTime;

    [System.Serializable]
	public class SpecificLevelData
	{
		public GameObject LevelLayout;
		public int CurrentTargetsLeft = 0;
		public int NumOfTargets;
		public GameObject[] Targets;
		public int StartingAmmo;
		public int AmmoAvailable;
		public bool EndingPointAvailable;
		public GameObject EndingPoint;
		public float[] StarTimes;		
	}

	public SpecificLevelData[] Levels;

	// Use this for initialization
	void Start () {

	
	}

	public void InitialiseLevelData(int LevelToLoadIn)
	{        
        CurrentLevelID = LevelToLoadIn;       

        LevelLayout = Instantiate(Levels[CurrentLevelID].LevelLayout, Vector3.zero, transform.rotation) as GameObject;
        NumofTargets.text = Levels[CurrentLevelID].NumOfTargets.ToString();
        Levels[CurrentLevelID].CurrentTargetsLeft = Levels[CurrentLevelID].NumOfTargets;

        System.Array.Clear(Levels[CurrentLevelID].Targets, 0, Levels[CurrentLevelID].Targets.Length);
        Levels[CurrentLevelID].Targets = GameObject.FindGameObjectsWithTag("Target");

        if (PlayerPrefs.GetInt("TimesAttempted" + CurrentLevelID) == 0)
        {
            BestTimeText.text = "-- --";
        }
        else
        {
            BestTimeText.text = PlayerPrefs.GetFloat("BestTime" + CurrentLevelID).ToString("F2");
        }

        if (NewLevel)
        {
            StartingPosition = PC.transform.position;
            StartingRotation = PC.transform.rotation;

            NewLevel = false;
        }

        Reset();




    }
	
	// Update is called once per frame
	void Update () {

		if(LevelStarted)
		{
			RunTimer();
		}

		if((Input.GetKeyUp(KeyCode.Q) || Input.GetButtonUp("Back")))
		{
			Reset();
		}

        if(CurrentGameState == GameState.End_Game)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                ReturnToMenu();
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                if (NextLevelAvailable)
                {
                    NextLevel();
                }
            }
        }
	
	}

    void NextLevel()
    {       

        //Destroys CurrentLoadLevel
        Destroy(LevelLayout);        

        CurrentLevelID++;

        InitialiseLevelData(CurrentLevelID);
    }

    void ReturnToMenu()
    {    
        MM_Script.TurnOnMenu();

        Reset();

        //Destroys CurrentLoadLevel
        Destroy(LevelLayout);

       
    }

	public void Reset()
	{
		//Player Reset
		PC.transform.position = StartingPosition;
        PC.transform.rotation = StartingRotation;
		PC_RB.velocity = Vector3.zero;
		PC_RB.constraints = RigidbodyConstraints.None;
		PCS_Script.SetAmmo();
        RFPC_Script.inControl = true;

        //JetPack
        RFPC_Script.ResetFuel();

		//LevelReset
		LevelStarted = false;
		LevelTimer = 0;
		Levels[CurrentLevelID].CurrentTargetsLeft = Levels[CurrentLevelID].NumOfTargets;
		TargetCount = 0;

		//TargetReset
		foreach(GameObject Target in Levels[CurrentLevelID].Targets)
		{
			Target.SetActive(true);
		}

        if(CurrentGameState == GameState.End_Game)
        {
            SwitchUI();
        }

       
		ResetUI();
	}

	void ResetUI()
	{
		Timer.text = LevelTimer.ToString("F2");
		NumofTargets.text = Levels[CurrentLevelID].NumOfTargets.ToString();
	}

	void RunTimer()
	{
		LevelTimer += Time.deltaTime;
		Timer.text = LevelTimer.ToString("F2");
	}

	public void AddTargetCount()
	{
		TargetCount++;
		Levels[CurrentLevelID].CurrentTargetsLeft--;

		NumofTargets.text = Levels[CurrentLevelID].CurrentTargetsLeft.ToString();

		if(TargetCount == Levels[CurrentLevelID].NumOfTargets)
		{
			LevelStarted = false;
			EndLevel();
		}
	}

	void EndLevel()
	{
        CurrentGameState = GameState.In_Game;

		PC_RB.velocity = Vector3.zero;
		PC_RB.constraints = RigidbodyConstraints.FreezeAll;
        RFPC_Script.inControl = false;

		if(PlayerPrefs.GetFloat("BestTime"+CurrentLevelID.ToString()) == 0)
		{
			PlayerPrefs.SetFloat("BestTime"+CurrentLevelID.ToString(), LevelTimer);
			BestTimeText.text = PlayerPrefs.GetFloat("BestTime"+CurrentLevelID.ToString()).ToString();
		}

        if (PlayerPrefs.GetInt("TimesAttempted" + CurrentLevelID) == 0)
        {
            PlayerPrefs.SetFloat("BestTime" + CurrentLevelID, LevelTimer);
            BestTimeText.text = PlayerPrefs.GetFloat("BestTime" + CurrentLevelID).ToString("F2");
            NewTime = true;
        }
        else
        {
            if(LevelTimer < PlayerPrefs.GetFloat("BestTime" + CurrentLevelID))
            {
                PlayerPrefs.SetFloat("BestTime" + CurrentLevelID, LevelTimer);
                BestTimeText.text = PlayerPrefs.GetFloat("BestTime" + CurrentLevelID).ToString("F2");
                NewTime = true;
            }
        }

        PlayerPrefs.SetInt("TimesAttempted" + CurrentLevelID, PlayerPrefs.GetInt("TimesAttempted" + CurrentLevelID) + 1);

        //UI
        SwitchUI();     

    }

    public void SwitchUI()
    {
        switch (CurrentGameState)
        {
            case (GameState.In_Game):
                In_GameUI.SetActive(false);
                End_GameUI.SetActive(true);
                DetermineUI();
                CurrentGameState = GameState.End_Game;
                break;
            case (GameState.End_Game):
                In_GameUI.SetActive(true);
                End_GameUI.SetActive(false);
                CurrentGameState = GameState.In_Game;
                break;
        }
    }

    void DetermineUI()
    {
        EndLevelTime.text = LevelTimer.ToString("F2");
        if (NewTime)
        {
            EndLevelMessage.text = EndLevelStrings[1];
        }
        else
        {
            EndLevelMessage.text = EndLevelStrings[0];
        }

        if(CurrentLevelID == Levels.Length - 1)
        {
            NextLevelText.enabled = false;
            NextLevelAvailable = false;
        }
        else
        {
            NextLevelText.enabled = true;
            NextLevelAvailable = true;
        }

        NewTime = false;
    }

    public void LevelStart()
	{
		LevelStarted = true;
        CurrentGameState = GameState.In_Game;
	}


}
