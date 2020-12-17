using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_control_script : MonoBehaviour {

	public AudioSource puzzle_click;

	public GameObject[] _puzzle; // оригинальный массив



	public Text click_count,hour_text, min_text, sec_text;

	public Text click_count_b, hour_text_b, min_text_b, sec_text_b;

	// стартовая позиция для первого элемента
	public float startPosX;
	public float startPosY;

	// отступ по Х и Y, рассчитывается в зависимости от размера объекта
	public float outX = 1.1f;
	public float outY = 1.1f;

	public float hour;
	public float min;
	public float sec;



	public static int click;
	public static GameObject[,] grid;
	public static Vector3[,] position;
	private GameObject[] puzzleRandom;
	public static bool win;
	int cur_scene;

	// Use this for initialization
	void Start () {
		puzzle_click = gameObject.GetComponent <AudioSource > ();
		puzzle_click.Stop ();
		cur_scene = Application.loadedLevel ;
		print ("Current scene number " + cur_scene);
		if(cur_scene ==1) {
		puzzleRandom = new GameObject[_puzzle.Length];

		// заполнение массива позиций клеток
		float posXreset = startPosX;
		position = new Vector3[4,4];
		for(int y = 0; y < 4; y++)
		{
			startPosY -= outY;
			for(int x = 0; x < 4; x++)
			{
				startPosX += outX;
				position[x,y] = new Vector3(startPosX, startPosY, 0);
			}
			startPosX = posXreset;
		}
			StartNewGame ();
		}
		if(cur_scene ==0) {
			Load ();
		}

	}
	// Update is called once per frame
	void Update () {
		if (cur_scene == 1) {
			click_count.text = "Ходы : " + click;

			sec += Time.deltaTime;

			if (sec >= 60) {
			
				min = +1;
				sec = 0;
			}
			if (min >= 60) {
			
				hour += 1;
				min = 0;
			}
			sec_text.text = "" + (int)sec;
			min_text.text = "" + (int)min + " :";
			hour_text.text = "" + (int)hour + " :";
		}
		if (Input.GetKey (KeyCode.A)) {
			Save ();
			print ("save");
		}
	}
	public void Puzzle_click_voice(){
		puzzle_click.Play ();
	}

	public void StartNewGame()
	{
		win = false;
		click = 0;
		RandomPuzzle();
		Debug.Log("New Game");
		sec = 0;
		min=0;
		hour =0;
		Load ();
	}
	public void Save(){
		PlayerPrefs.SetInt ("sec",(int)sec);
		PlayerPrefs.SetInt ("min",(int)min);
		PlayerPrefs.SetInt ("hour",(int)hour);
		PlayerPrefs.SetInt ("count",(int)click);
	}
	public void Load(){
		if (PlayerPrefs.GetInt ("sec") != 0) {
			sec_text_b.text = "" + PlayerPrefs.GetInt ("sec");
		} else {
			sec_text_b.text = "" + 0;
		}
		if (PlayerPrefs.GetInt ("min") != 0) {
			min_text_b.text = "" + PlayerPrefs.GetInt ("min") + " :";
		} else {
			min_text_b.text = "" + 0 + " :";
		}
		if (PlayerPrefs.GetInt ("hour") != 0) {
			hour_text_b.text = "" + PlayerPrefs.GetInt ("hour") + " :";
		} else {
			hour_text_b.text = "" + 0 + " :";
		}
		if (PlayerPrefs.GetInt ("count") != 0) {
			click_count_b.text = "Ходы : " + PlayerPrefs.GetInt ("count");
		}else{
			click_count_b.text = "Ходы : " + 0;
		}
	}

	// создание временного массива, с случайно перемешанными элементами
	void RandomPuzzle()
	{
		int[] tmp = new int[_puzzle.Length];
		for(int i = 0; i < _puzzle.Length; i++)
		{
			tmp[i] = 1;
		}
		int c = 0;
		while(c < _puzzle.Length)
		{
			int r = Random.Range(0, _puzzle.Length);
			if(tmp[r] == 1)
			{ 
				puzzleRandom[c] = Instantiate(_puzzle[r], new Vector3(0, 10, 0), Quaternion.identity) as GameObject;
				tmp[r] = 0;
				c++;
			}
		}
		CreatePuzzle();
	}
	void CreatePuzzle()
	{
		if(transform.childCount > 0)
		{
			// удаление старых объектов, если они есть
			for(int j = 0; j < transform.childCount; j++)
			{
				Destroy(transform.GetChild(j).gameObject);
			}
		}
		int i = 0;
		grid = new GameObject[4,4];
		int h = Random.Range(0,3);
		int v = Random.Range(0,3);
		GameObject clone = new GameObject();
		grid[h,v] = clone; // размещаем пустой объект в случайную клетку
		for(int y = 0; y < 4; y++)
		{
			for(int x = 0; x < 4; x++)
			{
				// создание дубликатов на основе временного массива
				if(grid[x,y] == null)
				{
					grid[x,y] = Instantiate(puzzleRandom[i], position[x,y], Quaternion.identity) as GameObject;
					grid[x,y].name = "ID-"+i;
					grid[x,y].transform.parent = transform;
					i++;
				}
			}
		}
		Destroy(clone); 
		for(int q = 0; q < _puzzle.Length; q++)
		{
			Destroy(puzzleRandom[q]);
		}
	}
	public void GameFinish()
	{
		int i = 1;
		for(int y = 0; y < 4; y++)
		{
			for(int x = 0; x < 4; x++)
			{
				if(grid[x,y]) { if(grid[x,y].GetComponent<puzzle_script >().ID == i) i++; } else i--;
			}
		}
		if(i == 15)
		{
			for(int y = 0; y < 4; y++)
			{
				for(int x = 0; x < 4; x++)
				{
					if(grid[x,y]) Destroy(grid[x,y].GetComponent<puzzle_script>());
				}
			}
			win = true;

			Debug.Log("Finish!");
			Save();
		}
	}
	

}
