using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject[] blocks;
	private const float startPos = -2.5f;
	private int? prev = null;
	private int? curr = null;
	private int prevCount = 0;

	// Use this for initialization
	void Start () {
		for(int y = 0; y < 6; y++) {
			for(int x = 0; x < Grid.w; x++) {
				curr = GenerateBlock(x, y);

				if(curr == prev && prev != null)
					prevCount++;
				else
					prevCount = 0;

				prev = curr;
				transform.position += new Vector3(1, 0, 0);
			}

			// Reset row back to 0
			transform.position = new Vector3(startPos, transform.position.y + 1, transform.position.z);
			prev = null;
		}
		
		MatchColumn();
		MatchRow();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Z)) {
			Grid.PushUpRow();
			GenerateRow();
		}
		
		if(Grid.changed) {
			MatchColumn();
			MatchRow();
			Grid.changed = false;
		}

	}

	private void MatchRow() {
		Stack matchingBlocks = new Stack();
		
		for(int y = 0; y < Grid.h; y++) {
			for(int x = 0; x < Grid.w; x++) {
		 		if(matchingBlocks.Count > 0) {
					if(Grid.grid[x, y] != null) {
		 				string prevBlock = Grid.grid[(int)matchingBlocks.Peek(), y].name.Substring(0, 1),
		 					   currBlock = Grid.grid[x, y].name.Substring(0, 1);
		 					   
		 				if(!prevBlock.Equals(currBlock)) {
		 					if(matchingBlocks.Count < 3) {
								matchingBlocks.Clear();	
								matchingBlocks.Push(x);
							}
						}
						else
							matchingBlocks.Push(x);						
		 			}
		 			else if(matchingBlocks.Count < 3)
		 				matchingBlocks.Clear();		
		 		}
		 		else
					matchingBlocks.Push(x);
		 	}
		 	
			if(matchingBlocks.Count > 2)
				for(int i = matchingBlocks.Count; i > 0; i--)
					Grid.DestroyBlock((int)matchingBlocks.Pop(), y);
			else
				matchingBlocks.Clear();
		}
	
	/*
		ArrayList matchingCol = new ArrayList();
		Transform prevBlock = null;
		int prevCount = 1;
			
		for(int y = 0; y < Grid.h; y++) {
			for(int x = 0; x < Grid.w; x++) {
				if(Grid.grid[x, y] != null) {
					Transform currBlock = Grid.grid[x, y];
						
					if(prevBlock != null)
						if (!currBlock.name.Substring(0, 1).Equals(prevBlock.name.Substring(0, 1))) {
							if (prevCount > 2)
								for (int i = 0; i < prevCount; i++)
									matchingCol.Add(x - i - 1);	
							
							prevCount = 1;
						}
						else
							prevCount++;
						
					prevBlock = currBlock;
				}
				else {
					if (prevCount > 2)
						for (int i = 0; i < prevCount; i++)
							matchingCol.Add(x - i - 1);	

					prevBlock = null;
					prevCount = 1;
				}
			}

			if (prevCount > 2)
				for (int i = 0; i < prevCount; i++)
					matchingCol.Add(Grid.w - i - 1);	

			foreach(int x in matchingCol)
				Grid.DestroyBlock(x, y);
				
			matchingCol = new ArrayList();
			prevBlock = null;
			prevCount = 1;
		}*/
	}

	private void MatchColumn() {
        ArrayList matchingCol = new ArrayList();
        GameObject prevBlock = null;
        int prevCount = 1;

		for(int x = 0; x < Grid.w; x++) {
			for(int y = 0; y < Grid.h; y++) {
				if(Grid.grid[x, y] == null) {
					if(prevCount > 2)
						for (int i = 0; i < prevCount; i++)
							matchingCol.Add(y - i - 1);	
					break;

				}

				GameObject currBlock = Grid.grid[x, y].gameObject;

				if(prevBlock != null)
	            	if (!currBlock.name.Substring(0, 1).Equals(prevBlock.name.Substring(0,1))) {
						if (prevCount > 2)
							for (int i = 0; i < prevCount; i++)
								matchingCol.Add(y - i - 1);	

						prevCount = 1;
					}
	                else
	                	prevCount++;



                    prevBlock = currBlock;
                }

            foreach(int y in matchingCol)
				Grid.DestroyBlock(x, y);
			
			matchingCol = new ArrayList();
            prevBlock = null;
            prevCount = 1;
		}


	}

	public void GenerateRow() {
		transform.position = new Vector3(startPos, 0, 0);
		
		for(int x = 0; x < Grid.w; x++) {
			GenerateBlock(x, 0);
			transform.position += new Vector3(1.0f, 0, 0);
		}
	}

	public int GenerateBlock(int x, int y) {
		int i = Random.Range (0, 5);

		GameObject block = (GameObject)Instantiate (blocks [i], transform.position, Quaternion.identity);
		Grid.grid [x, y] = block.transform;

		return i;
	}
}
