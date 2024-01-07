using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour
{

    public List<Node> _nodes;
    public List<Node> _update;
    public List<FlippedPieces> _flipped;
    public int[] fills;
    public List<Block> _blocks;
    public Node[,] _board;
    public List<Block> _lastMoved;

    public void Swap(Block b, Point dir, bool move){
        
        var n1 = _board[b.index.x, b.index.y];
        var n2 = _board[dir.x, dir.y];
        //Node n1n = n1.Node;
        //Node n2n = n2.Node;

        Debug.Log( "first string movement: " + n1.OccupiedBlock.Pos.ToString() + "- n1, " + n2.OccupiedBlock.Pos.ToString() + " - n2");

        n1.OccupiedBlock = n2.OccupiedBlock;
        n2.OccupiedBlock = b;

        _lastMoved.Add(n1.OccupiedBlock);
        _lastMoved.Add(n2.OccupiedBlock);

        Debug.Log( "second string movement: " + n1.OccupiedBlock.Pos.ToString() + "- n1, " + n2.OccupiedBlock.Pos.ToString() + " - n2");
        
        n1.OccupiedBlock.transform.parent = n1.transform;
        n2.OccupiedBlock.transform.parent = n2.transform;

        n1.OccupiedBlock.transform.DOLocalMove(Vector3.zero, 0.5f);
        n2.OccupiedBlock.transform.DOLocalMove(Vector3.zero, 0.5f);

        n1.OccupiedBlock.SetIndex(n1.OccupiedBlock.index);        
        n2.OccupiedBlock.SetIndex(n2.OccupiedBlock.index);

        //n1.OccupiedBlock.MoveBlock(n1.OccupiedBlock.newIndex);
        //n2.OccupiedBlock.MoveBlock(n1.OccupiedBlock.index);
    }

    public void Kill(Node victim){
        victim.OccupiedBlock.Kill();
    }

public void Swap(Block b, Block b2, bool move){ //yeah there's two swaps which is a bit redundant, bite me
        
        var n1 = _board[b.index.x, b.index.y];
        var n2 = _board[b2.index.x, b2.index.y];
        //Node n1n = n1.Node;
        //Node n2n = n2.Node;

        Debug.Log( "first string movement: " + n1.OccupiedBlock.Pos.ToString() + "- n1, " + n2.OccupiedBlock.Pos.ToString() + " - n2");

        n1.OccupiedBlock = n2.OccupiedBlock;
        n2.OccupiedBlock = b;

        _lastMoved.Add(n1.OccupiedBlock);
        _lastMoved.Add(n2.OccupiedBlock);

        Debug.Log( "second string movement: " + n1.OccupiedBlock.Pos.ToString() + "- n1, " + n2.OccupiedBlock.Pos.ToString() + " - n2");
        
        n1.OccupiedBlock.transform.parent = n1.transform;

        n2.OccupiedBlock.transform.parent = n2.transform;

        n1.OccupiedBlock.transform.DOLocalMove(Vector3.zero, 0.5f);
        n2.OccupiedBlock.transform.DOLocalMove(Vector3.zero, 0.5f);

        n1.OccupiedBlock.SetIndex(n1.OccupiedBlock.index);        
        n2.OccupiedBlock.SetIndex(n2.OccupiedBlock.index);

        //n1.OccupiedBlock.MoveBlock(n1.OccupiedBlock.newIndex);
        //n2.OccupiedBlock.MoveBlock(n1.OccupiedBlock.index);
    }

    void RemoveBlock(Block block) {
        this._blocks.Remove(block);
    }

    public Node GetNodeAtPosition(Vector2 pos) {
       // Debug.Log(pos.ToString() + " bar");
       // Debug.Log(this._nodes.FirstOrDefault(n => n.Pos == pos).ToString());
        return this._nodes.FirstOrDefault(n => n.Pos == pos);
    }


    public Node GetNodeAtPoint(Point point) {
        Vector2 pos = point.ToVector();
       // Debug.Log(pos.ToString() + " bar");
       // Debug.Log(this._nodes.FirstOrDefault(n => n.Pos == pos).ToString());
        return this._nodes.FirstOrDefault(n => n.Pos == pos);
    }


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
