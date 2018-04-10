# Viesti ohjaajille

## Unitystä

Koska ilmeisesti kaikilla ohjaajilla ei ollut Unity osaamista, tähän tiedostoon tulee yksinkertaiset ohjeet projektin käytöstä ja testailusta. Lopulliset sovelluksen käyttöohjeet tulevat erikseen.

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
- Avaa scene default.unity editorissa (File -> Open Scene -> Assets/Scenes/default.unity tai tuplaklikkaa sceneä Unityn selaimessa) 
- Paina Editorin yläosassa olevaa play nappia. Peli lataa muutaman sekunnin.

Testaus Buildatussa sovelluksessa:
- Pura kansio, jos latasit pelin netistä. Avaa holdem.exe
- Jos buildasit pelin itse, avaa Unityn luoma .exe tiedosto.

Ohjeet pelin aloitukseen löytyvät [käyttöohjeesta](https://github.com/porrasm/otm-harjoitustyo/blob/master/dokumentaatio/kaytto-ohjeet.md)

#### Multiplayerin testaus kahdella pelillä

Voit testata moninpeliä myös itseksesi. Tämä tapahtuu buildaamalla sovellus, jonka jälkeen voit avata pelin Unity editorissa ja buildatun sovelluksen kautta samanaikaisesti.

- Unity editorissa paina 'LAN Host' pelin alkaessa.
- Buildatussa sovelluksessa paina 'LAN Client'.

Muista painaa Unity editorissa P vasta sen jälkeen kun olet liittynyt peliin toisella pelaajalla. Muista myös painaa SPACE molemmissa peleissä.

### Projektin buildaus, testaus MACilla

Koska käytän koneellani Windowsia, enkä ole ehtinyt asentaa Unityn Mac modulea, en pysty buildaamaan peliä Macille. Tämä on kuitenkin hyvin yksinkertaista tehdä.

- Avaa scene 'default.unity'
- File -> Build Settings
- Avautuneessa ikkunassa paina 'Add Open Scenes'
- Paina 'Build' ja valitse sijainti. Buildaus kestää 1-3 minuuttia.

Jos buildaus jostain syystä epäonnistuu, voit aina testata peliä suoraan Unityn editorissa.

## Testien suoritus

Testien suoritus vaatii Unityn:
https://store.unity.com/

- Avaa ensiksi Unity projekti. (otm-harjoitustyo/TexasHoldEmGame/Assets/Scenes/default.unity)
- Kun Unity avautuu avaa valikko 'Window' ja valitse 'Test Runner'
- Valitse avautuneesta ikkunasta 'Play Mode'
- Paina joko 'Run All' tai 'Run Selected'.

Etenkin pelin toiminnallisuutta testaavat testit voivat vievät aikaa.
