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

    /// <summary>
    /// Sets the player of this turn.
    /// </summary>
    /// <param name="param1">Player</param>
    public void SetPlayer(Player p) {
        player = p;
    }

    /// <summary>
    /// Resets the turn.
    /// </summary>
    public void NewTurn() {
        Fold = false;
        Pay = 0;
        TurnString = string.Empty;
    }
}
