using UnityEngine;
using System.IO;

public class TextFileReader : MonoBehaviour 
{
	//Read the text file found in Assets/Resources
	public static string ReadTextFile(string textFileNamePath)
	{
		TextAsset levelData = Resources.Load(textFileNamePath) as TextAsset;
		if(levelData)
		{
			return levelData.text;
		}
		else
		{
			Debug.Log("Could not find text file given the path: " + textFileNamePath + " Please check that the file is correctly name and in the correct path.");
			return null;
		}
	}
}
