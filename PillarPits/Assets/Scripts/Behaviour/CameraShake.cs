using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public static CameraShake Instance;

	public float amplitude = 0.1F;
	public bool isShaking = false;

	private Vector3 InitialPostion;

	// Use this for initialization
	void Start () {

		Instance = this;
		InitialPostion = transform.localPosition;
	
	}
	
	// Update is called once per frame
	void Update () {

		if(isShaking)
		{
			transform.localPosition = InitialPostion + Random.insideUnitSphere * amplitude;
		}


	
	}
}
