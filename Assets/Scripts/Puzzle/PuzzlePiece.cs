using UnityEngine;
using System.Collections;
using System;

public class PuzzlePiece : MonoBehaviour {

    public Settings settings;

    public HeadPuzzle ownerPuzzle;

    public PuzzlePiece north;
    public PuzzlePiece south;
    public PuzzlePiece east;
    public PuzzlePiece west;

    //public bool linkedNorth = true;
    //public bool linkedSouth = true;
    //public bool linkedEast = true;
    //public bool linkedWest = true;

    [Flags]
    public enum Link
    {
        none = 0x0,
        north = 0x1,
        east = 0x2,
        south = 0x4,
        west = 0x8
    }

    //public const int NORTH = 1;
    //public const int EAST = 2;
    //public const int SOUTH = 4;
    //public const int WEST = 8;

    public const Link cross = Link.north | Link.south | Link.east | Link.west;

    

    public const Link corner1 = (Link.north | Link.east);
    public const Link corner2 = (Link.east | Link.south);
    public const Link corner3 = (Link.south | Link.west);
    public const Link corner4 = (Link.west | Link.north);

    public const Link lineVert = Link.north | Link.south;
    public const Link lineHorz = Link.east| Link.west;

    public const Link tSection1 = Link.south | Link.east | Link.west;
    public const Link tSection2 = Link.north | Link.south | Link.west;
    public const Link tSection3 = Link.north | Link.east | Link.west;
    public const Link tSection4 = Link.north | Link.south | Link.east;

    public Link linkState = cross;

    public bool LinkedNorth { get { return (linkState & Link.north) == Link.north; } }
    public bool LinkedSouth { get { return (linkState & Link.south) == Link.south; } }
    public bool LinkedEast { get { return (linkState & Link.east) == Link.east; } }
    public bool LinkedWest { get { return (linkState & Link.west) == Link.west; } }

    //public int linkState = 15;

    public bool isFullyLinked()
    {
        bool linked = true;

        // linking towards an edge
        if (LinkedNorth)
        {
            if(north == null)
            {
                return false;
            }
            if (!north.LinkedSouth)
            {
                return false;
            }
        }
        if (LinkedSouth)
        {
            if (south == null)
            {
                return false;
            }
            if (!south.LinkedNorth)
            {
                return false;
            }
        }
        if (LinkedEast)
        {
            if (east == null)
            {
                return false;
            }
            if (!east.LinkedWest)
            {
                return false;
            }
        }
        if (LinkedWest)
        {
            if (west == null)
            {
                return false;
            }
            if (!west.LinkedEast)
            {
                return false;
            }
        }

        return linked;
        
    }

    public void RandomRotate()
    {
        
        switch(UnityEngine.Random.Range(0, 4))
        {            
            case 1:
                transform.Rotate(new Vector3(0, 90, 0));
                RotateLinksRight();
                break;
            case 2:
                transform.Rotate(new Vector3(0, -90, 0));
                RotateLinksLeft();
                break;
            case 3:
                transform.Rotate(new Vector3(0, 180, 0));
                RotateLinksAround();
                break;
        }
    }

    public void RotateLinksAround()
    {
        Link newLinkstate = Link.none;

        if (LinkedNorth) newLinkstate |= Link.south;
        if (LinkedEast) newLinkstate |= Link.west;
        if (LinkedSouth) newLinkstate |= Link.north;
        if (LinkedWest) newLinkstate |= Link.east;

        linkState = newLinkstate;
    }

    public void RotateLinksLeft()
    {
        Link newLinkstate = Link.none;

        if (LinkedNorth) newLinkstate |= Link.west;
        if (LinkedEast) newLinkstate |= Link.north;
        if (LinkedSouth) newLinkstate |= Link.east;
        if (LinkedWest) newLinkstate |= Link.south;

        linkState = newLinkstate;
    }

    public void RotateLinksRight()
    {
        Link newLinkstate = Link.none;

        if (LinkedNorth) newLinkstate |= Link.east;
        if (LinkedEast) newLinkstate |= Link.south;
        if (LinkedSouth) newLinkstate |= Link.west;
        if (LinkedWest) newLinkstate |= Link.north;

        linkState = newLinkstate;
    }


    void OnMouseDown()
    {
        float moveTime = 0.2f;
        float rotateTime = 0.2f;

        float moveLength = transform.parent.parent.localScale.y / 3f;

        if (iTween.Count(gameObject) == 0)
        {
            iTween.MoveBy(gameObject, iTween.Hash("y", moveLength, "time", moveTime));
            iTween.RotateBy(gameObject, iTween.Hash("y", 1f / 4f, "time", rotateTime));
            iTween.MoveBy(gameObject, iTween.Hash("y", -moveLength, "time", moveTime, "delay", rotateTime, "oncomplete", "ClickRotateAndCheck"));
        }
    }

    void ClickRotateAndCheck()
    {
        RotateLinksRight();
        ownerPuzzle.CheckForSolution();
    }

    public void AddGraphics()
    {
        Debug.Log("asda: " + linkState);

        GameObject prefab = settings.puzzlePiece[0];

        switch (linkState)
        {
            case Link.north:
            case Link.east:
            case Link.south:
            case Link.west:
                prefab = settings.puzzlePiece[1];
                break;

            case lineHorz:
            case lineVert:
                prefab = settings.puzzlePiece[2];
                break;

            case corner1:
            case corner2:
            case corner3:
            case corner4:
                prefab = settings.puzzlePiece[3];
                break;
            case tSection1:
            case tSection2:
            case tSection3:
            case tSection4:
                prefab = settings.puzzlePiece[4];
                break;
            case cross:
                prefab = settings.puzzlePiece[5];
                break;

        }

        GameObject go = Instantiate(prefab) as GameObject;
        go.transform.SetParent(transform);
        go.transform.localScale = transform.localScale;
        go.transform.localPosition = Vector3.zero;

        // rotate graphics to fit links
        switch (linkState)
        {
            //case Link.west:
            //case lineHorz:
            //case corner4:
            //case tSection3:

            //    break;

            case Link.north:
            case corner1:
            case tSection4:
            case lineVert:
                // rot 90
                go.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
                break;

            case Link.east:
            case corner2:
            case tSection1:
                // rot 180
                go.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
                break;

            case Link.south:
            case corner3:
            case tSection2:
                // rot -90
                go.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                break;

        }
    }

}
