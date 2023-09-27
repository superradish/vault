using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArrayLayout  {

	[System.Serializable]
	public struct rowData{
		public bool[] row;
	}

    public Grid grid;
    public rowData[] rows = new rowData[16]; //Grid of 7x7 ? i switched to 16 from 14 to try and make it 8x8 we'll see if that works or whatever
}
