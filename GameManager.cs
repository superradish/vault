using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class NewBehaviourScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int _width = 8;
    [SerializeField] private int _height = 8;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;
    //[SerializeField] private List<BlockType> _types;

    public ArrayLayout boardLayout;

    System.Random random;

    public Sprite[] pieces;

    private GameState _state;
    
    private Board _board;

    //private BlockType GetBlockTypeByValue(int value) => _types.First(t => t.Value == value);

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        VerifyBoard();
    }

    void Update(){

    }
    public void OnPointerDown(PointerEventData eventData){
        int i = eventData.clickCount;
        Debug.Log(i);
     //   if (updating) return;
    }
     
    public void OnPointerUp(PointerEventData eventData)
    {
      //  MovePieces.instance.DropPiece();
      Debug.Log("up");
    }
    // Update is called once per frame
    void GenerateGrid()
    {
        _board = new Board();
        var center = new Vector2((float) _width / 2 - .5f,(float) _height / 2 - .5f );
        var boardSprite = Instantiate(_boardPrefab, center, Quaternion.identity);
        boardSprite.size = new Vector2(_width, _height);
        _board._board = new Node[_width, _height];
        _board._nodes = new List<Node>();
        _board._blocks = new List<Block>();
        for (int x = 0; x < _width; x++){
            for (int y = 0; y < _height; y++) {
                Node node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                _board._nodes.Add(node);
                Block block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity);
                block.Init(fillPieceWithRandom(), Point.fromVector(node.Pos), _board.GetNodeAtPosition(node.Pos), _board);
                _board._board[x, y] = _board.GetNodeAtPosition(node.Pos);        


                //block.SetBlock(node); 
                _board._blocks.Add(block);
            }
        }




        Camera.main.transform.position = new Vector3(center.x, center.y, -5);

        //SpawnBlocks(_width * _height);
    }
    
    void VerifyBoard(){
        List<int> remove;
        for (int x = 0; x < _width; x++){
            for (int y = 0; y < _height; y++){
                Point p = new Point(x, y);
                int val = getValueAtPoint(p);
                remove = new List<int>();
                    if (isConnected(p, true).Count > 1){
                        val = getValueAtPoint(p);
                        if (!remove.Contains(val))
                            remove.Add(val);
                        setValueAtPoint(p, fillPieceWithRandom());
                    }
                    remove.Remove(val);
                    if (isConnected(p, true).Count > 1){
                        val = getValueAtPoint(p);
                        if (!remove.Contains(val))
                            remove.Add(val);
                        setValueAtPoint(p, fillPieceWithRandom());
                    }
                    remove.Remove(val);
                }
            }
        }
    

    /*void SpawnBlocks(int amount){
        var freeNodes = _nodes.Where(n=>n.OccupiedBlock == null).OrderBy(b=>Random.value).ToList();

        foreach(var node in freeNodes.Take(amount)){
            var block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity); 
            block.SetBlock(node);        
            block.Init(new BlockType(fillPieceWithRandom()), Point.fromVector(node.Pos), node);
            _blocks.Add(block);
        }
    }*/

    Node GetNodeAtPosition(Vector2 pos) {
        return _board._nodes.FirstOrDefault(n => n.Pos == pos);
    }

    Block GetBlockAtPosition(Vector2 pos){
        return _board._blocks.FirstOrDefault(b => b.Pos == pos);
    }

    private int fillPieceWithRandom() 
    {  
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        return random.Next(0, 4);
    }

    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdeghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for (int i = 0; i < 20; i++)
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        return seed;
    }

    int getValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= _width || p.y < 0 || p.y >= _height) return -1;
       // Debug.Log( _board[p.x, p.y].Value.ToString() + " value returned by getvalueatpoint");
       //if( _board[p.x, p.y].Value == 0){Debug.Log(p.x.ToString() + ", " + p.y.ToString() + "get value at point Value is 0");}
        return _board._board[p.x, p.y].Value;
    }

    void setValueAtPoint(Point p, int v)
    {
        _board._board[p.x, p.y].OccupiedBlock.Value = v;
        _board._board[p.x, p.y].OccupiedBlock.updating = true;
    }

    Node getNodeAtPoint(Point p)
    {
        return _board._board[p.x, p.y];
    }

        int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
        return val;
    }



    int newValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for (int i = 0; i < pieces.Length; i++)
            available.Add(i + 1);
        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0) return 0;
        return available[random.Next(0, available.Count)];
    }


    public void RemoveBlock(Block block) {
        _board._blocks.Remove(block);
        Destroy(block.gameObject);
    }

    List<Point> isConnected(Point p, bool main)
    {
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        //Debug.Log(getValueAtPoint(p).ToString() + " called");
        Point[] directions =
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };
        
        foreach(Point dir in directions) //Checking if there is 2 or more same shapes in the directions
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for(int i = 0; i < 3; i++)
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if(getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1) //If there are more than 1 of the same shape in the direction then we know it is a match
                AddPoints(ref connected, line); //Add these points to the overarching connected list
        }

        for(int i = 0; i < 2; i++) //Checking if we are in the middle of two of the same shapes
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i + 2]) };
            foreach (Point next in check) //Check both sides of the piece, if they are the same value, add them to the list
            {
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);
        }

        for(int i = 0; i < 4; i++) //Check for a 2x2
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if (next >= 4)
                next -= 4;

            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[next]), Point.add(p, Point.add(directions[i], directions[next])) };
            foreach (Point pnt in check) //Check all sides of the piece, if they are the same value, add them to the list
            {
                if (getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }

            if (same > 2)
                AddPoints(ref connected, square);
        }

        if(main) //Checks for other matches along the current match
        {
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, isConnected(connected[i], false));
        }


        return connected;
    }
        void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach(Point p in add)
        {
            bool doAdd = true;
            for(int i = 0; i < points.Count; i++)
            {
                if(points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }

            if (doAdd) points.Add(p);
        }
    }


}

/*private void ChangeState(GameState newState){
    _state = newState;
    case GameState.GenerateLevel:
        GenerateGrid();
        break;
    case GameState.VerifyBoard:

}*/

public enum GameState {
    GenerateLevel,
    VerifyBoard,
    WaitingInput,
    Moving,
    Win,
    Lose
}

public class Board
{
    public List<Node> _nodes;
    public List<Block> _blocks;
    public Node[,] _board;

    public void Swap(Block b, Point dir){
        var n1 = _board[b.index.x, b.index.y];
        var n2 = _board[dir.x, dir.y];

        //(n1.OccupiedBlock, n2.OccupiedBlock) = (n2.OccupiedBlock, n1.OccupiedBlock);

        //n1.OccupiedBlock.transform.parent = n1.transform;
        //n2.OccupiedBlock.transform.parent = n2.transform;

        n1.OccupiedBlock.MoveBlock(n1.OccupiedBlock.newIndex);
        n2.OccupiedBlock.MoveBlock(n1.OccupiedBlock.index);
    }

    void RemoveBlock(Block block) {
        this._blocks.Remove(block);
    }

    public Node GetNodeAtPosition(Vector2 pos) {
        Debug.Log(pos.ToString() + " bar");
        Debug.Log(this._nodes.FirstOrDefault(n => n.Pos == pos).ToString());
        return this._nodes.FirstOrDefault(n => n.Pos == pos);
    }

}

//refactor piece to block later i'm just lazy right now
  //  [System.Serializable]
public class BlockType {   
    public int Value;
    public Color Color;
    public Block block;

    public BlockType(int v)
    {
        Value = v;
    //    piece = new Block(this);
    }

    public void SetBlock(Block b)
    {
        block = b;
        Value = (block == null) ? 0 : block.Value;
        if (block == null) return;
    }

    public Block getBlock()
    {
        return block;
    }

   // public void setValue(int v){
   //     value = v;
   // }

    public int Upgrade(Block p) => Value++;
}