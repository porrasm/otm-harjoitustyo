# Viesti ohjaajille

## Unitystä

Koska ilmeisesti kaikilla ohjaajilla ei ollut Unity osaamista, tähän tiedostoon tulee yksinkertaiset ohjeet projektin käytöstä ja testailusta. Lopulliset sovelluksen käyttöohjeet tulevat erikseen.

### Unity Networking

Kun lueskelet koodia löydät varmasti useita muuttujia, joilla on header [SerializeField] tai [SyncVar] ja metodeja, joilla on header [Command] tai [ClientRpc]. Selitän lyhyesti näiden merkityksen.

On huomattavasti helpompaa ymmärtää mitä koodissa tapahtuu jos tunnet näiden merkityksen.

#### Muuttujan header [SerializeField]
Unity alustaa kaikki muuttujat, joille on asetettu tämä headeri. Nämä eivät suinkaan siis ole null vaan niiden arvo on määritelty ennalta Unityn editorissa.

Esimerkkinä toimii HoldemUI luokka, joka on täynnä tällaisia muuttujia.
<img src="kuvat/unityInspector.jpg" width="200">

#### Muuttujan header [SyncVar]
Tämä tarkoittaa, että Unity automaattisesti päivittää tämän muuttujan arvon serveriltä kaikille clienteille.

#### Metodin header [Command]
Command metodit ovat serverillä suoritettavaa koodia ja niiden nimen pitää alkaa Cmd. Nämä metodit suoritetaan vain serverillä. Molemmat serveri ja client voi kutsua tälläisen metodin serverillä. Tätä käytetään, kun client haluaa kertoa serverille jotain. Esimerkkinä on esim. Player luokassa oleva metodi CmdSetName(string name). Client kirjoittaa nimensä kenttään jonka jälkeen tieto nimen vaihdosta lähetetään serverille.

#### Metodin header [ClientRpc]
ClientRpc metodit clienteillä suoritettavaa koodia ja niiden nimen pitää alkaa Rpc. Nämä metodit suoritetaan kaikilla clienteillä. Tässä tapauksessa Host, eli pelin aloittaja on sekä server, että client joten koodi pyörii myös silloin serverillä. Rpc metodi voidaan kutsua vain ja ainoastaan serveriltä. Jos client haluaa kutsua Rpc metodin tulee se tehdä serverin kautta command metodin avulla. 

#### [Command] ja [ClientRpc] yhdessä
Otetaan esimerkiksi tilanne, jossa pelaaja haluaa päivittää nimensä.

- Client A vaihtaa nimeään ja kutsuu metodin CmdSetName(string name)
- CmdSetName(string name) suoritetaan serverillä ja se kutsuu metodin RpcSetName(string name): public void CmdSetName(string name) { RpcSetName(name); }
- RpcSetName(string name) suoritetaan jokaisella clientillä (paitsi client A:lla), joka vaihtaa pelaajan nimen muuttujaksi name.
- Eli Client A kertoo serverille uuden nimensä, joka taas kertoo tämän nimen eteenpäin kaikille muille clienteillä.

Toivottavasti sait jonkinlaisen ymmärryksen siitä, miten networking toimii Unityssä ja toivottavasti saat hieman enemmän selvää luokista ja niiden toiminnallisuuksista.

## Projektin käyttä Unityssä lyhyesti

### Projektin avaus

Projektin voi avata joko avaamalla Unity ja avaamalla sitä kautta projektin root kansio.
Unity projektin root kansio on otm-harjoitustyo/TexasHoldEmGame
Projektin saa auki myös avaamalla minkä tahansa projektin 'scenen'. Scenet löytyvät hakemistosta otm-harjoitustyo/TexasHoldEmGame/Assets/Scenes
Projektin ensimmäinen avaus voi viedä muutaman minuutin.

### Projektin tiedostot

Kaikki pelin tiedostot (C# luokat, tekstuurit etc.) sijaitsevat Assets hakemistossa otm-harjoitustyo/TexasHoldEmGame/Assets
Kaikki koodi, paitsi testit, löytyvät hakemistosta Assets/Scripts
Testit löytyvät Assets/Tests

### Projektin toiminnallisuuden testaus

Projektia voi testata joko suoraan Unityn kautta tai buildatusta sovelluksesta.

Testaus Unityssä:
- Avaa scene MainMenu.unity editorissa (File -> Open Scene -> Assets/Scenes/MainMenu.unity tai tuplaklikkaa sceneä Unityn selaimessa) 
- Paina Editorin yläosassa olevaa play nappia. Peli lataa muutaman sekunnin.

Testaus Buildatussa sovelluksessa:
- Pura kansio, jos latasit pelin netistä. Avaa holdem.exe
- Jos buildasit pelin itse, avaa Unityn luoma .exe tiedosto.

Ohjeet pelin aloitukseen löytyvät [käyttöohjeesta](https://github.com/porrasm/otm-harjoitustyo/blob/master/dokumentaatio/kaytto-ohjeet.md)

#### Multiplayerin testaus kahdella pelillä

Voit testata moninpeliä myös itseksesi. Tämä tapahtuu buildaamalla sovellus, jonka jälkeen voit avata pelin Unity editorissa ja buildatun sovelluksen kautta samanaikaisesti. Voit myös avata buildatun sovelluksen useammin kuin kerran samaan aikaan.

- Noudata [käyttöohjeissa](https://github.com/porrasm/otm-harjoitustyo/blob/master/dokumentaatio/kaytto-ohjeet.md) olveia ohjeita.


## Testien suoritus

Testien suoritus vaatii Unityn:
https://store.unity.com/

- Avaa ensiksi Unity projekti. (otm-harjoitustyo/TexasHoldEmGame/Assets/Scenes/default.unity)
- Kun Unity avautuu avaa valikko 'Window' ja valitse 'Test Runner'
- Valitse avautuneesta ikkunasta 'Play Mode'
- Paina joko 'Run All' tai 'Run Selected'.

Etenkin pelin toiminnallisuutta testaavat testit voivat vievät aikaa.
