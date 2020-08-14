using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
	static private Slingshot S;

	[Header("Set in Inspector")]
	[SerializeField]
	private GameObject prefaProjectle;
	[SerializeField]
	private float velocityMult = 8f;

	static public Vector3 LAUNCH_POS
	{
		get
		{
			if (S == null) return Vector3.zero;
			return S.launchPos;
		}
	}

	[Header("Set Dynamically")]
	public GameObject launchPoint;
	public GameObject projectile; 
	public Vector3 launchPos;
	public bool aimingMode;

	private Rigidbody _projectileRigidbody;

	private void Awake()
	{
		S = this;

		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
	}

	private void Update()
	{
		if (!aimingMode) return;

		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

		Vector3 mouseDelta = mousePos3D - launchPos;
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if (mouseDelta.magnitude > maxMagnitude)
		{
			mouseDelta.Normalize();
			mouseDelta *= maxMagnitude;
		}

		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;
		if (Input.GetMouseButtonUp(0))
		{
			aimingMode = false;
			_projectileRigidbody.isKinematic = false;
			_projectileRigidbody.velocity = -mouseDelta * velocityMult;
			FollowCam.POI = projectile;
			projectile = null;

			MissionDemonition.ShotFired();
			ProjectleLine.S.poi = projectile;
		}
	}

	private void OnMouseEnter()
	{
		//print("Slingshot: OnMouseEnter");
		launchPoint.SetActive(true);
	}

	private void OnMouseExit()
	{
		//print("Slingshot: OnMouseExit");
		launchPoint.SetActive(false);
	}

	private void OnMouseDown()
	{
		aimingMode = true;
		projectile = Instantiate(prefaProjectle) as GameObject;
		projectile.transform.position = launchPos;
		projectile.GetComponent<Rigidbody>().isKinematic = true;

		_projectileRigidbody = projectile.GetComponent<Rigidbody>();
		_projectileRigidbody.isKinematic = true;
	}
}
