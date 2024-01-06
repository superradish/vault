using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class Block : MonoBehaviour, IEventSystemHandler
{
    public Board _board;
    public BlockType _type;
    public int Value;
    public Node Node;
    public Point index;
    public Point newIndex;
    //public BlockType Type;
    public bool Merging;
    public bool moving = false;
    public bool updating = false;    


    Image img;

    [HideInInspector]
    public Vector2 Pos, _originalPosition, mouseStart;
    [HideInInspector]
    public BoxCollider2D rect;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private TextMeshPro _text;

   /* public void Init(BlockType type)
    {
        Type = type;
        Value = type.Value;
        _renderer.color = type.Color;
        _text.text = type.Value.ToString();
    }*/

    void Awake(){
        //_originalPosition = transform.position;
    }

    public void Init(int val, Point p, Node node, Board board) {
        _board = board;
        Node = node;
        img = GetComponent<Image>();
        rect = GetComponent<BoxCollider2D>();
        node.OccupiedBlock = this;
        Pos = p.ToVector();
        //Type = type;
        Value = val;
        this.SetBlockType(val);
        //_renderer.color = type.Color;
        _text.text = Value.ToString();

        SetIndex(p);
        //img.sprite = piece;
    }

    void Update(){

        //if (!startBlock) return; 
        newIndex = Point.clone(this.index);
        Point add = Point.zero;
            Vector2 dir = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseStart);
            //Debug.Log(mouseStart + "foo, " + dir.x.ToString() + ", " + dir.y.ToString());
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            //Debug.Log(dir.magnitude.ToString());
            if (dir.magnitude > .75) // magnitude controls how soon a piece begins to be dragged
            {
                //make add either (1, 0) | (-1, 0) | (0, 1) | (0, -1) depending on the direction of the mouse point
                if (aDir.x > aDir.y)
                    add = (new Point((nDir.x > 0) ? 1 : -1, 0));
                else if(aDir.y > aDir.x)
                    add = (new Point(0, (nDir.y > 0) ? 1 : -1));
            }
            //Debug.Log(add.ToString() + " add tostring");
            newIndex.add(add);
            //Debug.Log(newIndex.ToString() + " newindex tostring");
    
            //Debug.Log(index.ToString() + ", " + newIndex.ToString());

            //targetBlock = GetBlockByPoint(newIndex);
            //Debug.Log("targetblock: " + targetBlock.index.ToString());
       
        
    }


    public Vector2 getPositionFromPoint(Point p)
    {
        return new Vector2(32 + (64 * p.x), -32 - (64 * p.y));
    }

    public void SetBlock(Node node) {
    if (Node != null) Node.OccupiedBlock = null;
    Node = node;
    node.Init(this);
    }

    public void AddBoard(Board board){
        _board = board;
    }

    public Block GetBlockAtPosition(Vector2 v){
        return _board.GetNodeAtPosition(v).OccupiedBlock;
    }

    public Block GetBlockByPoint(Point p)
    {
        return _board.GetNodeAtPosition(p.ToVector()).OccupiedBlock;

    }

    public void SetIndex(Point p)
    {
        index = p;
        //ResetPosition();
        UpdateName();
    }

    public void Kill(){
        this._type = BlockType.blank;
        _text.text = "X";

    }

    public string ToString(){
        return Value.ToString();
    }

    public void ResetPosition()
    {
        Pos = new Vector2(32 + (64 * index.x), -32 - (64 * index.y));
    }

    public void MovePosition(Vector2 move)
    {
        //rect.anchoredPosition += move * Time.deltaTime * 16f;
    }

    public void MovePositionTo(Vector2 move)
    {
    Debug.Log("fired in movepositionto");
       transform.position = Vector2.Lerp(index.ToVector(), move, Time.deltaTime * 16f);
    }

    void UpdateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public void OnMouseDown()
    {
        mouseStart = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 nDir = mouseStart.normalized;
        //Debug.Log(mouseStart + "foo, " + nDir.x.ToString() + ", " + nDir.y.ToString());
        //Debug.Log($"Down: {GetInstanceID()}");
    }

    public void OnMouseUp(){
        if (newIndex != this.index){
            updating = true;
            index = Point.clone(newIndex);
            newIndex = null;
            }

    }

    public bool UpdatePiece(){
            if(newIndex != null)
        {
            updating = true;
            return true;
        }
        else
        {   
            updating = false;
            return false;
        }
    }
    //refactor piece to block later i'm just lazy right now
  //  [System.Serializable]
    public enum BlockType {   
        blank = 0,
        bronze = 1,
        silver = 2,
        gold = 3,
        bag = 4
        
    }

    public void SetBlockType(int val)
    {
        this._type = (BlockType)val;
    }

   // public void setValue(int v){
   //     value = v;
   // }

    public int Upgrade(Block p) => Value++;
}

    /* keeping this in case i need it later for falling blocks but swaps happen in the Board class
    public void MoveBlock(Point dir){
        //Debug.Log("fired in movepositionto");
        Vector2 v = dir.ToVector();

        transform.position = Vector2.Lerp(index.ToVector(), v, Time.deltaTime * 16f);
    }*/



