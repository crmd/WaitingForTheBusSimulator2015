using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdFlap : MonoBehaviour {

	GameObject brid;
	GameObject board;
	public float flapMultiplier = 1000.0f;
	List<GameObject> activePipes = new List<GameObject>();
	public GameObject pipes;
	public float pipeSpeed = 2.0f;
	public int numPipes = 10;

	private Transform player;

	public bool started = false;

	public int score;

	// Use this for initialization
	void Start () {
		brid = GameObject.Find ("Brid");
		board = GameObject.Find ("BirdFlap");
		if(pipes)
		for (int i = 0; i < numPipes; ++i)
		{
			GameObject pipe = Instantiate(pipes, new Vector3(board.transform.position.x + 10.0f * (i+1),
			                                                 board.transform.position.y + Random.Range(-2.0f, 2.0f),
			                                                 board.transform.position.z), Quaternion.identity) as GameObject;
			activePipes.Add(pipe);
		}
		player = transform.parent;
		brid.rigidbody.useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(started)
		{
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				brid.rigidbody.velocity = Vector3.up * flapMultiplier;
			}
			for(int i = 0; i < activePipes.Count; ++i)
			{
				activePipes[i].transform.position = new Vector3(activePipes[i].transform.position.x - pipeSpeed * Time.deltaTime,
				                                                activePipes[i].transform.position.y,
				                                                activePipes[i].transform.position.z);
				if(activePipes[i].transform.position.x < board.transform.position.x - 20.0f)
				{
					GameObject oldPipe = activePipes[i];
					activePipes[i] = Instantiate(pipes, new Vector3(board.transform.position.x + 10.0f * (activePipes.Count - 2),
					                                                board.transform.position.y + Random.Range(-2.0f, 2.0f),
					                                                board.transform.position.z), Quaternion.identity) as GameObject;
					GameObject.Destroy(oldPipe);
				}
			}
		}
		else
		{
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				brid.rigidbody.useGravity = true;
				brid.rigidbody.velocity = Vector3.up * flapMultiplier;
				started = true;
			}
		}

		if(player.gameObject.name == "OVRPlayerController")
		{
			Vector3 tempVec;
			//Set Position
			tempVec = new Vector3(0.0f, -0.287f, 0.198f);
			transform.position = (new Vector3(tempVec.x,
			                                  tempVec.y * player.forward.y,
			                                  tempVec.z));
			//Set Rotation
			tempVec = new Vector3(330.13f, 180.0f, 0.0f);
			transform.localRotation = Quaternion.Euler(new Vector3(tempVec.x * player.forward.x,
			                                                       tempVec.y * player.forward.y,
			                                                       tempVec.z * player.forward.z));
		}
	}

	void FixedUpdate()
	{
		if(started)
		{
			brid.rigidbody.AddForce (-Vector3.up * 5);
		}
	}

	public void AddScore()
	{
		Debug.Log ("AddingScore");
		score++;
	}
}
