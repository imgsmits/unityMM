using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterDifficulty : MonoBehaviour
{
	int difficultyLevel = 0;
	int[] numColorsForLevel = {4,4,4,5,5,5,6,6};
	int[] numDigitsForLevel = {3,4,5,4,5,6,5,6};
	
    float lastTimeDif = 0.0f;

    void Start()
    {
		
		// Disable easier/harder if unavailable
		difficultyLevel = PlayerPrefs.GetInt("difficultyLevel");
		if ( difficultyLevel == 0 )
			GameObject.Find( "Canvas" ).transform.Find( "DifficultyPanel" ).Find( "ButtonEasier" ).GetComponent<Button>().interactable = false;
		else if ( difficultyLevel == numColorsForLevel.Length -1 )
			GameObject.Find( "Canvas" ).transform.Find( "DifficultyPanel" ).Find( "ButtonHarder" ).GetComponent<Button>().interactable = false;

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_SHOW, 0, 0, 0, "", 0.0f );
        lastTimeDif = Time.time;
    }

    public void OnEasier()
    {
        // Make easier
		difficultyLevel = PlayerPrefs.GetInt("difficultyLevel");
		difficultyLevel = Mathf.Max(0,difficultyLevel - 1);
        Code.numDigits = numDigitsForLevel[difficultyLevel];
        Code.numColors = numColorsForLevel[difficultyLevel];

        // Save to playerprefs
        PlayerPrefs.SetInt( "numDigits", Code.numDigits );
        PlayerPrefs.SetInt( "numColors", Code.numColors );
		PlayerPrefs.SetInt( "difficultyLevel", difficultyLevel );

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_SET, Code.numDigits, Code.numColors, 0, "EASIER", 0.0f );
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_HIDE, 0, 0, 0, "", Time.time - lastTimeDif );

        // Start game
        SceneManager.LoadScene( "GameScreen" );
    }

    public void OnTheSame()
    {
        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_SET, Code.numDigits, Code.numColors, 0, "SAME", 0.0f );
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_HIDE, 0, 0, 0, "", Time.time - lastTimeDif );

        // Start game
        SceneManager.LoadScene( "GameScreen" );
    }

    public void OnHarder()
    {
        // Make harder
		difficultyLevel = PlayerPrefs.GetInt("difficultyLevel");
		difficultyLevel = Mathf.Max(0,difficultyLevel + 1);
        Code.numDigits = numDigitsForLevel[difficultyLevel];
        Code.numColors = numColorsForLevel[difficultyLevel];

        // Save to playerprefs
        PlayerPrefs.SetInt( "numDigits", Code.numDigits );
        PlayerPrefs.SetInt( "numColors", Code.numColors );
		PlayerPrefs.SetInt( "difficultyLevel", difficultyLevel );

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_SET, Code.numDigits, Code.numColors, 0, "HARDER", 0.0f );
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_HIDE, 0, 0, 0, "", Time.time - lastTimeDif );

        // Start game
        SceneManager.LoadScene( "GameScreen" );
    }
}
