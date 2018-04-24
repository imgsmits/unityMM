using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
 * Logs data to the mysql database.
 */
public class DataLogger : MonoBehaviour
{
    // The url to request
    public string url = "http://localhost/masterymind/log.php";

    // The token for the hash-check
    public string token = "gSBlq5kjt7ZyqYlIgogSl";

    // The event identifiers
    public static string EVENT_SESSION_START = "EVENT_SESSION_START";
    public static string EVENT_SESSION_USERNAME = "EVENT_SESSION_USERNAME";
    public static string EVENT_GAME_START = "EVENT_GAME_START";
    public static string EVENT_GAME_MOVE = "EVENT_GAME_MOVE";
    public static string EVENT_GAME_QUIT_AVAILABLE = "EVENT_GAME_QUIT_AVAILABLE";
    public static string EVENT_GAME_HINT = "EVENT_GAME_HINT";
    public static string EVENT_GAME_CHEAT = "EVENT_GAME_CHEAT";
    public static string EVENT_GAME_LB_SHOW = "EVENT_GAME_LB_SHOW";
    public static string EVENT_GAME_LB_HIDE = "EVENT_GAME_LB_HIDE";
    public static string EVENT_GAME_SKIPPED = "EVENT_GAME_SKIPPED";
    public static string EVENT_GAME_QUIT = "EVENT_GAME_QUIT";
    public static string EVENT_GAME_LOST = "EVENT_GAME_LOST";
    public static string EVENT_GAME_WON = "EVENT_GAME_WON";
    public static string EVENT_GAME_END = "EVENT_GAME_END";
    public static string EVENT_DIFFICULTY_SHOW = "EVENT_DIFFICULTY_SHOW";
    public static string EVENT_DIFFICULTY_SET = "EVENT_DIFFICULTY_SET";
    public static string EVENT_DIFFICULTY_HIDE = "EVENT_DIFFICULTY_HIDE";
    public static string EVENT_SESSION_END = "EVENT_SESSION_END";


    IEnumerator PostForm(string eventName,int param1,int param2,int param3,string param4,float param5)
    {
        // Create a new form
        WWWForm form = new WWWForm();
        
        // Add the default fields
        form.AddField( "token", token );
        form.AddField( "timestamp", Time.time.ToString() );
        form.AddField( "userid", PlayerPrefs.GetString("userid") );

        // Add the event and parameter fields
        form.AddField( "event", eventName );
        form.AddField( "param1", param1 );
        form.AddField( "param2", param2 );
        form.AddField( "param3", param3 );
        form.AddField( "param4", param4 );
        form.AddField( "param5", param5.ToString() );

        // Post the request using a coroutine
        using( var w = UnityWebRequest.Post( url, form ) )
        {
            yield return w.SendWebRequest();
            if( w.isNetworkError || w.isHttpError )
                Debug.Log( w.error );
        }
    }

    public void Log( string eventName, int param1, int param2, int param3, string param4, float param5 )
    {
        StartCoroutine( PostForm(eventName,param1,param2,param3,param4,param5) );
    }
}
