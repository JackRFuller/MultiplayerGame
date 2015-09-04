using UnityEngine;
using System.Collections;

public class CombatZoneBehaviour : MonoBehaviour {

	[SerializeField] private LevelManager LM_Script;

	void OnTriggerEnter(Collider collider)
	{
		if(collider.tag == "Player")
		{
			LM_Script.LevelStart();
		}
	}


}
