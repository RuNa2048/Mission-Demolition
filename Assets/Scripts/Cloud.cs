using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
	[Header("Set in Inspector")]
	public GameObject cloudSphere;
	public int numSphereMin = 6;
	public int numSphereMax = 8;
	public Vector3 sphereOffsetscale = new Vector3(5, 2, 1);
	public Vector2 sphereScaleRangeX = new Vector2(4, 8);
	public Vector2 sphereScaleRangeY = new Vector2(3, 4);
	public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
	public float scaleYMin = 2f;

	private List<GameObject> spheres;

	private void Start()
	{
		spheres = new List<GameObject>();

		int num = Random.Range(numSphereMin, numSphereMax);
		for (int i = 0; i < num; i++)
		{
			GameObject sp = Instantiate<GameObject>(cloudSphere);
			spheres.Add(sp);
			Transform spTrans = sp.transform;
			spTrans.SetParent(this.transform);

			Vector3 offset = Random.insideUnitSphere;
			offset.x *= sphereOffsetscale.x;
			offset.y *= sphereOffsetscale.y;
			offset.z *= sphereOffsetscale.z;
			spTrans.localPosition = offset;

			Vector3 scale = Vector3.zero;
			scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
			scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
			scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

			scale.y = 1 - (Mathf.Abs(offset.y) / sphereOffsetscale.x);
			scale.y = Mathf.Max(scale.y, scaleYMin);

			spTrans.localScale = scale;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Restart();
		}
	}

	private void Restart()
	{
		foreach (GameObject sp in spheres)
		{
			Destroy(sp);
		}
		Start();
	}
}
