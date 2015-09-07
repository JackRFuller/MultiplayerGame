using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public enum ControlState
    {
        Xbox360Controller,
        MouseAndKeyboard,
    }

	public enum GameState
	{
		Menu,
		Gameplay,
	}

    public ControlState CurrentControlState;
	public GameState CurrentGameState;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
