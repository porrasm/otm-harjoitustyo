## Pelin lataus

### Windows
Pelin viimesimmän buildatun version voi ladata tästä:
[tästä](https://drive.google.com/open?id=1it7eoycKKE_Dtn377UHfCfEqNNkFKwAJ)
Toisaalta voit myös buildata sovelluksen suoraan git projektista. Ohjeet [täällä](https://github.com/porrasm/otm-harjoitustyo/blob/master/ohjaajille.md)
Pura zip tiedosto ja avaa 'holdem.exe'.

### MAC

Ohjeet projektin testaukseen MACilla [täällä](https://github.com/porrasm/otm-harjoitustyo/blob/master/ohjaajille.md)


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