using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectleLine : MonoBehaviour
{
	static public ProjectleLine S;

	[Header("Set in Inspector")]
	public float minDist = 0.1f;

	private LineRenderer _line;
	private GameObject _poi;
	private List<Vector3> points;

	private void Awake()
	{
		S = this;
		_line = GetComponent<LineRenderer>();
		_line.enabled = false;
		points = new List<Vector3>();
	}

	public GameObject poi
	{
		get
		{
			return (_poi);
		}
		set
		{
			_poi = value;
			if (_poi != null)
			{
				_line.enabled = false;
				points = new List<Vector3>();
				AddPoint();
			}
		}
	}

	public void Clear()
	{
		_poi = null;
		_line.enabled = false;
		points = new List<Vector3>();
	}

	public void AddPoint()
	{
		Vector3 pt = _poi.transform.position;
		if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
		{
			return;
		}
		if (points.Count == 0)
		{
			Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
			points.Add(pt + launchPosDiff);
			points.Add(pt);
			_line.positionCount = 2;
			_line.SetPosition(0, points[0]);
			_line.SetPosition(1, points[1]);
			_line.enabled = true;
		}
		else
		{
			points.Add(pt);
			_line.positionCount = points.Count;
			_line.SetPosition(points.Count - 1, lastPoint);
			_line.enabled = true;
		}
	}

	public Vector3 lastPoint
	{
		get
		{
			if (points == null)
			{
				return Vector3.zero;
			}
			return points[points.Count - 1];
		}
	}

	private void FixedUpdate()
	{
		if (poi == null)
		{
			if (FollowCam.POI != null)
			{
				if (FollowCam.POI.tag == "Projectle")
				{
					poi = FollowCam.POI;
				}
				else
				{
					return;
				}
			}
			else
			{
				return;
			}
		}
		AddPoint();
		if (FollowCam.POI == null)
		{
			poi = null;
		}
	}
}
