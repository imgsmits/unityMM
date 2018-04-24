using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A set of codes.
 */
public class CodeSet
{
    // The list of codes in the set
    public Code[] codes = null;

    // The number of current codes in the set
    public int numCodes = 0;

    // The number of codes originally in the set
    public int numOriginalCodes = 0;
    
    /**
     * Generates all possible codes and adds them.
     */
    public void AddAllCodes()
    {
        // Calculate how many codes there are and prepare room
        numCodes = Mathf.FloorToInt( Mathf.Pow( Code.numColors, Code.numDigits ) );
        codes = new Code[numCodes];
        numOriginalCodes = numCodes;

        // Generate the code of all zeroes
        int[] code = new int[Code.numDigits];
        for( int i = 0; i < Code.numDigits; i++ )
            code[i] = 0;

        // Keep adding and incrementing codes until the last digit becomes overflowed
        int index = 0;
        while( code[Code.numDigits - 1] != Code.numColors )
        {
            codes[index++] = new Code( code );

            for( int i = 0; i < Code.numDigits; i++ )
            {
                code[i] = code[i] + 1;
                if( i!=Code.numDigits-1 && code[i] == Code.numColors )
                    code[i] = 0;
                else
                    break;
            }
        }
    }

    /**
     * Eliminates codes from the set that would not give the provided feedback on the provided
     * guess if that code were the solution. Use the flag to only count the number of codes
     * that would be eliminated.
     */
    public int Eliminate( Code guess, Feedback feedback, bool countOnly = false )
    {
        // Prepare room for new codes
        // TODO: this is too much, as stuff may/will be eliminated; on the other hand, the counter only is
        // one move behind so it is not that bad
        Code[] newCodes = new Code[numCodes];
        int newNumCodes = 0;

        // Loop through all codes and calculate feedback; only add to the new set those codes
        // that would give that feedback
        for ( int i = 0; i < numCodes;  i++ )
        {
            if( Code.CalculateFeedback( guess, codes[i] ) == feedback )
                newCodes[newNumCodes++] = codes[i];
        }

        // Calculate how many eliminated
        int eliminated = numCodes - newNumCodes;

        // If not only counting but really eliminating update the
        // code set
        if( !countOnly )
        {
            codes = newCodes;
            numCodes = newNumCodes;
        }

        // Return the eliminated codes count
        return eliminated;
    }
}