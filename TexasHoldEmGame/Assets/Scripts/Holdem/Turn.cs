public class Turn {

    public Turn() {
        NewTurn();
    }

    private Player player;
    public bool Fold;
    public int Pay;
    public string TurnString;

    public override string ToString() {
        return TurnString;
    }

    public void SetPlayer(Player p) {
        player = p;
    }

    public void NewTurn() {
        Fold = false;
        Pay = 0;
        TurnString = string.Empty;
    }
}
