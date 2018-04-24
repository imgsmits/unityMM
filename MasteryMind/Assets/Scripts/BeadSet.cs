using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A set of beads in the UI to display or edit codes and display feedback.
 */
public class BeadSet : MonoBehaviour
{
    // All sprites for colored beads and feedback
    public Sprite[] beadSprites = null;
    public Sprite[] feedbackSprites = null;

    // The beads and feedback swatches
    BeadSetBead[] beads = null;
    BeadSetFeedback[] feedbacks = null;

    //
    float spacingBeads = 0.75f;
    float spacingButton = 0.5f;
    float spacingFeedback = 0.45f;

    // Whether colors may be edited by clicking
    bool editable = true;

    GameManager2 gameManager = null;

    /**
     * Initializes the bead set.
     */
    public void Initialize()
    {
        // Fetch the game manager
        gameManager = GameObject.Find( "GameManager" ).GetComponent<GameManager2>();

        // Fetch the three main components
        GameObject bead = transform.Find("bead1").gameObject;
        GameObject button = transform.Find( "buttonEnter" ).gameObject;
        GameObject feedback = transform.Find( "feedback1" ).gameObject;

        // Advance horizontal offset beyond first bead
        float horizontalOffset = bead.transform.position.x + spacingBeads;
        
        // Instantiates beads
        for( int i = 1; i < Code.numDigits; i++ )
        {
            GameObject beadCopy = Instantiate( bead, new Vector3(horizontalOffset,bead.transform.position.y, bead.transform.position.z), bead.transform.rotation, transform );
            beadCopy.name = "bead" + (i+1);
            beadCopy.GetComponent<BeadSetBead>().index = i;
            horizontalOffset += spacingBeads;
        }

        // Position button and feedback
        horizontalOffset += spacingButton;
        button.transform.position = new Vector3( horizontalOffset, bead.transform.position.y, bead.transform.position.z );
        feedback.transform.position = new Vector3( horizontalOffset, bead.transform.position.y, bead.transform.position.z );

        // Instantiate feedbacks
        horizontalOffset += spacingFeedback;
        for ( int i = 1; i < Code.numDigits;i++)
        {
            GameObject feedbackCopy = Instantiate( feedback, new Vector3(horizontalOffset, bead.transform.position.y, bead.transform.position.z ), feedback.transform.rotation, transform );
            horizontalOffset += spacingFeedback;
            feedbackCopy.name = "feedback" + (i+1);
        } 

        // Fetch the beads and feedback swatches
        beads = transform.GetComponentsInChildren<BeadSetBead>();
        feedbacks = transform.GetComponentsInChildren<BeadSetFeedback>();
    }

    /**
     * Handles a click on a bead.
     */
	public void OnBeadClicked(int index)
    {
        // Ignore clicks if not editable
        if( !editable || gameManager.paused )
            return;

        // Cycle through the colors
        if( beads[index].color == -1 )
            beads[index].color = 0;
        else
            beads[index].color = (beads[index].color + 1) % Code.numColors;

        // Update the sprite
        beads[index].GetComponent<SpriteRenderer>().sprite = beadSprites[beads[index].color];
    }

    /**
     * Handle a click on the enter button.
     */
    public void OnEnterClicked()
    {
        // Ignore clicks if not editable
        if( !editable || gameManager.paused )
            return;

        // Check if all colors are set
        for( int i = 0; i < beads.Length; i++ )
            if( beads[i].color == -1 )
                return;

        // Gather the code
        int[] guess = new int[beads.Length];
        for( int i = 0; i < beads.Length; i++ )
            guess[i] = beads[i].color;

        // Hide the button
        transform.Find( "buttonEnter" ).GetComponent<SpriteRenderer>().enabled = false;

        // Make the guess
        Feedback fb = GameObject.Find( "GameManager" ).GetComponent<GameManager2>().ApplyMove( new Code(guess) );

        // Clear feedback
        for( int i = 0; i < feedbacks.Length; i++ )
            feedbacks[i].GetComponent<SpriteRenderer>().enabled = false;

        // Display the feedback
        int feedbackIndex = 0;
        for( int i = 0; i < fb.numRightPlace; i++ )
        {
            feedbacks[feedbackIndex].GetComponent<SpriteRenderer>().enabled = true;
            feedbacks[feedbackIndex++].GetComponent<SpriteRenderer>().sprite = feedbackSprites[1];
        }
        for( int i = 0; i < fb.numRightColor; i++ )
        {
            feedbacks[feedbackIndex].GetComponent<SpriteRenderer>().enabled = true;
            feedbacks[feedbackIndex++].GetComponent<SpriteRenderer>().sprite = feedbackSprites[0];
        }

        // Notify game manager
        GameObject.Find( "GameManager" ).GetComponent<GameManager2>().CompleteMove();
    }

    public void SetToCode( Code code )
    {
        for( int i = 0; i < beads.Length; i++ )
        {
            beads[i].color = code.code[i];
            beads[i].GetComponent<SpriteRenderer>().sprite = beadSprites[beads[i].color];
        }
    }

    public void SetBeadToColor( int index, int color )
    {
        beads[index].color = color;
        beads[index].GetComponent<SpriteRenderer>().sprite = beadSprites[beads[index].color];
    }

    public void Hide()
    {
        // Hide the beads
        for( int i = 0; i < beads.Length; i++ )
        {
            Color c = beads[i].GetComponent<SpriteRenderer>().color;
            c.a = 0.2f;
            beads[i].GetComponent<SpriteRenderer>().color = c;
        }

        // Hide the feedback
        for( int i = 0; i < feedbacks.Length; i++ )
            feedbacks[i].GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ShowBeads()
    {
        // Show the beads
        for( int i = 0; i < beads.Length; i++ )
        {
            Color c = beads[i].GetComponent<SpriteRenderer>().color;
            c.a = 1.0f;
            beads[i].GetComponent<SpriteRenderer>().color = c;
        }
    }

    public void MakeEditable()
    {
        BeadSetButtonEnter buttonEnter = transform.GetComponentInChildren<BeadSetButtonEnter>();
        buttonEnter.GetComponent<SpriteRenderer>().enabled = true;
        editable = true;

        ShowBeads();
    }

    public void MakeUneditable()
    {
        BeadSetButtonEnter buttonEnter = transform.GetComponentInChildren<BeadSetButtonEnter>();
        buttonEnter.GetComponent<SpriteRenderer>().enabled = false;
        editable = false;
    }
}