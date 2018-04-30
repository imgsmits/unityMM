using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedAITest : MonoBehaviour 
{
	void Start() 
	{
		Code.numDigits = 6;
		Code.numColors = 6;
		
		Code solution = new Code();
		solution.MakeRandom();
		
		Debug.Log("numDig="+Code.numDigits+", numCol="+Code.numColors+", sol="+solution);
		
		Computer computer = new Computer();
		Debug.Log("(1) Possibilities: "+computer.codeSet.numCodes);
		
		
		long t1 = System.DateTime.Now.Ticks;
		Code guess = computer.PrepareGuess();
		long t2 = System.DateTime.Now.Ticks;
		Debug.Log("Computer says "+computer.guess+" in "+((t2-t1)/10000)+"ms");
		
		t1 = System.DateTime.Now.Ticks;
		Feedback fb = Code.CalculateFeedback(guess,solution);
		t2 = System.DateTime.Now.Ticks;
		Debug.Log("Feedback: "+fb+" in "+((t2-t1)/10000)+"ms");
		
		t1 = System.DateTime.Now.Ticks;
		computer.AdvanceTurn(guess,fb);
		t2 = System.DateTime.Now.Ticks;
		Debug.Log("(2) Possibilities: "+computer.codeSet.numCodes+" in "+((t2-t1)/10000)+"ms");
		
		t1 = System.DateTime.Now.Ticks;
		guess = computer.PrepareGuess();
		t2 = System.DateTime.Now.Ticks;
		Debug.Log("Computer says "+computer.guess+" in "+((t2-t1)/10000)+"ms");
		
		t1 = System.DateTime.Now.Ticks;
		fb = Code.CalculateFeedback(guess,solution);
		t2 = System.DateTime.Now.Ticks;
		Debug.Log("Feedback: "+fb+" in "+((t2-t1)/10000)+"ms");
		
		t1 = System.DateTime.Now.Ticks;
		computer.AdvanceTurn(guess,fb);
		t2 = System.DateTime.Now.Ticks;
		Debug.Log("(3) Possibilities: "+computer.codeSet.numCodes+" in "+((t2-t1)/10000)+"ms");
		
		t1 = System.DateTime.Now.Ticks;
		guess = computer.PrepareGuess();
		t2 = System.DateTime.Now.Ticks;
		Debug.Log("Computer says "+computer.guess+" in "+((t2-t1)/10000)+"ms");
	}
}
