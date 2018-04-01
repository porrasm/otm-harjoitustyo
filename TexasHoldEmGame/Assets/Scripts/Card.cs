public class Card {

    int suit;
    int number;

    public void SetCard(int suit, int number) {
        this.suit = suit;
        this.number = number;
    }

    public override string ToString() {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }

}
