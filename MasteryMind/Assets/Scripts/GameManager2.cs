using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// TODO: disabling all buttons is not enough; we need to limit interaction through a bool

public class GameManager2 : MonoBehaviour
{
    // Structure for the faux leaderboard data
    struct LeaderboardItem
    {
        public string name;
        public int score;
        public LeaderboardItem(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    // Fill the faux leaderboard data
    LeaderboardItem[] leaderboardItems =
    {
       new LeaderboardItem("Albert",7500),
       new LeaderboardItem("Bert",6500),
       new LeaderboardItem("Cecilia",6250),
       new LeaderboardItem("Davita",5750),
       new LeaderboardItem("Fanne",5300),
       new LeaderboardItem("Anneke",5200),
       new LeaderboardItem("Nick",4850),
       new LeaderboardItem("Alexander",4550),
       new LeaderboardItem("Joost",4200),
       new LeaderboardItem("Robert",3950),
       new LeaderboardItem("player",0)

    };

    // The bead sets on screen
    BeadSet[] beadSets = null;
    int currentBeadSet = 0;
    
    // The code to be guessed
    Code solution;

    // The player
    Player player;

    // The AI-computer player
    Computer parallelComputer;

    // State flags
    bool solutionGuessed = false;
    bool outOfTurns = false;
    public bool paused = false;
    bool usedHint = false;
    bool usedCheat = false;
    bool quitAvailable = false;

    // Initial settings
    public int numStartDigits = 3;
    public int numStartColors = 4;
    public int secondsUntilQuitOption = 2 * 60;

    // Scoring settings
    public int ptsForRightPlace = 10;
	public int ptsForRightColor = 5;
    public int ptsForSolvingPerTurn = 50;

    float lastTimeLB = 0.0f;

    /**
     * Initializes the game.
     */
    void Start()
    {
        // If the application has run for less then 10 seconds assume it is a new session
        if( PlayerPrefs.GetInt( "resetSession" ) == 1 || Time.time < 10.0f )
        {
            // Take set values for difficulty and score
            Code.numDigits = numStartDigits;
            Code.numColors = numStartColors;

            // Save difficulty to playerprefs
            PlayerPrefs.SetInt( "numDigits", Code.numDigits );
            PlayerPrefs.SetInt( "numColors", Code.numColors );

            // Reset options
            PlayerPrefs.SetInt( "secondsUntilQuitOption", secondsUntilQuitOption );
            PlayerPrefs.SetInt( "playerScore", 0 );
            PlayerPrefs.SetInt( "resetSession", 0 );
        }
        else
        {
            // Fetch from playerprefs
            Code.numDigits = PlayerPrefs.GetInt( "numDigits" );
            Code.numColors = PlayerPrefs.GetInt( "numColors" );
        }

        // Fetch the end-of-game panel and the leaderboard and hide it
        GameObject.Find( "Canvas" ).transform.Find( "CompletedPanel" ).gameObject.SetActive( false );
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).gameObject.SetActive( false );

        // Disable the quit button
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonQuit" ).GetComponent<Button>().interactable = false;

        // Adjust available colors to num digits
        Transform ac = GameObject.Find( "ColorsAvailable" ).transform;
        for( int i = Code.numColors; i < ac.transform.childCount; i++ )
            ac.GetChild( i ).gameObject.SetActive( false );

        // Fetch the bead sets from the scene and sort vertically
        beadSets = GameObject.FindObjectsOfType<BeadSet>();
        for( int i = 0; i < beadSets.Length; i++ )
        {
            beadSets[i].Initialize();
            beadSets[i].MakeUneditable();
            beadSets[i].Hide();
        }
        System.Array.Sort( beadSets, YPositionComparison );

        // Enable the first bead set
        currentBeadSet = 0;
        beadSets[currentBeadSet].ShowBeads();
        beadSets[currentBeadSet].MakeEditable();

        // Pick a code to guess
        solution = new Code();
        solution.MakeRandom();

        // Prepare player and AI-computer
        player = new Player();
        parallelComputer = new Computer();
        parallelComputer.PrepareGuess();

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_START, Code.numDigits, Code.numColors, beadSets.Length, solution.ToString(), 0.0f );

        // Update the UI
        UpdateUI();
    }

    /**
     * Applies a move in the game.
     */
    public Feedback ApplyMove( Code guess )
    {
        // Compute feedback
        Feedback fb = GiveFeedback( guess );

        // Give points for feedback
        player.score += ptsForRightPlace * fb.numRightPlace + ptsForRightColor * fb.numRightColor;

        // Advance player and AI-computer turns
        player.AdvanceTurn( guess, fb );
        parallelComputer.AdvanceTurn( guess, fb );

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_MOVE, fb.numRightPlace, fb.numRightColor, player.turn, guess.ToString(), 0.0f );

        // Check if it is correct
        if( fb.IsCorrect() )
        {
            player.score += (beadSets.Length-player.turn) * ptsForSolvingPerTurn;
            solutionGuessed = true;

            // Log
            GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_WON, player.score, 0, player.turn, "", 0.0f );
        }

        // Update the UI
        UpdateUI();

        // Return the feedback
        return fb;
    }

    /**
     * Completes a move.
     */
    public void CompleteMove()
    {
        // Disable the current bead set
        beadSets[currentBeadSet].MakeUneditable();

        // Check lose condition
        outOfTurns = ++currentBeadSet >= beadSets.Length;
        if ( outOfTurns )
        {
            // Log
            GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_LOST, player.score, 0, player.turn, "", 0.0f );
        }

        // Check for win/lose condition
        if( solutionGuessed || outOfTurns )
        {
            CompleteGame();
        }
        else
        {
            // Enable next
            beadSets[currentBeadSet].MakeEditable();
            parallelComputer.PrepareGuess();

            // Update the UI
            UpdateUI();
        }
    }

    void Update()
    {
        if( paused ) return;

        // Enable the quit button if time has passed
        if( !quitAvailable && Time.time >= secondsUntilQuitOption )
        {
            GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonQuit" ).GetComponent<Button>().interactable = true;
            quitAvailable = true;

            // Log
            GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_QUIT_AVAILABLE, 0, 0, 0, "", 0.0f );
        }
    }

    public void OnHint()
    {
        if( paused ) return;

        // Provide a hint from the AI-computer
        beadSets[currentBeadSet].SetToCode( parallelComputer.guess );

        // Disable the hint button
        usedHint = true;
        GameObject.Find( "Canvas" ).transform.Find("ButtonPanel").Find( "ButtonHint" ).GetComponent<Button>().interactable = false;

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_HINT, 0, 0, 0, parallelComputer.guess.ToString(), 0.0f );
    }

    public void OnCheat()
    {
        if( paused ) return;

        // Provide a cheat from the solution
        int index = Random.Range( 0, Code.numDigits - 1 );
        beadSets[currentBeadSet].SetBeadToColor( index, solution.code[index] );

        // Disable the cheat button
        usedCheat = true;
        GameObject.Find( "Canvas" ).transform.Find("ButtonPanel").Find( "ButtonCheat" ).GetComponent<Button>().interactable = false;

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_CHEAT, index, solution.code[index], player.turn, "", 0.0f );
    }

    public void OnLeaderboardShow()
    {
        // Pause
        paused = true;

        // Disable all buttons
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonHint" ).GetComponent<Button>().interactable = false;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonCheat" ).GetComponent<Button>().interactable = false;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonLeaderboard" ).GetComponent<Button>().interactable = false;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonSkip" ).GetComponent<Button>().interactable = false;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonQuit" ).GetComponent<Button>().interactable = false;

        // Update player slot in leaderboard data
        string username = PlayerPrefs.GetString( "username" );
        for(int i=0;i<leaderboardItems.Length;i++)
            if(leaderboardItems[i].name=="player" || leaderboardItems[i].name== username)
            {
                leaderboardItems[i].name = username;
                leaderboardItems[i].score = player.score;
            }

        // Sort leaderboard data
        System.Array.Sort( leaderboardItems, LBIComparison );

        // Find the player's position
        int playerRank = 0;
        for( int i = 0; i < leaderboardItems.Length; i++ )
            if( leaderboardItems[i].name == username )
                playerRank = i;

        // Fill in the leaderboard data
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader1" ).GetComponent<Text>().text = "1. " + leaderboardItems[0].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore1" ).GetComponent<Text>().text = leaderboardItems[0].score.ToString();
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader2" ).GetComponent<Text>().text = "2. " + leaderboardItems[1].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore2" ).GetComponent<Text>().text = leaderboardItems[1].score.ToString();
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader3" ).GetComponent<Text>().text = "3. " + leaderboardItems[2].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore3" ).GetComponent<Text>().text = leaderboardItems[2].score.ToString();
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader4" ).GetComponent<Text>().text = "4. " + leaderboardItems[3].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore4" ).GetComponent<Text>().text = leaderboardItems[3].score.ToString();
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader5" ).GetComponent<Text>().text = "5. " + leaderboardItems[4].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore5" ).GetComponent<Text>().text = leaderboardItems[4].score.ToString();
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader6" ).GetComponent<Text>().text = "6. " + leaderboardItems[5].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore6" ).GetComponent<Text>().text = leaderboardItems[5].score.ToString();
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader7" ).GetComponent<Text>().text = "7. " + leaderboardItems[6].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore7" ).GetComponent<Text>().text = leaderboardItems[6].score.ToString();
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader8" ).GetComponent<Text>().text = "8. " + leaderboardItems[7].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore8" ).GetComponent<Text>().text = leaderboardItems[7].score.ToString();
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader9" ).GetComponent<Text>().text = "9. " + leaderboardItems[8].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore9" ).GetComponent<Text>().text = leaderboardItems[8].score.ToString();
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextLeader10" ).GetComponent<Text>().text = "10. " + leaderboardItems[9].name;
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).Find( "TextScore10" ).GetComponent<Text>().text = leaderboardItems[9].score.ToString();

        // Show the leaderboard
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).gameObject.SetActive( true);

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_LB_SHOW, player.score,0,playerRank, "", 0.0f );
        lastTimeLB = Time.time;
    }

    public void OnLeaderBoardHide()
    {
        // Find the player's position
        string username = PlayerPrefs.GetString( "username" );
        int playerRank = 0;
        for( int i = 0; i < leaderboardItems.Length; i++ )
            if( leaderboardItems[i].name == username )
                playerRank = i;

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_LB_HIDE, player.score, 0, playerRank, "", Time.time-lastTimeLB );

        // Hide the board
        GameObject.Find( "Canvas" ).transform.Find( "LeaderboardPanel" ).gameObject.SetActive( false );

        // Enable all buttons
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonHint" ).GetComponent<Button>().interactable = !usedHint;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonCheat" ).GetComponent<Button>().interactable = !usedCheat;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonLeaderboard" ).GetComponent<Button>().interactable = true;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonSkip" ).GetComponent<Button>().interactable = true;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonQuit" ).GetComponent<Button>().interactable = quitAvailable;

        // Unpause
        paused = false;
    }


    public void OnSkipGame()
    {
        if( paused ) return;

        PlayerPrefs.SetInt( "playerScore", player.score );

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_SKIPPED, player.score, 0, player.turn, "", 0.0f );

        CompleteGame();
    }

    public void OnQuit()
    {
        if( paused ) return;

        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_QUIT, player.score, 0, player.turn, "", 0.0f );
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_END, 0, 0, 0, "", 0.0f );

        SceneManager.LoadScene( "EndScreen" );
    }

    public void OnContinue()
    {
        // Log
        GetComponent<DataLogger>().Log( DataLogger.EVENT_GAME_END, 0, 0, 0, "", 0.0f );

        // Go to the select-difficulty-screen
        SceneManager.LoadScene( "DifficultyScreen" );
    }


    public void CompleteGame()
    {
        paused = true;

        // Disable all buttons
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonHint" ).GetComponent<Button>().interactable = false;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonCheat" ).GetComponent<Button>().interactable = false;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonLeaderboard" ).GetComponent<Button>().interactable = false;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonSkip" ).GetComponent<Button>().interactable = false;
        GameObject.Find( "Canvas" ).transform.Find( "ButtonPanel" ).Find( "ButtonQuit" ).GetComponent<Button>().interactable = false;

        // Save score
        PlayerPrefs.SetInt( "playerScore", player.score );

        // Fetch the end-of-game panel
        Transform panel = GameObject.Find( "Canvas" ).transform.Find( "CompletedPanel" ).transform;
        if( solutionGuessed )
            panel.Find( "TextResult" ).GetComponent<Text>().text = "YOU WON!";
        else if( outOfTurns )
            panel.Find( "TextResult" ).GetComponent<Text>().text = "YOU LOST!";
        else
            panel.Find( "TextResult" ).GetComponent<Text>().text = "YOU SKIPPED";
        panel.Find( "TextMessage" ).GetComponent<Text>().text = "You took " + player.turn + " turns.";
        panel.gameObject.SetActive( true );
    }

    public void UpdateUI()
    {
        GameObject canvas = GameObject.Find( "Canvas" );
        canvas.transform.Find( "UserPanel" ).Find( "TextUsername" ).GetComponent<Text>().text = PlayerPrefs.GetString( "username" );
        canvas.transform.Find( "UserPanel" ).Find( "TextScore" ).GetComponent<Text>().text = player.score.ToString();
    }

    public Feedback GiveFeedback( Code guess )
    {
        return Code.CalculateFeedback( guess, solution );
    }

    private int YPositionComparison( BeadSet a, BeadSet b )
    {
        var ya = a.transform.position.y;
        var yb = b.transform.position.y;
        return ya.CompareTo( yb );
    }

    private int LBIComparison( LeaderboardItem a, LeaderboardItem b )
    {
        return b.score.CompareTo( a.score );
    }
}
