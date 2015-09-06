using UnityEngine;
using System.Collections;

public class GunAnimationController : MonoBehaviour {

    

	[SerializeField] private Animator GunAnimator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Shoot()
	{
		GunAnimator.SetBool("Shoot", true);
		GunAnimator.SetBool("Idle", false);

		StartCoroutine(Cooldown(0.25F));
	}

	public void Reload()
	{
		GunAnimator.SetBool("Reload", true);
		GunAnimator.SetBool("Idle", false);

		StartCoroutine(Cooldown(2.0F));
	}

	IEnumerator Cooldown(float CooldownTime)
	{
		yield return new WaitForSeconds(CooldownTime);
		ReturnToIdle();
	}

	public void ReturnToIdle()
	{
		GunAnimator.SetBool("Reload", false);
		GunAnimator.SetBool("Shoot", false);
		GunAnimator.SetBool("Idle", true);
	}
}
