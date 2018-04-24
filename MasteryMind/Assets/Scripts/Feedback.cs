using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Feedback on a code: how many correctly placed and how many of correct colors but not rightly placed.
 */
public class Feedback
{
    public int numRightPlace = 0;
    public int numRightColor = 0;

    public Feedback( int numRightPlace = 0, int numRightColor = 0 )
    {
        this.numRightPlace = numRightPlace;
        this.numRightColor = numRightColor;
    }

    public bool IsCorrect()
    {
        return numRightPlace == Code.numDigits;
    }

    public static bool operator==( Feedback fb1, Feedback fb2 )
    {
        return fb1.numRightPlace == fb2.numRightPlace && fb1.numRightColor == fb2.numRightColor;
    }

    public static bool operator!=( Feedback fb1, Feedback fb2 )
    {
        return fb1.numRightPlace != fb2.numRightPlace || fb1.numRightColor != fb2.numRightColor;
    }

    public override bool Equals( object obj )
    {
        return this == (Feedback)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}