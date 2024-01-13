using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System.Linq;
using JetBrains.Annotations;

public class Block : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
    public Board _board;
    public BlockType _type;
    public int Value;
    public Node Node;
    public Node Target;
    public Point index;
    public Point oldIndex;
    public Point newIndex;
    //public BlockType Type;
    public bool Merging;
    public bool moving = false;
    public bool updating = false;    

    public GameObject block;

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
        Target = null;
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
        oldIndex = index;
        index = p;
        //ResetPosition();
        UpdateName();
    }

    public void Kill(){
        this._type = BlockType.blank;
        _text.text = "X";

    }

    public override string ToString(){
        return new string("value:" + Value.ToString() + ", " + index.x.ToString() + ", " + index.y.ToString());
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
        Debug.Log("dropped in movepieces " + this.Node.name);
        if (updating) return;
        MovePieces.instance.MovePiece(Node);
        //mouseStart = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 nDir = mouseStart.normalized;
        //Debug.Log(mouseStart + "foo, " + nDir.x.ToString() + ", " + nDir.y.ToString());
        //Debug.Log($"Down: {GetInstanceID()}");
    }

    public void OnMouseUp(){

          newIndex = Point.clone(this.index);
          oldIndex = Point.clone(this.index);
        _board._update.Add(this.Node);
        Point add = Point.zero;
            Vector2 dir = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseStart);
            //Debug.Log(mouseStart + "foo, " + dir.x.ToString() + ", " + dir.y.ToString());
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            //Debug.Log(dir.magnitude.ToString());
            if (dir.magnitude > .75) // magnitude controls how soon a piece begins to be dragged
            {
            if (!this.updating){
            this.updating = true;
            }
                //make add either (1, 0) | (-1, 0) | (0, 1) | (0, -1) depending on the direction of the mouse point
                if (aDir.x > aDir.y)
                    add = (new Point((nDir.x > 0) ? 1 : -1, 0));
                else if(aDir.y > aDir.x)
                    add = (new Point(0, (nDir.y > 0) ? 1 : -1));
            
            //Debug.Log(add.ToString() + " add tostring");
            newIndex.add(add);
            this.Target = _board.GetNodeAtPoint(newIndex);
            Target.OccupiedBlock.Target = this.Node;
                        Debug.Log("fired in updating");
                       _board._update.Add(this.Target);
            newIndex = null; 
           // if(this.updating == true){
 
            //MoveBlock(Target.OccupiedBlock.index);
            //Target.OccupiedBlock.MoveBlock(oldIndex);
            
            //this.UpdatePiece();

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
        this.Value = val;
        this._text.text = val.ToString();
    }

   // public void setValue(int v){
   //     value = v;
   // }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("dropped in movepieces");
        if (updating) return;
        MovePieces.instance.MovePiece(this.Node);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        MovePieces.instance.DropPiece();
    }

    public void MoveBlock(Point dir){
        //Debug.Log("fired in movepositionto");
        Vector2 v = dir.ToVector();

        transform.position = Vector2.Lerp(index.ToVector(), v, Time.deltaTime * 16f);
    }

    public int Upgrade(Block p) => Value++;
}



