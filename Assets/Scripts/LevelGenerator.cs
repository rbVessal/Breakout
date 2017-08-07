using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour 
{
	private const int MAX_NUMBER_OF_ROWS = 4;
	private const int MAX_NUMBER_OF_COLUMNS = 8;

	public const int MAX_NUMBER_OF_LEVELS = 2;
	private const string LEVELS_PATH = "Levels/level";
	public static int levelNumber = 1;

	public static void StartLevel(ref List<GameObject>bricks)
	{
		CreateLevel(LEVELS_PATH + 1, ref bricks);
	}

	public static void CurrentLevel(ref List<GameObject>bricks)
	{
		CreateLevel(LEVELS_PATH + levelNumber, ref bricks);
	}
	
	public static void NextLevel(ref List<GameObject>bricks)
	{
		if(levelNumber < MAX_NUMBER_OF_LEVELS)
		{
			levelNumber++;
		}
		CreateLevel(LEVELS_PATH + levelNumber, ref bricks);
	}

	private static void CreateLevel(string levelNamePath, ref List<GameObject>bricks)
	{
		//Load the level from the text fiel
		StringReader stringReader;
		LoadLevel(ref levelNamePath, out stringReader);

		//Create the level
		GenerateBricksInLevel(ref stringReader, ref bricks);
	}

	private static void GenerateBricksInLevel(ref StringReader stringReader, ref List<GameObject>bricks)
	{
		GameObject brickPrefab = Resources.Load("Prefabs/Brick") as GameObject;
		GameObject goldenBarPrefab = Resources.Load ("Prefabs/GoldenBar") as GameObject;
		
		Boundary screenBoundary = ScreenBoundary.GetBoundary();
		Vector3 brickSize = new Vector3(screenBoundary.width/MAX_NUMBER_OF_COLUMNS, 
		                                (screenBoundary.height/(MAX_NUMBER_OF_ROWS * (MAX_NUMBER_OF_COLUMNS/MAX_NUMBER_OF_ROWS))),
		                                brickPrefab.transform.localScale.z);
		brickPrefab.transform.localScale = brickSize;
		goldenBarPrefab.transform.localScale = brickSize;
		
		string line;
		Vector3 brickPosition = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, 0.0f));
		int rows = 0;
		int columns = 0;
		while((line = stringReader.ReadLine()) != null)
		{
			if(rows < MAX_NUMBER_OF_ROWS)
			{
				char[] textFileContentArray = line.ToCharArray();
				bool firstBrick = true;
				//Use that content to create the breakout level
				foreach(char character in textFileContentArray)
				{
					//Calculate the brick position regardless
					//of whether there is a character representing a brick
					if(columns < MAX_NUMBER_OF_COLUMNS)
					{
						if(firstBrick)
						{
							brickPosition.x += brickSize.x/2;
							if(rows == 0)
							{
								brickPosition.y = screenBoundary.top - (brickSize.y/2);
							}
							else
							{
								brickPosition.y = screenBoundary.top - (brickSize.y * (((float)rows + 1.0f) - 0.5f));
							}
							firstBrick = false;
						}
						else
						{
							brickPosition.x += brickSize.x;
						}
						brickPosition.z = Camera.main.nearClipPlane;
						columns++;
						switch(character)
						{
							case 'B':
							{
								GameObject brick = Instantiate(brickPrefab, brickPosition, Quaternion.identity) as GameObject;
								bricks.Add (brick);
								break;
							}
							case 'G':
							{
								GameObject brick = Instantiate(goldenBarPrefab, brickPosition, Quaternion.identity) as GameObject;
								bricks.Add (brick);						
								break;
							}
							default:
							{
								break;
							}
						}
					}
					else
					{
						break;
					}
				}
				
				rows++;
				columns = 0;
				//Reset the x position of the brick position to the left side of the screen
				brickPosition.x = screenBoundary.left;
			}
			else
			{
				break;
			}
		}
	}

	private static void LoadLevel(ref string levelNamePath, out StringReader stringReader)
	{
		//Read the level text file
		string textFileContent = TextFileReader.ReadTextFile(levelNamePath);
		stringReader = new StringReader(textFileContent);
	}
}
