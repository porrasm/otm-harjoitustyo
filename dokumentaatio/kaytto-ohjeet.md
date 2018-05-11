## Pelin lataus

### Windows
Pelin viimesimmän buildatun version Windowsille tai MACille voi ladata kohdasta 'Releases':
[Releases](https://github.com/porrasm/otm-harjoitustyo/releases)

HUOM: Jotkin viruksentorjunta ohjelmistot saattavat varoittaa ohjelmasta. Myös esim VirusTotal.com varoittaa ohjelmasta. Kyseessä ei ole virus vaan todennäköisesti false positive sillä sama ongelma esiintyy useilla eri projekteilla ja useilla tietokoneilla Unity pelien Windows versioissa.


## Main Menu

Päävalikko on hyvin yksinkertainen ja siinä on 3 nappia. Host Match, Join Match ja Quit.

- Host Match avaa pelin luomis valikon
- Join Match avaa peliin liittymis valikon
- Quit sulkee pelin

### Pelin luominen

- Paina ensiksi päävalikosta Host Match.
- Sinulla on 2 asetusta. Voit asettaa pelille salasanan ja voit asettaa pelin buy inin eli rahamäärän joka jaetaan alussa kaikille.
- Voit jättää kentät tyhjiksi tai voit halutessasi muuttaa arvoja.
- Paina Create Match

### Peliin liittyminen

- Paina ensiksi päävalikosta Join Match.
- Voit joko liittyä peliin salasanan avulla. Kirjoita salasana kenttään ja paina Join Private Match
- Vaihtoehtoisesti voit myös painaa Quick Join joka liittyy automaattisesti avoimeen peliin.

## Pelin toiminnallisuus

### Pelin aloitus

Kun olet joko luonut pelin tai liittynyt peliin, sinut siirretään peliaulaan. Peliaulassa näet kaikki muut pelaajat ja heidän nimensä. Kirjoita nimesi nimikenttään ja paina Ready, kun olet valmis.

Kun pelaajia on vähintään 2 ja kun kaikki pelaajat ovat valmiita, pelin hostille avautuu nappi 'Start Game'. HUOM: Vain pelin host voi aloittaa pelin ja vasta silloin kuin kaikki pelaajat ovat valmiita.

### Pelaaminen

[Texas Hold 'em säännöt](https://www.pokerlistings.com/poker-rules-texas-holdem)

Pelin pelaaminen tapahatuu vasemmassa alareunassa olevan käyttöliittymän avulla.

- Käyttöliittymän vihreän napin avulla voit joko Checkata, Callata tai mennä All In jos rahat loppuvat kesken.
- Käyttöliittymän keltainen nappi on Raise nappi. Napin oikealla puolella on tekstisyöte johon voit kirjoittaa korotuksesi määrän muodossa 'sataset.sentit'
- Kääyttöliittymän punainen nappi on Fold nappi ja sitä painamalla foldaat kierroksen.

### Käytöliittymän muut tiedot
Käyttöliittymässä on useita laatikoita, joiden merkitys on selitetty alhaalla.

- Call napin oikealla puolella oleva laatikko kertoo kuinka paljon sinun pitää callata.
- Bet laatikko kertoo sinun kokonaispanoksesi kyseisellä kierroksella.
- Money laatikko näyttää kuinka paljon sinulla on rahaa jäljellä.
- Table laatikko näyttää kuinka paljon rahaa pöydässä on sillä hetkellä.
- Ylin leveä tekstilaatikko kertoo mitä sinulla on kädessä. Siinä voisi lukea esimerkiksi 'One Pair' tai 'Full House' jos olet oikein onnekas.

Tämän lisäksi pöydän ympärillä on valkoiset laatikot joiden sisällä on pelaajien nimet, sekä pelaajien rahamäärät.

## Yksinpeli ja testaus yksin

Peli on tällä hetkellä ainoastaan moninpeli, eli et voi pelata sitä yksin. Voit kuitenkin pelata sitä itseäsi vastaan avaamalla pelin auki useamman kerran. Sovelluksen avaaminen useamman kerran toimii ainakin Windowsilla.

## BUGEJA

- Välillä tasapelitilanteessa peli laskee väärän voittajan
