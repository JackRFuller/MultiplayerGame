using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	[Header("Universal Level Attributes")]
	[SerializeField] private PC_Shoot PCS_Script;
	[SerializeField] GameObject PC;
	private Vector3 StartingPosition;
	private Vector3 StartingRotation;
	[SerializeField] Rigidbody PC_RB;
	public int CurrentLevelID;
	[SerializeField] private float LevelTimer;
	[SerializeField] private bool LevelStarted;
	[SerializeField] private int TargetCount = 0;

	[Header("UI Items")]
	[SerializeField] private Text Timer;
	[SerializeField] private Text NumofTargets;
	[SerializeField] private Text BestTimeText;

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

		public PlayerPrefs BestTime;
		public PlayerPrefs TimesAttempted;
	}

	public SpecificLevelData[] Levels;

	// Use this for initialization
	void Start () {

		InitialiseLevelData();
	
	}

	void InitialiseLevelData()
	{
		StartingPosition = PC.transform.position;

		Instantiate(Levels[CurrentLevelID].LevelLayout, Vector3.zero, transform.rotation);
		NumofTargets.text = Levels[CurrentLevelID].NumOfTargets.ToString();
		Levels[CurrentLevelID].CurrentTargetsLeft = Levels[CurrentLevelID].NumOfTargets;

		System.Array.Clear(Levels[CurrentLevelID].Targets,0,Levels[CurrentLevelID].Targets.Length);
		Levels[CurrentLevelID].Targets = GameObject.FindGameObjectsWithTag("Target");


		 
	}
	
	// Update is called once per frame
	void Update () {

		if(LevelStarted)
		{
			RunTimer();
		}

		if(Input.GetKey(KeyCode.Q))
		{
			Reset();
		}
	
	}

	void Reset()
	{
		//Player Reset
		PC.transform.position = StartingPosition;
		PC_RB.velocity = Vector3.zero;
		PC_RB.constraints = RigidbodyConstraints.None;
		PCS_Script.SetAmmo();

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
		PC_RB.velocity = Vector3.zero;
		PC_RB.constraints = RigidbodyConstraints.FreezeAll;

		if(PlayerPrefs.GetFloat("BestTime"+CurrentLevelID.ToString()) == 0)
		{
			PlayerPrefs.SetFloat("BestTime"+CurrentLevelID.ToString(), LevelTimer);
			BestTimeText.text = PlayerPrefs.GetFloat("BestTime"+CurrentLevelID.ToString()).ToString();
		}

		if(LevelTimer < PlayerPrefs.GetFloat("BestTime"+CurrentLevelID.ToString()))
		{

		}
	}

	public void LevelStart()
	{
		LevelStarted = true;
	}


}
