using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Game : MonoBehaviour
{
    
    public GameObject chesspiece;
    public Sprite board_4x4, board_5x5, board_6x6, board_7x7, board_8x8;

    // array for all positions on board
    public GameObject[,] positions = new GameObject[Values.board_size, Values.board_size];
    // array for chesspiece
    public GameObject[] players = new GameObject[Values.board_size];
    private List<string> pieces = new List<string>(new string[] { "black_rook", "black_knight", "black_queen", "black_bishop" });

    private List<string> selected = new List<string>();
    private bool gameOver = false;

    public IEnumerable<Player> AllPlayers;
    public Canvas Canvas;
    
    void Start()
    {
        
        Canvas = GetComponentInChildren<Canvas>();
        Canvas.gameObject.SetActive(false);

        switch (Values.board_size)
        {
            case 4: GameObject.FindGameObjectWithTag("Board").GetComponent<SpriteRenderer>().sprite = board_4x4; break;
            case 5: GameObject.FindGameObjectWithTag("Board").GetComponent<SpriteRenderer>().sprite = board_5x5; break;
            case 6: GameObject.FindGameObjectWithTag("Board").GetComponent<SpriteRenderer>().sprite = board_6x6; break;
            case 7: GameObject.FindGameObjectWithTag("Board").GetComponent<SpriteRenderer>().sprite = board_7x7; break;
            default: GameObject.FindGameObjectWithTag("Board").GetComponent<SpriteRenderer>().sprite = board_8x8; break;

        }
        //here we create chesspieces for playing the game
        if (Values.solution == false)
        {
            if (Values.choice == 1)
            {
                for (int i = 0; i < Values.board_size; i++)
                {
               
                    players = new GameObject[]
                    {
                        Create(pieces[2], i, 0)
                    };
                    SetPosition(players[0]);
                }
            }
            if (Values.choice == 2)
            {
                Values.solution = true;
                RandomPieces();

                for (int i = 0; i < Values.board_size; i++)
                {
                    if (BoardSolution(selected[0], 0) && AllPlayers != null) break;
                    selected.Add(selected[0]);
                    selected.RemoveAt(0);
                    if ((i + 1) == Values.board_size)
                    {
                        selected.RemoveRange(0, Values.board_size);
                        RandomPieces();
                        i = 0;
                    }
                }               
                foreach (var player in AllPlayers)
                {
                    SetPositionEmpty(player.GetXBoard(), player.GetYBoard());
                    player.SetXBoard(AllPlayers.ToList().IndexOf(player));
                    player.SetYBoard(0);
                    player.SetCoords();
                    player.HasTarget = false;
                    SetPosition(player.gameObject);
                }
                Values.solution = false;
            }   
        }
        else
        {
            if (Values.choice == 1)
            {
                for (int i = 0; i < Values.board_size; i++)
                {
                    selected.Add(pieces[2]);
                }
                BoardSolution(selected[0], 0);
                AllPlayers = GetComponentsInChildren<Player>();
            }
            //random riješenje
            else if (Values.choice == 2)
            {
                RandomPieces();

                for (int i = 0; i < Values.board_size; i++)
                {
                    if (BoardSolution(selected[0], 0) && AllPlayers!=null) break;
                    selected.Add(selected[0]);
                    selected.RemoveAt(0);
                    if((i+1) == Values.board_size)
                    {
                        selected.RemoveRange(0,Values.board_size);
                        RandomPieces();
                        i = 0;
                    }
                } 
            } 
        }
        AllPlayers = GetComponentsInChildren<Player>();
    }

    public void RandomPieces()
    {
        for (int i = 0; i < Values.board_size; i++)
        {
            selected.Add(pieces[UnityEngine.Random.Range(0, 4)]);
        }
    }
    
    public Boolean BoardSolution(string piece, int col)
    {
        Player player = GetComponent<Player>();
        GameObject obj;
        if (col >= Values.board_size)
        {
            InitateTargetCheck();
            if (AllPlayers.All(x => x.HasTarget.HasValue && x.HasTarget.Value == false)) return true;
        }

        players = new GameObject[]
        {
           Create(piece, 0, col)
        };

        SetPosition(players[0]);
        obj = players[0];
        player = obj.GetComponent<Player>();
        AllPlayers = GetComponentsInChildren<Player>();

        for (int i = 0; i < Values.board_size; i++)
        {
            positions[i, col] = player.gameObject; 
            InitateTargetCheck();
            if (AllPlayers.All(x => x.HasTarget.HasValue && x.HasTarget.Value == false))
            {
                if (col == Values.board_size - 1)
                {
                    if (BoardSolution(selected[col], col + 1)) return true;
                }
                else
                {
                    if (BoardSolution(selected[col+1], col + 1)) return true;
                }
            }
            RemovePosition(player);
        }
        DestroyImmediate(obj);
        AllPlayers = GetComponentsInChildren<Player>();
        return false;
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity, this.transform);
        Player cm = obj.GetComponent<Player>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Player cm = obj.GetComponent<Player>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }
        
    public void RemovePosition(Player player)
    {
        positions[player.GetXBoard(), player.GetYBoard()] = null;
    }

      // function for enabling cp to move and make position empty
    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void Update()
    {
       if (gameOver == true && Input.GetMouseButtonDown(0))
       {
           gameOver = false;

           SceneManager.LoadScene("Game");
       }
    }

    public void InitateTargetCheck()
    {
       foreach (var player in AllPlayers)
       {
           player.CheckForAttackedPlaces();
       }

       if (AllPlayers.All(x => x.HasTarget.HasValue && !x.HasTarget.Value) && Values.solution==false)
       {
           GetComponent<Game>().Winner();
       }
    }

    public void Winner()
    {     
       if (Values.solution == false)
       {
           gameOver = true;
            Canvas.gameObject.SetActive(true);
       }
    }
    public void Back()
    {
       SceneManager.LoadScene("StartMenu");
    }
} 

