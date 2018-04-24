using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A sequence of colored beads in the game.
 */
public class Code
{
    public static int numDigits = 4;
    public static int numColors = 6;

    public int[] code = null;

    public Code()
    {
        code = new int[numDigits];
        MakeZero();
    }
    
    public Code( Code original )
    {
        code = new int[numDigits];
        for( int i = 0; i < numDigits; i++ )
            code[i] = original.code[i];
    }

    public Code( int[] digits )
    {
        code = new int[numDigits];
        for( int i = 0; i < numDigits; i++ )
            code[i] = digits[i];
    }

    public void MakeRandom()
    {
        for( int i = 0; i < numDigits; i++ )
            code[i] = Random.Range( 0, numColors-1 );
    }

    public void MakeZero()
    {
        for( int i = 0; i < numDigits; i++ )
            code[i] = 0;
    }

    override public string ToString()
    {
        string s = "Code(";
        for( int i = 0; i < numDigits; i++ )
            s += code[i];
        s += ")";
        return s;
    }

    public static Feedback CalculateFeedback( Code guess, Code solution )
    {
        // Make copies we can change
        Code copyGuess = new Code( guess );
        Code copySolution = new Code( solution );

        // Calculate the number of colors in the right place
        int numRightPlace = 0;
        for( int i = 0; i < numDigits; i++ )
        {
            if( copyGuess.code[i] != -1 && copySolution.code[i] != -1 && copyGuess.code[i] == copySolution.code[i] )
            {
                numRightPlace++;
                copyGuess.code[i] = -1;
                copySolution.code[i] = -1;
            }
        }

        // Calculate the number of colors right
        int numRightColor = 0;
        for( int i = 0; i < numDigits; i++ )
        {
            for( int j = 0; j < numDigits; j++ )
            {
                if( copyGuess.code[i] != -1 && copySolution.code[j] != -1 && copyGuess.code[i] == copySolution.code[j] )
                {
                    numRightColor++;
                    copyGuess.code[i] = -1;
                    copySolution.code[j] = -1;
                }
            }
        }

        // Return feedback
        return new Feedback( numRightPlace, numRightColor );
    }
}