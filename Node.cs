using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2 Pos => transform.position;
    public Block OccupiedBlock;
    public int Value => OccupiedBlock.Value;

    public void Init(Block block){
        OccupiedBlock = block;
    }
}
