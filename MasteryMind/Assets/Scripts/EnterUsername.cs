using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EnterUsername : MonoBehaviour
{
    GameObject canvas = null;
    Button buttonContinue = null;
    InputField inputUsername = null;

	void Start()
    {
        // This is the entrypoint of the application
        GetComponent<DataLogger>().Log( DataLogger.EVENT_SESSION_START, 0, 0, 0, "", 0.0f );

        // Fetch userid from URL
        string url = Application.absoluteURL;
        string userid = url.Substring(url.LastIndexOf("id=")+3);
        PlayerPrefs.SetString( "userid", userid );

        // Disable the continue button
        canvas = GameObject.Find( "Canvas" );
        buttonContinue = canvas.transform.Find( "WelcomePanel" ).Find( "ButtonContinue" ).GetComponent<Button>();
        buttonContinue.interactable = false;

        // Set focus to entry field
        inputUsername = canvas.transform.Find( "WelcomePanel" ).Find( "InputField" ).GetComponent<InputField>();
        EventSystem.current.SetSelectedGameObject( inputUsername.gameObject, null );
    }

    public void OnInputChanged()
    {
        // Allow only 3-or-more length names
        buttonContinue.interactable = (inputUsername.text.Length >= 3);
    }
	
	public void OnContinue()
    {
        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_SESSION_USERNAME, 0, 0, 0, inputUsername.text, 0.0f );

        // Save the username
        PlayerPrefs.SetString( "username", inputUsername.text );
        PlayerPrefs.SetInt( "resetSession", 1 );

        // Move to the next scene
        SceneManager.LoadScene( "GameScreen" );
    }
}
