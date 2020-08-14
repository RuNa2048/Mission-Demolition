using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
	static public GameObject POI;

	[Header("Set in Inspector")]
	[SerializeField]
	private float _easing = 0.05f;
	[SerializeField]
	private Vector2 minXY = Vector2.zero;

	[Header("Set Dynamically")]
	public float camZ;

	private void Awake()
	{
		camZ = this.transform.position.z;
	}

	private void FixedUpdate()
	{
		Vector3 destination;

		if (POI == null)
		{
			destination = Vector3.zero;
		}
		else
		{
			destination = POI.transform.position;
			if (POI.tag == "Projectle")
			{
				if (POI.GetComponent<Rigidbody>().IsSleeping())
				{
					POI = null;
					return;
				}
			}
		}
		destination.x = Mathf.Max(minXY.x, destination.x);
		destination.y = Mathf.Max(minXY.x, destination.y);
		destination = Vector3.Lerp(transform.position, destination, _easing);
		destination.z = camZ;
		transform.position = destination;

		Camera.main.orthographicSize = destination.y + 10;	
	}
}
