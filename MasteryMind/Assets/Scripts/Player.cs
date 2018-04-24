using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public CodeSet codeSet = null;

    public int turn = 0;
    public int lastEliminations = 0;

    public int score = 0;

	public Player()
    {
        score = PlayerPrefs.GetInt( "playerScore" );

        codeSet = new CodeSet();
        codeSet.AddAllCodes();   
	}

    public void  AdvanceTurn( Code guess, Feedback feedback )
    {
        lastEliminations = codeSet.Eliminate( guess, feedback );
        turn++;
    }
    
}
