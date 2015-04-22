using UnityEngine;
using System.Collections;

public class BirdFlapBrid : MonoBehaviour {

	public BirdFlap birdFlap;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "pipe")
		{
			birdFlap.SendMessage("BirdDeath", SendMessageOptions.DontRequireReceiver);
		}
	}
}
