using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game : MonoBehaviour 
{	
	enum GameState
	{
		NewGame,
		Reset,
		NextLevel,
		Won,
		GameOver
	}

	public Text livesText;
	public Text gameStateText;
	private List<GameObject> bricks;
	private GameState gameState;
	private Player player;
	// Use this for initialization
	void Start () 
	{
		NewGame();
	}

	void NewGame()
	{
		bricks = new List<GameObject>();
		LevelGenerator.StartLevel(ref bricks);
	
		GameObject playerGameObject = GameObject.FindWithTag("Player");
		player = playerGameObject.GetComponent<Player>() as Player;

		UpdateLivesText();
		gameStateText.enabled = false;

		Player.PlayerLostABall += UpdateLivesText;
		Player.PlayerLostAllBalls += GameOver;
		Brick.BrickDestroyed += checkForVictory;
		GoldenBar.GoldenBarDestroyed += checkForVictory;

		gameState = GameState.NewGame;
	}

	void Reset()
	{
		player.Reset();
		UpdateLivesText();
		RemoveAllBricks();
		LevelGenerator.CurrentLevel(ref bricks);
		gameStateText.enabled = false;
	}

	void NextLevel()
	{
		Reset();
		LevelGenerator.NextLevel(ref bricks);
	}

	void checkForVictory(GameObject brick)
	{
		bricks.Remove(brick);
		if(bricks.Count == 0)
		{
			gameState = GameState.Won;
			switchGameStates();
		}
	}

	void switchGameStates()
	{
		switch(gameState)
		{
			case GameState.Won:
			{
				if(LevelGenerator.levelNumber < LevelGenerator.MAX_NUMBER_OF_LEVELS)
				{
					UpdateGameStatesText("You Won!  Press [ENTER] to restart or n to go to next level.");
				}
				else
				{
					UpdateGameStatesText("You Won!  Press [ENTER] to restart.");
				}
				break;
			}
			case GameState.GameOver:
			{
				UpdateGameStatesText("GameOver!  Press [ENTER] to restart.");
				break;
			}
			case GameState.Reset:
			{
				Reset();
				break;
			}
			case GameState.NextLevel:
			{
				NextLevel();
				break;
			}
			default:
			{
				Debug.Log ("gameState set to invalid GameState");
				break;
			}
		}
	}

	void UpdateGameStatesText(string text)
	{
		gameStateText.enabled = true;
		gameStateText.text = text;
	}

	void UpdateLivesText()
	{
		livesText.text = "Lives: " + player.Lives;
	}

	void RemoveAllBricks()
	{
		for(int i = bricks.Count - 1; i > -1; i--)
		{
			GameObject brick = bricks[i];
			bricks.Remove(brick);
			Destroy(brick);
		}
	}

	void GameOver()
	{
		UpdateLivesText();
		//If the player didn't win already
		if(gameState != GameState.Won)
		{
			gameState = GameState.GameOver;
			switchGameStates();
		}
	}

	void FixedUpdate()
	{
		if(Input.GetKeyDown(KeyCode.Return))
		{
			gameState = GameState.Reset;
			switchGameStates();
		}
		else if(Input.GetKeyDown(KeyCode.N))
		{
			if(bricks.Count == 0)
			{
				gameState = GameState.NextLevel;
				switchGameStates();
			}
		}
		else if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
