using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Board : MonoBehaviour
{

    public List<Node> _nodes;
    public List<Node> _update;
    public List<FlippedPieces> _flipped;
    public List<Block> _blocks;
    public Node[,] _board;
<<<<<<< HEAD
    public List<Node> _lastMoved;
    
=======
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
>>>>>>> 9ff8dfcd931bd71debb7d94b17d0dfdd98d2821c

    public void Kill(Node victim){
        victim.OccupiedBlock.Kill();
    }

public void FlipPieces(Block b, Point p, bool move){ 
        
        var n1 = _board[b.index.x, b.index.y];
        var n2 = _board[p.x, p.y];
        //Node n1n = n1.Node;
        //Node n2n = n2.Node;

        Debug.Log( "first string movement: " + n1.OccupiedBlock.Pos.ToString() + "- n1, " + n2.OccupiedBlock.Pos.ToString() + " - n2");

        n1.OccupiedBlock = n2.OccupiedBlock;
        n2.OccupiedBlock = b;

        _lastMoved.Add(n1);
        _lastMoved.Add(n2);

        Debug.Log( "second string movement: " + n1.OccupiedBlock.Pos.ToString() + "- n1, " + n2.OccupiedBlock.Pos.ToString() + " - n2");
        
        n1.OccupiedBlock.transform.parent = n1.transform;

        n2.OccupiedBlock.transform.parent = n2.transform;

        n1.OccupiedBlock.transform.DOLocalMove(Vector3.zero, 0.5f);
        n2.OccupiedBlock.transform.DOLocalMove(Vector3.zero, 0.5f);

        n1.OccupiedBlock.SetIndex(n1.OccupiedBlock.index);  
        n1.OccupiedBlock.newIndex = null;      
        n2.OccupiedBlock.SetIndex(n2.OccupiedBlock.index);
        n2.OccupiedBlock.newIndex = null;

       // _update = FindConnected(n1);

/*
        n1.OccupiedBlock.SetIndex(n1.OccupiedBlock.index);        
        n2.OccupiedBlock.SetIndex(n2.OccupiedBlock.index);
        n1.OccupiedBlock.updating = false;
        n2.OccupiedBlock.updating = false;
        n1.OccupiedBlock.UpdatePiece();
        n2.OccupiedBlock.UpdatePiece();
        n1.OccupiedBlock.Target = null;
        n2.OccupiedBlock.Target = null;*/

        //n1.OccupiedBlock.MoveBlock(n1.OccupiedBlock.newIndex);
        //n2.OccupiedBlock.MoveBlock(n1.OccupiedBlock.index);
    }

    public List<Node> FindConnected(Node node){
        Debug.Log("Ping");
        List<Node> connected = new List<Node>();
        int val = node.OccupiedBlock.Value;
        Point[] directions;  directions = new Point[]{
            Point.down,
            Point.left,
            Point.up,
            Point.right
            };
        var rows = _board.Length;
        var cols = _board.GetLength(1); // we're assuming it's rectangular

    List<Node> neighbors = new(); // we will return this once it's filled
    
    HashSet<Point> processed = new(); // helps avoid re-visiting the same node
    Queue<Point> toSearch = new();
    toSearch.Enqueue(node.OccupiedBlock.index);

    while (toSearch.Count > 0)
    {
        Point current = toSearch.Dequeue();
        processed.Add(current);

        Node n = GetNodeAtPoint(current); // make sure your x and y are correct here
        neighbors.Add(node);

        foreach (var dir in directions)
        {
            Point pos = Point.add(current, dir);
            if (pos.x < 0 || pos.y < 0 || pos.x >= rows || pos.y >= cols) 
                continue; // avoid out of bounds

            if (processed.Contains(pos))
                continue; // avoid endless repeats
            
            // TODO: here is where you check to see if this neighbor is worth exploring.
            // for example, if its type is the same as our current "node", like:
            var neighborNode = GetNodeAtPoint(pos);
            if (neighborNode.OccupiedBlock.Value != node.OccupiedBlock.Value) // change this, ofc
                continue;

            toSearch.Enqueue(pos); // found a neighboring node worth exploring
        }
    }
    // optional to remove the starting node. you might want to keep it tho
    //var start = grid[startPos.x][startPos.y];
    //neighbors.Remove(start);
    return connected;

    }

    void RemoveBlock(Block block) {
        this._blocks.Remove(block);
    }

    public Node GetNodeAtPosition(Vector2 pos) {
       // Debug.Log(pos.ToString() + " bar");
       // Debug.Log(this._nodes.FirstOrDefault(n => n.Pos == pos).ToString());
        return this._nodes.FirstOrDefault(n => n.Pos == pos);
    }


    public Node GetNodeAtPoint(Point p) {
        Vector2 pos = p.ToVector();
        if(p.x < 0){p.x = 0;};
        if(p.y < 0){p.y = 0;};
        if(p.x > 7){p.x = 7;};
        if(p.y > 7){p.y = 7;};
        
       // Debug.Log(pos.ToString() + " bar");
       // Debug.Log(this._nodes.FirstOrDefault(n => n.Pos == pos).ToString());
        return this._nodes.FirstOrDefault(n => n.Pos == pos);
    }


    public Vector2 GetPositionFromPoint(Point p)
    {
        return new Vector2(32 + (64 * p.x), -32 - (64 * p.y));
    }

    public void ResetPiece(Node piece)
    {
        piece.OccupiedBlock.ResetPosition();
        _update.Add(piece);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
<<<<<<< HEAD
    }

    internal List<Node> GetLastMoved()
    {
        return this._lastMoved;
=======
>>>>>>> 9ff8dfcd931bd71debb7d94b17d0dfdd98d2821c
    }
}


