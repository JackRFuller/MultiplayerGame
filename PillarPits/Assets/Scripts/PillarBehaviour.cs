using UnityEngine;
using System.Collections;

public class PillarBehaviour : MonoBehaviour {

	[SerializeField] private float minHeight;
	[SerializeField] private float MaxHeight;

	// Use this for initialization
	void Start () {

		float Height = Random.Range(minHeight,MaxHeight);
		transform.localScale = new Vector3(transform.localScale.x, Height, transform.localScale.z);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
