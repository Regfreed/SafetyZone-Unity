using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;
    GameObject reference = null;

    // Board positions not world positions
    public int matrixX;
    public int matrixY;

    //false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        gameObject.GetComponent<Transform>().localScale = new Vector3(Values.piece_size + 0.5f, Values.piece_size + 0.5f, -1);
    }

    //function for tapping on screen
    // for box collider 2d  to work corectly
    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
        if (cp == null)
        {
            controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Player>().GetXBoard(), reference.GetComponent<Player>().GetYBoard());

            //setting x and y position of piece and seting new cords for that piece
            reference.GetComponent<Player>().SetXBoard(matrixX);
            reference.GetComponent<Player>().SetYBoard(matrixY);
            reference.GetComponent<Player>().SetCoords();

            //seting controller to keeping track of cords of piece
            controller.GetComponent<Game>().SetPosition(reference);
            reference.GetComponent<Player>().DestroyMovePlates();

            controller.GetComponent<Game>().InitateTargetCheck();
        }
        else { reference.GetComponent<Player>().DestroyMovePlates(); }
    }
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }
    public void SetReference(GameObject obj)
    {
        reference = obj;
    }
    public GameObject GetReference()
    {
        return reference;
    }
}
