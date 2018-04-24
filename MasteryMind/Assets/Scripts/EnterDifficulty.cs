using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterDifficulty : MonoBehaviour
{
    float lastTimeDif = 0.0f;

    void Start()
    {
        // Enable/disable quit button based on playing time
        if( Time.time >= PlayerPrefs.GetInt( "secondsUntilQuitOption" ) )
        {     
            GameObject.Find( "Canvas" ).transform.Find( "DifficultyPanel" ).Find( "ButtonQuit" ).GetComponent<Button>().interactable = true;
            GameObject.Find( "Canvas" ).transform.Find( "DifficultyPanel" ).Find( "TextMessage" ).GetComponent<Text>().text = "Please choose the difficulty of the next game, or choose to quit the game.";
        }
        else
        {
            GameObject.Find( "Canvas" ).transform.Find( "DifficultyPanel" ).Find( "ButtonQuit" ).GetComponent<Button>().interactable = false;
            GameObject.Find( "Canvas" ).transform.Find( "DifficultyPanel" ).Find( "TextMessage" ).GetComponent<Text>().text = "Please choose the difficulty of the next game.";
        }

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_SHOW, 0, 0, 0, "", 0.0f );
        lastTimeDif = Time.time;
    }

    public void OnEasier()
    {
        // Make easier
        Code.numDigits = Mathf.Max( 1, PlayerPrefs.GetInt( "numDigits" ) - 1 );
        Code.numColors = Mathf.Max( 2, PlayerPrefs.GetInt( "numColors" ) - 1 );

        // Save to playerprefs
        PlayerPrefs.SetInt( "numDigits", Code.numDigits );
        PlayerPrefs.SetInt( "numColors", Code.numColors );

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
        // Make easier
        Code.numDigits = Mathf.Min( 5, PlayerPrefs.GetInt( "numDigits" ) + 1 );
        Code.numColors = Mathf.Min( 6, PlayerPrefs.GetInt( "numColors" ) + 1 );

        // Save to playerprefs
        PlayerPrefs.SetInt( "numDigits", Code.numDigits );
        PlayerPrefs.SetInt( "numColors", Code.numColors );

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_SET, Code.numDigits, Code.numColors, 0, "HARDER", 0.0f );
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_HIDE, 0, 0, 0, "", Time.time - lastTimeDif );

        // Start game
        SceneManager.LoadScene( "GameScreen" );
    }

    public void OnQuit()
    {
        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_QUIT, 0, 0, 0, "", 0.0f );
        GetComponent<DataLogger>().Log( DataLogger.EVENT_DIFFICULTY_HIDE, 0, 0, 0, "", 0.0f );

        SceneManager.LoadScene( "EndScreen" );
    }
}
