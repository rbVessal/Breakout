using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	public delegate void PlayerLostAllBallsAction();
	public static event PlayerLostAllBallsAction PlayerLostAllBalls;
	public delegate void PlayerLostABallAction();
	public static event PlayerLostABallAction PlayerLostABall;

	public int startingLives = 1;
	private int lives;
	private GameObject paddleGameObject;
	private List<GameObject>balls;
	// Use this for initialization
	void Awake () 
	{
		if(startingLives > 0)
		{
			lives = startingLives;
		}
		else
		{
			lives = 1;
		}
		balls = new List<GameObject>();
		//Create the paddle and the ball
		CreatePaddleAndBalls();
		Ball.Destroyed += RemoveBall;
		GoldenBar.GoldenBarDestroyed += DoubleBallSize;
	}

	public int Lives
	{
		get{return lives;}
	}

	private void DoubleBallSize(GameObject gameObject)
	{
		DoubleBallSizePowerUp.Activate(ref balls);
	}

	public void Reset()
	{
		lives = startingLives;
		RemovePaddleAndBalls();
		CreatePaddleAndBalls();
	}

	void CreatePaddleAndBalls()
	{
		GameObject paddleAsset = Resources.Load("Prefabs/Paddle") as GameObject;
		Paddle paddle = paddleAsset.GetComponent<Paddle>();
		paddleGameObject = Instantiate(paddleAsset, paddle.StartingPosition, Quaternion.identity) as GameObject;

		CreateBall();
	}

	void CreateBall()
	{
		GameObject ballGameObject = Resources.Load ("Prefabs/Ball") as GameObject;
		Ball ball = ballGameObject.GetComponent<Ball>();
		GameObject ballCloneGameObject = Instantiate(ballGameObject, ball.CenterPositionOverRelativeTarget(paddleGameObject), Quaternion.identity) as GameObject;

		//Parent the ball to the paddle, so when the paddle moves, the ball
		//moves along with it
		ballCloneGameObject.transform.parent = paddleGameObject.transform;
		balls.Add(ballCloneGameObject);
	}

	void RemoveBall(GameObject ball)
	{
		lives--;
		balls.Remove(ball);
		if(balls.Count == 0 
		   && lives == 0)
		{
			Player.PlayerLostAllBalls();
		}
		else
		{
			ResetBallAndPaddle();
			Player.PlayerLostABall();
		}
	}

	void ResetBallAndPaddle()
	{
		Paddle paddle = paddleGameObject.GetComponent<Paddle>();
		paddleGameObject.transform.position = paddle.StartingPosition;

		CreateBall();
	}

	void RemovePaddleAndBalls()
	{
		Destroy(paddleGameObject);
		for(int i = balls.Count - 1; i > -1; i--)
		{
			GameObject ball = balls[i];
			ball.SetActive(false);
			balls.Remove(ball);
			Destroy (ball);
		}
	}
}
