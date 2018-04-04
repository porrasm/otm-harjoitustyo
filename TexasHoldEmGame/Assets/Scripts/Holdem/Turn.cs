public class Turn {

    public Turn() {
        NewTurn();
    }

    Player player;
    public bool fold;
    public int raise;
    public string turn;

    public override string ToString() {
        return turn;
    }

    public void SetPlayer(Player p) {
        player = p;
    }

    public void NewTurn() {
        fold = false;
        raise = 0;
        turn = "";
    }

}
