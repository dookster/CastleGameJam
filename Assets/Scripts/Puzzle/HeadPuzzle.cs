using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeadPuzzle : MonoBehaviour {

    /// <summary>
    /// 
    ///   1 2 3 4 
    ///   5 6 7 8
    ///   etc...
    /// 
    /// </summary>

    public int width = 4;
    public int height = 4;

    public int removeFrom = 1;
    public int removeTo = 3;

    public PuzzlePiece[,] puzzlePieces;

    public Settings settings;

    public Elephant creature;

    public bool exitPuzzle = false;

    public bool active = true;

	// Use this for initialization
	void Start ()
    {
        puzzlePieces = new PuzzlePiece[width, height];

        //for(int n = 0; n < width * height; n++)
        //{
           
        //}

        // create pieces
	    for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject go = new GameObject("Puzzle " + x + "," + y);
                puzzlePieces[x, y] = go.AddComponent<PuzzlePiece>();
                puzzlePieces[x, y].settings = settings;
                puzzlePieces[x, y].ownerPuzzle = this;

                BoxCollider coll = go.AddComponent<BoxCollider>();

                go.transform.localPosition = new Vector3(x, 0, -y);
                //go.transform.localRotation = transform.rotation;
            }
        }

        // connect pieces
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PuzzlePiece piece = puzzlePieces[x, y];

                // connect pieces in the grid (not links)
                if (x > 0) piece.west = puzzlePieces[x - 1, y];
                if (x < width - 1) piece.east = puzzlePieces[x + 1, y];
                if (y > 0) piece.north = puzzlePieces[x, y - 1];
                if (y < height - 1) piece.south = puzzlePieces[x, y + 1];

                // remove edge links
                if (x == 0) piece.linkState ^= PuzzlePiece.Link.west;
                if (x == width-1) piece.linkState ^= PuzzlePiece.Link.east;
                if (y == 0) piece.linkState ^= PuzzlePiece.Link.north;
                if (y == height - 1) piece.linkState ^= PuzzlePiece.Link.south;

                //if (y == 3 && x == 3) piece.linkState = PuzzlePiece.Link.north;
            }
        }

        // remove random pieces
        List<PuzzlePiece> remainPieces = new List<PuzzlePiece>();
        foreach (PuzzlePiece p in puzzlePieces)
        {
            remainPieces.Add(p);
        }
        Debug.Log("RR " + remainPieces.Count);
        int tilesToClear = Random.Range(removeFrom, removeTo + 1);
        while(tilesToClear > 0)
        {
            PuzzlePiece rP = remainPieces[Random.Range(0, remainPieces.Count)];
            rP.linkState = PuzzlePiece.Link.none;
            remainPieces.Remove(rP);
            tilesToClear--;
        }
        

        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {
        //        if(Random.Range(0, width * height) < ((width * height) / 4))
        //        {
        //            puzzlePieces[x, y].linkState = PuzzlePiece.Link.none;
        //        }
        //    }
        //}

        // remove unconnected links
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PuzzlePiece piece = puzzlePieces[x, y];
                if(piece.LinkedNorth && !puzzlePieces[x, y-1].LinkedSouth )
                {
                    piece.linkState ^= PuzzlePiece.Link.north;
                }
                if (piece.LinkedEast && !puzzlePieces[x+1, y].LinkedWest)
                {
                    piece.linkState ^= PuzzlePiece.Link.east;
                }
                if (piece.LinkedSouth && !puzzlePieces[x, y + 1].LinkedNorth)
                {
                    piece.linkState ^= PuzzlePiece.Link.south;
                }
                if (piece.LinkedWest && !puzzlePieces[x - 1, y].LinkedEast)
                {
                    piece.linkState ^= PuzzlePiece.Link.west;
                }
            }
        }

        // rotate randomly and add graphics
        foreach (PuzzlePiece p in puzzlePieces)
        {
            if (exitPuzzle)
            {
                p.AddGraphics(settings.puzzlePieceEnd);
            }
            else
            {
                p.AddGraphics(settings.puzzlePiece);
            }
            p.RandomRotate();
            p.transform.SetParent(transform, false);
        }

        // Redo if rotated puzzle is solved (hopefully doesn't happen forever...)
        while (IsPuzzleSolved())
        {
            foreach (PuzzlePiece p in puzzlePieces)
            {
                p.RandomRotate();
            }
        }


        // quick end puzzle stuff...
        if (exitPuzzle)
        {
            MovePiecesOut();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void CheckForSolution()
    {
        if (IsPuzzleSolved())
        {
            if (exitPuzzle)
            {
                Debug.Log("GAME OVER");
                active = false;
            }
            else
            {
                Player.Instance.StopInteractingWithCreature();
            }
        }
    }

    private bool IsPuzzleSolved()
    {
        foreach(PuzzlePiece p in puzzlePieces)
        {
            if (!p.isFullyLinked()) return false;
        }
        return true;
    }


    // quick hack time
    private void MovePiecesOut()
    {
        puzzlePieces[0, 0].transform.Translate(-2, 0, 2, Space.World);
        puzzlePieces[1, 0].transform.Translate(2, 0, 2, Space.World);
        puzzlePieces[0, 1].transform.Translate(-2, 0, -2, Space.World); 
        puzzlePieces[1, 1].transform.Translate(2, 0, -2, Space.World);

        
    }

    public void MovePiecesIn()
    {
        StartCoroutine(SlidePiecesIn());
    }

    IEnumerator SlidePiecesIn()
    {
        yield return new WaitForSeconds(1f);

        iTween.MoveTo(puzzlePieces[0, 0].gameObject, iTween.Hash("position", puzzlePieces[0, 0].gameObject.transform.position + new Vector3(2, 0, -2), "time", 2, "easetype", "linear"));
        iTween.MoveTo(puzzlePieces[1, 0].gameObject, iTween.Hash("position", puzzlePieces[1, 0].gameObject.transform.position + new Vector3(-2, 0, -2), "time", 2, "easetype", "linear"));
        iTween.MoveTo(puzzlePieces[0, 1].gameObject, iTween.Hash("position", puzzlePieces[0, 1].gameObject.transform.position + new Vector3(2, 0, 2), "time", 2, "easetype", "linear"));
        iTween.MoveTo(puzzlePieces[1, 1].gameObject, iTween.Hash("position", puzzlePieces[1, 1].gameObject.transform.position + new Vector3(-2, 0, 2), "time", 2, "easetype", "linear"));
    }
}
