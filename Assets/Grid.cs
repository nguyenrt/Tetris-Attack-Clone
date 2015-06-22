using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public static int w = 6;
	public static int h = 11;
	public static Transform[,] grid = new Transform[w, h];
	public static bool needsToBeChecked = false;	//For debugger
	public static bool changed = false;
	public static ArrayList occupiedPos = new ArrayList();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		DropBlocks();
		
		if(Input.GetKeyDown(KeyCode.Space)) {
			SwapBlocks(CursorControls.leftX, CursorControls.rightX, CursorControls.cursorY);
		}
	}
	
	public static void PushUpRow() {
		for(int x = 0; x < w; x++)
			for(int y = h - 1; y > 0 ; y--) {
				grid[x, y] = grid[x, y - 1];

				if(grid[x, y] != null)
					grid[x, y].position += new Vector3(0, 1, 0);
		}
	}

	public static void DestroyBlock(int x, int y) {
		Vector2 v = new Vector2(x, y);
		
		if(!occupiedPos.Contains(v))
			occupiedPos.Add(v);

		iTween.ColorTo(grid[x,y].gameObject, iTween.Hash("time", 2.0f,
		                                                 "color", Color.clear,
		                                                 "onCompleteTarget", GameObject.Find("Grid"),
		                                                 "onCompleteParams", v,	
		                                                 "onComplete", "NullBlocks"));
	}
	
	private void NullBlocks(Vector2 v) {
		int x = (int)v.x, 
			y = (int)v.y;
			
		if(grid[x, y] != null) {
			occupiedPos.Remove(v);
			Destroy(grid[x, y].gameObject);
			grid[x, y] = null;
			changed = true;
		}

	}
	
	private void DropBlocks() {
		ArrayList yPos = new ArrayList();

		for(int x = 0; x < w; x++) {
			for(int y = 0; y < h; y++)
				if(grid[x, y] != null)
					yPos.Add(y);

			foreach(int y in yPos) {
				for(int i = y; i - 1 >= 0 && grid[x, i - 1] == null; i--) {
					grid[x, i].position += new Vector3(0, -1, 0);
					grid[x, i - 1] = grid[x, i];
					grid[x, i] = null;
				}			
			}

			yPos.Clear();
		}
	}

	private void SwapBlocks(int leftX, int rightX, int cursorY) {
		Transform leftBlock = grid [leftX, cursorY],
				  rightBlock = grid [rightX, cursorY];

		if(leftBlock == null && rightBlock == null)
			return;
			
		foreach(Vector2 v in occupiedPos) {
			int x = (int)v.x,
				y = (int)v.y;
				
				if((leftX == x || rightX == x) && cursorY == y)
					return;
		}

		Vector3 leftPos = new Vector3 ((float)leftX - 2.5f, (float)cursorY, 0),
				rightPos = new Vector3 ((float)rightX - 2.5f, (float)cursorY, 0);

		if(leftBlock == null) {
			grid [leftX, cursorY] = rightBlock;
			rightBlock.position = leftPos;
			grid [rightX, cursorY] = null;
		}
		else if(rightBlock == null) {
			grid [rightX, cursorY] = leftBlock;
			leftBlock.position = rightPos;
			grid [leftX, cursorY] = null;
		}
		else {
			grid [leftX, cursorY].position = rightPos;
			grid [rightX, cursorY].position = leftPos;
			grid [leftX, cursorY] = rightBlock;
			grid [rightX, cursorY] = leftBlock;
		}

		changed = true;
		needsToBeChecked = true;
	}
}
