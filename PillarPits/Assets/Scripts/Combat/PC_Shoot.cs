using UnityEngine;
using System.Collections;

public class PC_Shoot : MonoBehaviour {

	[Header("Shooting Attributes")]
	[SerializeField] private float ShotRange;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 fwd = transform.TransformDirection(Vector3.left);
		RaycastHit hit;
		Debug.DrawRay(transform.position, fwd * 100, Color.green);

		if(Input.GetMouseButtonDown(0))
		{
		

			if(Physics.Raycast(transform.position, fwd, out hit, ShotRange))
			{

				if(hit.collider.gameObject.name == "Cube")
				{
					Debug.Log("Success");
				}
			}
		}
	
	}
}
