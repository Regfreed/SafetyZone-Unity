internal class Values
{
    public static int board_size = 8;
    public static int choice = 1;   // 1 for queens and 2 for all

    /*nakon sto sam podjelio sahovnicu na manje dijelove trebao sam pronaci pravi broj za podjelit figure na pravo mjesto
     to sam napravio tako da sam nasao prvo odgovarajucu poziciju na sahovnici za dolnji lijevi i gornji desni kut
    zbrojio sam apsolutne vrijednosti x koordinata i podjelio to sa brojem polja-1 jer lista mojih elementa kreće od 0, stoga
    ce svaki puta prva figura uvijek biti na pravom mjestu a ostale će se poredati prema njoj*/
    public static float board_size_multiplier = 0.66f;
    public static float board_size_subtructor = -2.3f;
    public static float piece_size = 3.0f;
    public static bool solution = false;
}