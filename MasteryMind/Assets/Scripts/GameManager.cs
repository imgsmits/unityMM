using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    BeadSet[] beadSets = null;
    int currentBeadSet = 0;

    Code solution;
    Player player;
    Computer parallelComputer;
    Computer independentComputer;
    
    void Start()
    {
        // Fetch the bead sets from the scene and sort vertically
        beadSets = GameObject.FindObjectsOfType<BeadSet>();
        for( int i = 0; i < beadSets.Length; i++ )
            beadSets[i].MakeUneditable();
        System.Array.Sort( beadSets, YPositionComparison );

        // Enable the first
        currentBeadSet = 0;
        beadSets[currentBeadSet].MakeEditable();

        // Pick a code
        solution = new Code();
        solution.MakeRandom();

        // Prepare players
        player = new Player();
        parallelComputer = new Computer();
        parallelComputer.PrepareGuess();
        independentComputer = new Computer();
        independentComputer.PrepareGuess();

        UpdateUI();

        Debug.Log( "SOLUTION = " + solution );
    }

    public Feedback ApplyMove( Code guess )
    {
        Debug.Log( "GUESS #" + player.turn + " = " + guess );
        Feedback fb = GiveFeedback( guess );
        player.AdvanceTurn( guess, fb );
        parallelComputer.AdvanceTurn( guess, fb );
        independentComputer.AdvanceTurn( independentComputer.guess, GiveFeedback( independentComputer.guess ) );

        if( fb.IsCorrect() )
            Debug.Log( "CODE GUESSED IN " + player.turn + " TURNS" );

        return fb;
    }

    public void CompleteMove()
    {
        // Disable last set and check for done
        beadSets[currentBeadSet].MakeUneditable();
        if ( ++currentBeadSet >= beadSets.Length )
        {
            Debug.Log( "NUMBER OF TURNS EXPRIED" );
            return;
        }

        // Enable next
        beadSets[currentBeadSet].MakeEditable();

        parallelComputer.PrepareGuess();
        independentComputer.PrepareGuess();

        UpdateUI();
    }

    public void UpdateUI()
    {
        GameObject canvas = GameObject.Find( "Canvas" );

        canvas.transform.Find( "PlayerInfoPanel" ).Find( "PlayerInfoTurn" ).GetComponent<Text>().text = "Turn " + (player.turn+1) + " of " + beadSets.Length;
        canvas.transform.Find( "PlayerInfoPanel" ).Find( "PlayerInfoPossibilities" ).GetComponent<Text>().text = "Possible solutions left: " + player.codeSet.numCodes;
        canvas.transform.Find( "PlayerInfoPanel" ).Find( "PlayerInfoEliminations" ).GetComponent<Text>().text = "Solutions eliminated: " + (player.codeSet.numOriginalCodes - player.codeSet.numCodes);
        canvas.transform.Find( "PlayerInfoPanel" ).Find( "PlayerInfoProgress" ).GetComponent<Text>().text = "Progress: " + Mathf.FloorToInt(100*((player.codeSet.numOriginalCodes - player.codeSet.numCodes) / (float)(player.codeSet.numOriginalCodes-1))) + "%";

        if( player.turn == 0 )
        {
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyLastEliminations" ).GetComponent<Text>().text = "Eliminations in last move: N/A";
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyCfParallelPrediction" ).GetComponent<Text>().text = "Eliminations predicted by par.cmp: N/A";
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyCfParrallelEfficiency" ).GetComponent<Text>().text = "Efficiency vs. par.cmp: N/A";
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyCfIndependentPrediction" ).GetComponent<Text>().text = "Eliminations predicted by par.cmp: N/A";
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyCfIndependentEfficiency" ).GetComponent<Text>().text = "Efficiency vs. par.cmp: N/A";
        }
        else
        {
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyLastEliminations" ).GetComponent<Text>().text = "Eliminations in last move: " + player.lastEliminations;
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyCfParallelPrediction" ).GetComponent<Text>().text = "Eliminations predicted by par.cmp: " + parallelComputer.lastPredictedEliminations;
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyCfParrallelEfficiency" ).GetComponent<Text>().text = "Efficiency vs. par.cmp: " + Mathf.FloorToInt( 100 * (player.lastEliminations / (float)parallelComputer.lastPredictedEliminations) ) + "%";
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyCfIndependentPrediction" ).GetComponent<Text>().text = "Eliminations predicted by ind.cmp: " + independentComputer.lastPredictedEliminations;
            canvas.transform.Find( "PlayerEfficiencyPanel" ).Find( "PlayerEfficiencyCfIndependentEfficiency" ).GetComponent<Text>().text = "Efficiency vs. ind.cmp: " + Mathf.FloorToInt( 100 * (player.lastEliminations / (float)independentComputer.lastPredictedEliminations) ) + "%";
        }

        canvas.transform.Find( "Computer1InfoPanel" ).Find( "PlayerInfoTurn" ).GetComponent<Text>().text = "Turn " + (parallelComputer.turn + 1) + " of " + beadSets.Length;
        canvas.transform.Find( "Computer1InfoPanel" ).Find( "PlayerInfoPossibilities" ).GetComponent<Text>().text = "Possible solutions left: " + parallelComputer.codeSet.numCodes;
        canvas.transform.Find( "Computer1InfoPanel" ).Find( "PlayerInfoEliminations" ).GetComponent<Text>().text = "Solutions eliminated: " + (parallelComputer.codeSet.numOriginalCodes - parallelComputer.codeSet.numCodes);
        canvas.transform.Find( "Computer1InfoPanel" ).Find( "PlayerInfoProgress" ).GetComponent<Text>().text = "Progress: " + Mathf.FloorToInt( 100 * ((parallelComputer.codeSet.numOriginalCodes - parallelComputer.codeSet.numCodes) / (float)(parallelComputer.codeSet.numOriginalCodes-1)) ) + "%";
        canvas.transform.Find( "Computer1InfoPanel" ).Find( "PlayerInfoWouldPlay" ).GetComponent<Text>().text = "Proposed guess: " + parallelComputer.guess;
        canvas.transform.Find( "Computer1InfoPanel" ).Find( "PlayerInfoPredictedEliminations" ).GetComponent<Text>().text = "Estimated eliminations: " + parallelComputer.predictedEliminations;

        canvas.transform.Find( "Computer2InfoPanel" ).Find( "PlayerInfoTurn" ).GetComponent<Text>().text = "Turn " + (independentComputer.turn + 1) + " of " + beadSets.Length;
        canvas.transform.Find( "Computer2InfoPanel" ).Find( "PlayerInfoPossibilities" ).GetComponent<Text>().text = "Possible solutions left: " + independentComputer.codeSet.numCodes;
        canvas.transform.Find( "Computer2InfoPanel" ).Find( "PlayerInfoEliminations" ).GetComponent<Text>().text = "Solutions eliminated: " + (independentComputer.codeSet.numOriginalCodes - independentComputer.codeSet.numCodes);
        canvas.transform.Find( "Computer2InfoPanel" ).Find( "PlayerInfoProgress" ).GetComponent<Text>().text = "Progress: " + Mathf.FloorToInt( 100 * ((independentComputer.codeSet.numOriginalCodes - independentComputer.codeSet.numCodes) / (float)(independentComputer.codeSet.numOriginalCodes-1)) ) + "%";
        canvas.transform.Find( "Computer2InfoPanel" ).Find( "PlayerInfoWouldPlay" ).GetComponent<Text>().text = "Proposed guess: " + independentComputer.guess;
        canvas.transform.Find( "Computer2InfoPanel" ).Find( "PlayerInfoPredictedEliminations" ).GetComponent<Text>().text = "Estimated eliminations: " + independentComputer.predictedEliminations;
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
}
