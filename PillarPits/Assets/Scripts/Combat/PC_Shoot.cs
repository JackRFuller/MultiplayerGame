using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PC_Shoot : MonoBehaviour {

	[SerializeField] private LevelManager LM_Script;

	[Header("Shooting Attributes")]
	[SerializeField] private float ShotRange;
	[SerializeField] private float ShootingCooldownTime;
	[SerializeField] private LineRenderer BulletTrail;
	private bool CanShoot = true;

	[Header("Reloading Attributes")]
	private bool Reloading;
	[SerializeField] private int CurrentClip;
	[SerializeField] private int MaxAmmo;
	[SerializeField] private int CurrentAmmo;
	[SerializeField] private int MaxClipSize;
	[SerializeField] private float ReloadCooldownTime;

	[Header("Ammo UI Objects")]
	[SerializeField] private Image Reticle;
	[SerializeField] private Text CurrentClipText;
	[SerializeField] private Text CurrentAmmoText;

	[Header("Animation Settings")]
	[SerializeField] private GunAnimationController GAC_Script;


	// Use this for initialization
	void Start () {

		SetAmmo();
	
	}

	public void SetAmmo()
	{
		CurrentAmmo = LM_Script.Levels[LM_Script.CurrentLevelID].AmmoAvailable;
		MaxAmmo = LM_Script.Levels[LM_Script.CurrentLevelID].AmmoAvailable;
		CurrentClip = LM_Script.Levels[LM_Script.CurrentLevelID].StartingAmmo;

		UpdateAmmoText();
	}
	
	// Update is called once per frame
	void Update () {

		//Reload
		if((Input.GetKey("r") || Input.GetButtonUp("RB")))
		{
			if(!Reloading)
			{
				AmmoCheck();
			}

		}

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height /2, 0));
		RaycastHit hit;

		//Shoot
		if((Input.GetMouseButtonDown(0) || Input.GetAxis("RightTrigger") > 0))
		{		
			if(CheckShoot())
			{
				if(CanShoot)
				{

					if(Physics.Raycast(ray, out hit, ShotRange))
					{
						BulletTrail.enabled = true;
						StartCoroutine(BulletTrailCoolDown());
						if(hit.collider.tag == "Target")
						{
							DestroyEnemy(hit.collider.gameObject);
						}
					}

					CanShoot = false;
					StartCoroutine(ShootingCooldown());
					CurrentClip--;
					UpdateAmmoText();

					
					GAC_Script.Shoot();


				}

			}
			else
			{
				AmmoCheck();
			}

		}
	}

	void DestroyEnemy(GameObject Target)
	{
		Target.SetActive(false);
		LM_Script.AddTargetCount();
	}

	IEnumerator BulletTrailCoolDown()
	{
		yield return new WaitForSeconds(0.1F);
		BulletTrail.enabled = false;
	}

	IEnumerator ShootingCooldown()
	{
		yield return new WaitForSeconds(ShootingCooldownTime);

		CanShoot = true;
	}

	bool CheckShoot()
	{
		if(CurrentClip > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void AmmoCheck()
	{
		if(CurrentAmmo > 0)
		{
			if(CurrentClip < MaxClipSize)
			{
				Reload();
			}
		}
	}

	void Reload()
	{
		GAC_Script.Reload();
		Reloading = true;
		Reticle.enabled = false;
		StartCoroutine(ReloadCooldown());

		for(int i = 0; i < MaxClipSize; i++)
		{
			if(CurrentAmmo > 0)
			{
				CurrentClip++;
				CurrentAmmo--;
				UpdateAmmoText();
				if(CurrentClip == MaxClipSize)
				{
					break;
				}
			}
		}
	}

	void UpdateAmmoText()
	{
		CurrentClipText.text = CurrentClip.ToString();
		CurrentAmmoText.text = CurrentAmmo.ToString();
	}

	IEnumerator ReloadCooldown()
	{
		yield return new WaitForSeconds(ReloadCooldownTime);
		Reticle.enabled = true;
		Reloading = false;
	}
}
