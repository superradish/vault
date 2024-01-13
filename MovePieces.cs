using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    public static MovePieces instance;
    Board _board;

    Node moving = new Node();
    Point newIndex;
    Vector2 mouseStart;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _board = GetComponent<Board>();
    }

    void Update()
    {
        if(moving.OccupiedBlock != null)
        {
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            Debug.Log("This space for rent");
            newIndex = Point.clone(moving.OccupiedBlock.index);
            Point add = Point.zero;
            if (dir.magnitude > 8) // magnitude controls how soon a piece begins to be dragged, 8 feels about right, 32 is terrible
            {
                //make add either (1, 0) | (-1, 0) | (0, 1) | (0, -1) depending on the direction of the mouse point
                if (aDir.x > aDir.y)
                    add = (new Point((nDir.x > 0) ? 1 : -1, 0));
                else if(aDir.y > aDir.x)
                    add = (new Point(0, (nDir.y > 0) ? -1 : 1));
            }
            newIndex.add(add);

            Debug.Log("moving.occupiedblock index" + moving.OccupiedBlock.index.ToString());
            Vector2 pos = _board.GetPositionFromPoint(moving.OccupiedBlock.index);
            if (!newIndex.Equals(moving.OccupiedBlock.index))
                pos += Point.mult(new Point(add.x, -add.y), 16).ToVector();
            moving.OccupiedBlock.MovePositionTo(pos);
        }
    }

    public void MovePiece(Node piece)
    {
        Debug.Log("moved in movepieces");
        this.moving = piece;
        mouseStart = Input.mousePosition;
    }

    public void DropPiece()
    {
        //Debug.Log("dropped in movepieces");
        if (moving == null) return;
        if (!newIndex.Equals(moving.OccupiedBlock.index))
            _board.FlipPieces(moving.OccupiedBlock, newIndex, true);
        else
            _board.ResetPiece(moving);
        moving = null;
    }
}
