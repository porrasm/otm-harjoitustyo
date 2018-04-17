## Pelin lataus

### Windows
Pelin viimesimmän buildatun version Windowsille tai MACille voi ladata tästä:
[holdem.17.04.18.zip](http://www.mediafire.com/file/p30xsl670k13gso/holdem.17.04.18.zip)

HUOM: Jotkin viruksentorjunta ohjelmistot saattavat varoittaa ohjelmasta. Myös esim VirusTotal.com varoittaa ohjelmasta. Kyseessä ei ole virus vaan todennäköisesti false positive sillä sama ongelma esiintyy useilla eri projekteilla ja useilla tietokoneilla Unity pelien Windows versioissa.

## Pelin Aloitus:

- Koska pelissä ei ole vielä graafista käyttöliittymää, pelin aloitus tapahtuu suurilta osin näppäimillä.
- Jos haluat pelata peliä yksin, paina 'H' tai klikkaa vasemmasta yläkulmasta 'LAN Host'.
- Jos haluat pelata peli netissä, klikkaa 'Enable Match Maker' 
- Paina P, kun kaikki pelaajat ovat liittyneet. Tämä alustaa pelin.
- Paina SPACE. Tämä asettaa luokan Player.Ready = true
- Peli toimii tämän jälkeen automaattisesti.

## Pelin toiminnallisuus

[Texas Hold 'em](https://www.pokerlistings.com/poker-rules-texas-holdem)

Pelin vasemmassa alareunassa on käyttöliittymä, jossa on napit Call tai Check, Raise ja Fold.
Kirjoita Raise napin oikealla puolella olevaan tekstisyötteeseen korotettava summa desimaalilukuna ja paina sitten Raise.

### Muu toiminnallisuus

Jos pidät pohjassa hiiren oikeaa näppäintä, voit liikuttaa kameraa.
Jos klikkaat pelikorttia, voit raahata sitä. Scrollaaminen tuo korttia lähemmän tai kauemmas. WASD avulla voit kääntää korttia.


## Multiplayer

Käyttöliittymä menee päällekkäin, mutta nappeja voi painaa.

Hostaaminen:

- Aloita peli, mutta älä paina 'H' tai klikkaa 'LAN Host'.
- Klikkaa 'Enable Match Maker'
- Paina 'Create Internet Match'. Voit myös asettaa serverin nimen samalla.
- Odota kunnes kaikki pelaajat ovat liittyneet ja paina P ja sitten SPACE.

Liittyminen:

- Aloita peli, mutta älä paina 'H' tai klikkaa 'LAN Host'.
- Klikkaa 'Enable Match Maker'
- Paina 'Join Internet Match' ja sitten 'Join Match (serverin nimi)'.
- Paina SPACE.

## BUGEJA

- pelin aloitus käyttöliittymä on buginen
- peli hyväksyy negatiivisen panostuksen
- peliin liittyvä pelaaja, eli kaikki muut paitsi host, eivät näe kortteja.
