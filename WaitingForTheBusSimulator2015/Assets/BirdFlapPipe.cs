using UnityEngine;
using System.Collections;

public class BirdFlapPipe : MonoBehaviour {

	private BirdFlap flapBrid;

	// Use this for initialization
	void Start () {
		GameObject brid = GameObject.Find ("OVRPlayerController");

		if(brid == null)
		{
			brid = GameObject.Find ("First Person Controller");
		}
		flapBrid = brid.GetComponentInChildren<BirdFlap> ();
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name == "Brid")
		{
			flapBrid.AddScore();
		}
	}
}
