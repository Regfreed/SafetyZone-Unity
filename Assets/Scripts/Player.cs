using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool? HasTarget = null;

    // References
    public GameObject controller;
    public GameObject movePlate;

    // Positions
    private int xBoard = -1;
    private int yBoard = -1;

    // References for all the sprites that the chesspiece can be 
    public Sprite black_queen, black_knight, black_bishop, black_rook;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        
       
        switch (this.name) 
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; this.GetComponent<Transform>().localScale = new Vector3(Values.piece_size, Values.piece_size, -1); break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; this.GetComponent<Transform>().localScale = new Vector3(Values.piece_size, Values.piece_size, -1); break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; this.GetComponent<Transform>().localScale = new Vector3(Values.piece_size, Values.piece_size, -1); break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; this.GetComponent<Transform>().localScale = new Vector3(Values.piece_size, Values.piece_size, -1); break;
        }
        // take the instantiated location and adjust the transform
        SetCoords();
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= Values.board_size_multiplier;
        y *= Values.board_size_multiplier;

        x += Values.board_size_subtructor;
        y += Values.board_size_subtructor;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard() { return xBoard; }
    public int GetYBoard() { return yBoard; }
    public void SetXBoard(int x) { xBoard = x; }
    public void SetYBoard(int y) { yBoard = y; }

    private void OnMouseUp()
    {
        if (Values.solution == false)
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }
    }

    //destroy move plates of old chesspiece
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
                DestroyImmediate(movePlates[i]);
        }     
    }
    //this function show posible moves for certian chesspiece
    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_knight":
                LMovePlate();
                break;
            case "black_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_rook":
                LineMovePlate(0, 1);
                LineMovePlate(1, 0);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();
        GameObject cp;
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        //making sure that our positions are still on board
        while (sc.PositionOnBoard(x, y))
        {
            cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
                x += xIncrement;
                y += yIncrement;
            }
            else
            {
                MovePlateSpawnAttack(x, y);
                x += xIncrement;
                y += yIncrement;
            }
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        //getting component from controller
        Game sc = controller.GetComponent<Game>();
        //checking if position is on the board, then gettin actual chesspiece if its there, and if not then create plate, else if there is piece then need to block action

        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else 
            {
                MovePlateSpawnAttack(x, y);
            }
        }
    }
    //function for setting piece on board 
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= Values.board_size_multiplier;
        y *= Values.board_size_multiplier;

        x += Values.board_size_subtructor;
        y += Values.board_size_subtructor;

        // we need to set a reference for moveplate
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity, controller.transform);

        //we need to access moveplate script so we can set reference to our self and also set the coords of piece
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX,matrixY);
    }
    public void MovePlateSpawnAttack(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= Values.board_size_multiplier;
        y *= Values.board_size_multiplier;

        x += Values.board_size_subtructor;
        y += Values.board_size_subtructor;

        // we need to set a reference for moveplate
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity, controller.transform);
        mp.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        //we need to access moveplate script so we can set reference to our self and also set the coords of piece
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
    public void CheckForAttackedPlaces()
    {
        int attacked = 0;
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameObject cp;

        Game sc = controller.GetComponent<Game>();
        GameObject[] movePlates;
        for (int i = 0; i < Values.board_size; i++)
        {
            for (int j = 0; j < Values.board_size; j++)
            {
                cp = controller.GetComponent<Game>().GetPosition(i, j);
                if (sc.GetPosition(i, j) != null)
                {                   
                    cp.GetComponent<Player>().SetXBoard(i);
                    cp.GetComponent<Player>().SetYBoard(j);
                    cp.GetComponent<Player>().SetCoords();

                    //seting controller to keeping track of cords of piece
                    controller.GetComponent<Game>().SetPosition(cp);
                    InitiateMovePlates();

                    movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
                    foreach (var plate in movePlates)
                    {
                        if (plate.GetComponent<MovePlate>().attack)
                        {
                            attacked++;
                        }
                    }                   
                    DestroyMovePlates();
                }
            }
        }

        if (attacked == 0)
        {
            HasTarget = false;
        }
        else
        {
            HasTarget = true;
        }
    }
}
