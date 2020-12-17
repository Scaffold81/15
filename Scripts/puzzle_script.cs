using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class puzzle_script : MonoBehaviour {
	public int ID; 
	public Game_control_script game_control;
	// Use this for initialization
	void Start () {
		game_control = GameObject.FindObjectOfType <Game_control_script > ();
		//gameObject.name = "" + ID;
		//gameObject.GetComponentInChildren <Text> ().text = "" + ID;

	}
	
	// текущая и пустая клетка, меняются местами
	void ReplaceBlocks(int x, int y, int XX, int YY)
	{
		Game_control_script.grid[x,y].transform.position = Game_control_script.position[XX,YY];
		Game_control_script.grid[XX,YY] = Game_control_script.grid[x,y];
		Game_control_script.grid[x,y] = null;
		Game_control_script.click++;
		//Game_control_script.GameFinish();
	}

	void OnMouseDown()
	{
		game_control.Puzzle_click_voice ();
		for(int y = 0; y < 4; y++)
		{
			for(int x = 0; x < 4; x++)
			{
				if(Game_control_script.grid[x,y])
				{
					if(Game_control_script.grid[x,y].GetComponent<puzzle_script >().ID == ID)
					{
						if(x > 0 && Game_control_script.grid[x-1,y] == null)
						{
							ReplaceBlocks(x,y,x-1,y);
							return;
						}
						else if(x < 3 && Game_control_script.grid[x+1,y] == null)
						{
							ReplaceBlocks(x,y,x+1,y);
							return;
						}
					}
				}
				if(Game_control_script.grid[x,y])
				{
					if(Game_control_script.grid[x,y].GetComponent <puzzle_script >().ID == ID)
					{
						if(y > 0 && Game_control_script.grid[x,y-1] == null)
						{
							ReplaceBlocks(x,y,x,y-1);
							return;
						}
						else if(y < 3 && Game_control_script.grid[x,y+1] == null)
						{
							ReplaceBlocks(x,y,x,y+1);
							return;
						}
					}
				}
			}
		}
	}
}
