using UnityEngine;
using System.Collections;

public class DebuggerBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	//	if(Grid.needsToBeChecked) {
			string output = "";
			for(int y = Grid.h - 1; y > -1; y--) {
				for(int x = 0; x < Grid.w; x++)
					if(Grid.grid[x, y] != null)
						switch(Grid.grid[x, y].name.Substring(0, 1)) {
							case "R": output += "R "; break;
							case "G": output += "G "; break;
							case "T": output += "T "; break;
							case "V": output += "V "; break;
							case "Y": output += "Y "; break;
						}
					else 
						output += "X ";
				
				output += "\n";
			}

			guiText.text = output;
			Grid.needsToBeChecked = false;
	//	}
	}
	
}
