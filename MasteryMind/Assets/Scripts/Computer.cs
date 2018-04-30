using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer
{
    public CodeSet codeSet = null;
    int[] codeScore = null;

    public int turn = 0;
    public Code guess = null;
    public int predictedEliminations = 0;
    public int lastPredictedEliminations = 0;
    public int lastEliminations = 0;
	
	Feedback[] possibleFeedback = null;

    public Computer()
    {
		// Add all codes to the set
        codeSet = new CodeSet();
        codeSet.AddAllCodes(); 

		// Prepare room for code scoring
		codeScore = new int[codeSet.numCodes];		

        // Determine number of possible feedback combinations
        int numPossibleFeedback = 0;
        for( int i = 0; i < Code.numDigits; i++ )
            for( int j = 0; j < Code.numDigits; j++ )
                if( i + j <= Code.numDigits )
                    numPossibleFeedback++;

        // Generate possible feedback combinations
        possibleFeedback = new Feedback[numPossibleFeedback];
        numPossibleFeedback = 0;
        for( int i = 0; i < Code.numDigits; i++ )
            for( int j = 0; j < Code.numDigits; j++ )
                if( i + j <= Code.numDigits )
                    possibleFeedback[numPossibleFeedback++] = new Feedback( i, j );		
	}

    public Code PrepareGuess()
    {
        // Generate guess for first turn
        if ( turn == 0 )
        {
            // Pick two different colors
            int color1 = Random.Range( 0, Code.numColors );
            int color2 = Random.Range( 0, Code.numColors );
            while( color1 == color2 )
                color2 = Random.Range( 0, Code.numColors );

            // Generate a code with two colors
            int[] code = new int[Code.numDigits];
            for( int i = 0; i < Code.numDigits / 2; i++ )
                code[i] = color1;
            for( int i = Code.numDigits / 2; i < Code.numDigits; i++ )
                code[i] = color2;
            guess = new Code( code );

            // Find the feedback that gives the lowest eliminations
			// NOTE: disable for speed
            /*int minScore = int.MaxValue;
            for( int j = 0; j < possibleFeedback.Length; j++ )
            {
                int localScore = codeSet.Eliminate( guess, possibleFeedback[j], true );
                if( localScore < minScore )
                    minScore = localScore;
            }

            // Take that value as the predicted number of eliminations
            lastPredictedEliminations = predictedEliminations;
            predictedEliminations = minScore;
			*/
        }
		
		// Generate guess for second turn (and 3rd turn for 6/6 combos)
		// This does not generate the *best* hint but the *first* hint that eliminates over 80% of the codes
		else if ( turn == 1 || (Code.numDigits==6&&Code.numColors==6&&turn==2) )
		{
			// For each code left calculate the score it would get for any feedback, taking the least
            // number of predicted eliminations, and thereof find the highest candidate
            int maxScoreIndex = -1;
            for( int i = 0; i < codeSet.numCodes; i++ )
            {
                // Prepare code score for min search
                codeScore[i] = int.MaxValue;

                // For each possible feedback F find the lowest predicted eliminations
                for( int j = 0; j < possibleFeedback.Length; j++ )
                {
                    int localScore = codeSet.PredictNumEliminated( codeSet.codes[i], possibleFeedback[j] );
                    if( localScore < codeScore[i] )
                        codeScore[i] = localScore;
                }

                // Update max score by taking the code that scorest highest even with its
                // minimal estimated eliminations
                if( maxScoreIndex < 0 || codeScore[maxScoreIndex] < codeScore[i] )
                    maxScoreIndex = i;
				
				// Check: if it elimnates over a certain percentage then we will take this
				float percentageLeft = (codeSet.numCodes-codeScore[maxScoreIndex])/(float)codeSet.numCodes;
				//Debug.Log("\t (num - scr) / (num) = ("+codeSet.numCodes+" - "+codeScore[maxScoreIndex]+") / "+ codeSet.numCodes+" = "+
				if ( percentageLeft < 0.20f )
					break;
            }

            // Take that code as the guess and set eliminations
            guess = new Code( codeSet.codes[maxScoreIndex] );
		}
        else
        {
            // For each code left calculate the score it would get for any feedback, taking the least
            // number of predicted eliminations, and thereof find the highest candidate

            // TODO: in reality, a code that could no longer be a solution, could be a good guess
            // thus, in fact, one should always take the whole set but hey

            int maxScoreIndex = -1;
            for( int i = 0; i < codeSet.numCodes; i++ )
            {
                // Prepare code score for min search
                codeScore[i] = int.MaxValue;

                // For each possible feedback F find the lowest predicted eliminations
                for( int j = 0; j < possibleFeedback.Length; j++ )
                {
                    int localScore = codeSet.PredictNumEliminated( codeSet.codes[i], possibleFeedback[j] );
                    if( localScore < codeScore[i] )
                        codeScore[i] = localScore;
                }

                // Update max score by taking the code that scorest highest even with its
                // minimal estimated eliminations
                if( maxScoreIndex < 0 || codeScore[maxScoreIndex] < codeScore[i] )
                    maxScoreIndex = i;
            }

            // Take that code as the guess and set eliminations
            guess = new Code( codeSet.codes[maxScoreIndex] );
			// NOTE: disable for speed
            //lastPredictedEliminations = predictedEliminations;
            //predictedEliminations = codeScore[maxScoreIndex];
        }

        return guess;
    }

    public void  AdvanceTurn( Code guess, Feedback feedback )
    {
        lastEliminations = codeSet.Eliminate( guess, feedback );
        turn++;
    }
    
}
