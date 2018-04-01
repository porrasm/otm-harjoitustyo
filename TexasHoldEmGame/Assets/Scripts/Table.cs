public class Table {

    int tableValue;

    Card[] cards;
    int tableIndex;

    public void InitializeTable() {
        cards = new Card[5];
        tableIndex = 0;
    }
	
    public void DealCard(Card card) {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }



}
