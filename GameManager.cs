using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;


public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private int _width = 8;
    [SerializeField] private int _height = 8;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private SpriteRenderer _spritePrefab;
    //[SerializeField] private List<BlockType> _types;
    [SerializeField] private Board _boardPrefab;
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
        int count = 0;
        bool wasFlipped = false;  

        // _board._update = IsConnected(getNodeAtPoint(Point.zero), true);

          /* if(_board._update.Count > 0){
            wasFlipped = true;
            _board.Swap(_board._update[0].OccupiedBlock, _board._update[1].OccupiedBlock, true); //****old test code;}
            //load the updated pieces into Moved for later
            for(int i = 0; i < _board._update.Count; i++){
                _board._lastMoved.Add(_board._update[i]);
                 Debug.Log("I updated last at " + _board._update[i].OccupiedBlock.ToString());
                _board._update[i].OccupiedBlock.updating = false; // done updating, we use lastmoved to reference the previously moved blocks and oldIndex to refer to their locations
            }
          //  _board._update = _board.FindConnected(_board._lastMoved[0]);
            
            _board._update = new List<Node>(); //clean the update list, we moved - now we check for connections
            }

  

           /* foreach(Node n in _board._lastMoved){
                Debug.Log("I moved last at " + n.OccupiedBlock.ToString());
            }
            _board._lastMoved = new List<Node>();*/

            //Debug.Log("this should fire twice for each move");
            //check for connections for each block and add to a list of connected if they are
            //Point p = b.index;

            
            List<Node> finishedUpdating = _board.GetLastMoved();
            
        for (int i = 0; i < finishedUpdating.Count; i++)
                {
            
            Node node = finishedUpdating[i];
            FlippedPieces flip = getFlipped(node);
            Node flippedPiece = null;

            List<Node> connected = IsConnected(node, true);
            
            Node other = node.OccupiedBlock.Target;
            Debug.Log("fired in finishedupdating");
            if (wasFlipped) //If we flipped to make this update
            {
                Debug.Log("fired in wasflipped");
                flippedPiece = flip.getOtherPiece(node);
                AddPoints(ref connected, IsConnected(other, true));
          
            }

              if (connected.Count == 0) //If we didn't make a match
            {
                Debug.Log("fired in wasflipped but flip back");
                if (wasFlipped) //If we flipped
                    _board.FlipPieces(node.OccupiedBlock, other.OccupiedBlock.index, false); //Flip back
            }
            else //If we made a match
            {               
                foreach (Node pnt in connected) //Remove the node pieces connected         
                {   //inside here is where we put the upgrade code

                    Node n = pnt;
                    Block block = node.OccupiedBlock;
                    if (block != null)
                    {
                        if(block.updating){
                    //Debug.Log("Kill " + pnt.ToString());
                    //_board.Kill(pnt);
                    
                        //int newValue = node.Upgrade(nodePiece);   //this fires for each piece in the connected array, we only want to move the one that was moved, if there was one moved
                        //Node newNode = new Node(newValue, pnt);  //can't set up new nodes or else we'll have nodes on top of nodes and stuff stops working

                        }
                        else{block.gameObject.SetActive(false);
                        //    _board._dead.Add(nodePiece);
                        //Debug.Log("Dead fired");
                        node.OccupiedBlock.SetBlockType(0);
                        block.updating = false;
                        }
                        
                    }
                }
            }
            
            
            
    }
     //cleans the list after every update
            
    }        

    

    private void AddPoints(ref List<Node> connected, List<Node> nodes)
    {
        foreach(Node p in nodes)
        {
            bool doAdd = true;
            for(int i = 0; i < connected.Count; i++)
            {
                if(connected[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }
            if (doAdd) connected.Add(p);
        }
    }
    

    private FlippedPieces getFlipped(Node node)
    {
        FlippedPieces flip = null;
        for (int i = 0; i < _board._flipped.Count; i++)
        {
            if (_board._flipped[i].getOtherPiece(node) != null)
            {
                flip = _board._flipped[i];
                break;
            }
        }
        return flip;
        }
    // Update is called once per frame
    void GenerateGrid()
    {   
        var center = new Vector2((float) _width / 2 - .5f,(float) _height / 2 - .5f );
        _board = Instantiate(_boardPrefab, center, Quaternion.identity);
        var boardSprite = _board.GetComponent<SpriteRenderer>();
        boardSprite.size = new Vector2(_width, _height);
        _board._board = new Node[_width, _height];
        _board._update = new List<Node>();
        _board._nodes = new List<Node>();
        _board._blocks = new List<Block>();
        _board._lastMoved = new List<Node>();
        for (int x = 0; x < _width; x++){
            for (int y = 0; y < _height; y++) {
                Node node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                _board._nodes.Add(node);
                Block block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity);
                block.Init(fillPieceWithRandom(), Point.fromVector(node.Pos), GetNodeAtPosition(node.Pos), _board);
                _board._board[x, y] = GetNodeAtPosition(node.Pos);        


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
            for (int y = 0; y < _height; y++)
                {
                    Point p = new Point(x, y);
                    int val = getNodeAtPoint(p).OccupiedBlock.Value;
                    Node n = getNodeAtPoint(p);
                    if (val <= 0) continue;
                    int counter = 0;
                    remove = new List<int>();
                    if (IsConnected(n, true).Count > 0)
                    {
                        val = n.OccupiedBlock.Value;
                        counter++; //bust out of this loop if it makes an impossible grid. it does happen.
                        if (!remove.Contains(val))
                            remove.Add(val);
                        setValueAtPoint(p, replacePieceWithRandom(newValue(ref remove)));
                    }
                }
            }
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
        return random.Next(1, 5);
    }

        private int replacePieceWithRandom(int val) 
    {  
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        int replace = random.Next(1, 5);;
        if(replace == val){
            replace = random.Next(1, 5);
           // Debug.Log("replacement called");
           }
        
        return replace;}
        
    

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
        _board._board[p.x, p.y].OccupiedBlock.updating = false;
        _board._board[p.x, p.y].OccupiedBlock.SetBlockType(v);
    }

    Node getNodeAtPoint(Point p)
    {
        if(p.x < 0){p.x = 0;};
        if(p.y < 0){p.y = 0;};
        if(p.x > _width - 1){p.x = _width - 1;};
        if(p.y > _height - 1){p.y = _height - 1;};
        
        return _board._board[p.x, p.y];
    }

        int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
        return val;
    }


    public void RemoveBlock(Block block) {
        _board._blocks.Remove(block);
        Destroy(block.gameObject);
    }

    public List<Node> IsConnected(Node p, bool main)
    {
        List<Node> connected = new List<Node>();
        int val = p.OccupiedBlock.Value;

        Point[] directions;  directions = new Point[]{
            Point.down,
            Point.left,
            Point.up,
            Point.right
            };
       
        //Debug.Log("checking value: " + p.OccupiedBlock.Value.ToString() + ", " + p.OccupiedBlock.index.x.ToString() + ", " + p.OccupiedBlock.index.y.ToString());

        foreach(Point dir in directions) //Checking if there is 2 or more same shapes in the directions
        {
            
            int same = 0;
            List<Node> line = new List<Node>();
            
        //Debug.Log("checking point here dir " + dir.x.ToString() + ", " + dir.y.ToString()) ;
            
            
            for(int i = 0; i < 3; i++)
            {
                
                Point check = Point.add(p.OccupiedBlock.index, Point.mult(dir, i));

               
                if(getNodeAtPoint(check).OccupiedBlock.Value == val)
                {
                    line.Add(getNodeAtPoint(check));
                    same++;
                }
            }
            
            if (same > 1) //If there are more than 1 of the same shape in the direction then we know it is a match
            Debug.Log("match found");
                AddPoints(ref connected, line); //Add these points to the overarching connected list
        }

        for(int i = 0; i < 2; i++) //Checking if we are in the middle of two of the same shapes
        {
            List<Node> line = new List<Node>();

            int same = 0;
            Point[] check = { Point.add(p.OccupiedBlock.index, directions[i]), Point.add(p.OccupiedBlock.index, directions[i + 2]) };
            foreach (Point next in check) //Check both sides of the piece, if they are the same value, add them to the list
            {
                //Debug.Log("debug value " + getNodeAtPoint(next).OccupiedBlock.Value.ToString());
                if (getNodeAtPoint(next).OccupiedBlock.Value == val)
                {
                    line.Add(_board.GetNodeAtPoint(next));
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);
        }

        if(main) //Checks for other matches along the current match
        {
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, IsConnected(connected[i], false));
        }


        return connected;
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


[System.Serializable]
public class FlippedPieces
{
    public Node one;
    public Node two;

    public FlippedPieces(Node o, Node t)
    {
        one = o; two = t;
    }

    public Node getOtherPiece(Node p)
    {
        if (p == one)
            return two;
        else if (p == two)
            return one;
        else
            return null;
    }
}

