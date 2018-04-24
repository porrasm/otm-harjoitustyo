public class Turn {

    public Turn() {
        NewTurn();
    }

    private Player player;
    public bool Fold;
    public int Raise;
    public string TurnString;

    public override string ToString() {
        return TurnString;
    }

    public void SetPlayer(Player p) {
        player = p;
    }

    public void NewTurn() {
        Fold = false;
        Raise = 0;
        TurnString = string.Empty;
    }
}
