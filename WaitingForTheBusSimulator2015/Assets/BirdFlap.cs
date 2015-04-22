using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum BirdFlapState
{
	BFS_Ready,
	BFS_Running,
	BFS_Dead
}

public class BirdFlap : MonoBehaviour {

	GameObject brid;
	GameObject board;
	public float flapMultiplier = 1000.0f;
	[SerializeField]
	List<GameObject> activePipes = new List<GameObject>();
	public GameObject pipes;
	public float pipeSpeed = 2.0f;
	private int numPipes = 10;

	private Transform player;

//	public bool started = false;

	public int score;
	public int highScore;

	private Vector3 bridStartPos;
	
	private Text titleText;
	private Text scoreText;
	private Text gameOverText;
	private Text gameOverScoreText;
	private Text highScoreText;
	private Text clickToFlapText;

	public AudioSource flappyAudio;
	public AudioClip flappyFlap;
	public AudioClip flappyDeath;

	public BirdFlapState birdFlapState = BirdFlapState.BFS_Ready;
	


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
		brid.GetComponent<BirdFlapBrid>().birdFlap = this;

		bridStartPos = brid.transform.position;

		//Find the flapBrid text objects
		titleText = GameObject.Find ("TitleText").GetComponent<Text>();
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text>();
		gameOverText = GameObject.Find ("GameOverText").GetComponent<Text>();
		gameOverScoreText = GameObject.Find ("GameOverScoreText").GetComponent<Text>();
		highScoreText = GameObject.Find ("GameOverHighScoreText").GetComponent<Text>();
		clickToFlapText = GameObject.Find ("ClickToFlapText").GetComponent<Text>();

		flappyAudio.clip = flappyFlap;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateText();
		switch(birdFlapState)
		{
		case BirdFlapState.BFS_Ready:
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				Flap ();
			}
			break;
		case BirdFlapState.BFS_Running:



			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				Flap ();
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
			break;
		case BirdFlapState.BFS_Dead:



			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				for(int i = 0; i < activePipes.Count; ++i)
				{
					GameObject.Destroy(activePipes[i]);
				}
				activePipes.Clear();
				SpawnPipes();
				brid.transform.position = bridStartPos;
				Flap ();
			}
			break;
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

	void SpawnPipes()
	{
		for (int i = 0; i < numPipes; ++i)
		{
			GameObject pipe = Instantiate(pipes, new Vector3(board.transform.position.x + 10.0f * (i+1),
			                                                 board.transform.position.y + Random.Range(-2.0f, 2.0f),
			                                                 board.transform.position.z), Quaternion.identity) as GameObject;
			activePipes.Add(pipe);
		}
	}

	void UpdateText()
	{
		switch(birdFlapState)
		{
		case BirdFlapState.BFS_Ready:
			titleText.enabled = true;
			scoreText.enabled = false;
			gameOverText.enabled = false;
			gameOverScoreText.enabled = false;
			highScoreText.enabled = false;
			clickToFlapText.enabled = true;
			break;
		case BirdFlapState.BFS_Running:
			titleText.enabled = false;
			scoreText.enabled = true;
			gameOverText.enabled = false;
			gameOverScoreText.enabled = false;
			highScoreText.enabled = false;
			clickToFlapText.enabled = false;
			scoreText.text = "Score: " + score.ToString();
			break;
		case BirdFlapState.BFS_Dead:
			titleText.enabled = false;
			scoreText.enabled = false;
			gameOverText.enabled = true;
			gameOverScoreText.enabled = true;
			highScoreText.enabled = true;
			clickToFlapText.enabled = true;
			highScoreText.text = "High Score: " + highScore.ToString();
			gameOverScoreText.text = "Score: " + score.ToString();
			break;
		}
	}

	void FixedUpdate()
	{
		if(birdFlapState == BirdFlapState.BFS_Running)
		{
			brid.rigidbody.AddForce (-Vector3.up * 5);
		}
	}

	private void Flap()
	{
		brid.collider.enabled = true;
		brid.rigidbody.useGravity = true;
		birdFlapState = BirdFlapState.BFS_Running;
		brid.rigidbody.velocity = Vector3.up * flapMultiplier;
		flappyAudio.clip = flappyFlap;
		flappyAudio.Play ();
	}

	public void BirdDeath()
	{
		birdFlapState = BirdFlapState.BFS_Dead;
		if(highScore < score)
		{
			highScore = score;
		}
		brid.collider.enabled = false;
		flappyAudio.clip = flappyDeath;
		flappyAudio.Play();
	}

	public void AddScore()
	{
		score++;
	}
}
