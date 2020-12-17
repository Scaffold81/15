using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_script : MonoBehaviour {
	public Game_control_script game_control; 
	// Use this for initialization
	void Start () {

		
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void Quit(){
		Application.Quit ();
	}
	public void Restart (){
		if (game_control != null) {
			game_control.StartNewGame ();
		}
	}
	public void Main_menu(){
	Application.LoadLevel(0);
	}
	public void New_game(){
		Application.LoadLevel(1);
	}
}
