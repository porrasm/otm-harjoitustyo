# Arkkitehtuurikuvaus

## Rakenne

## Päätoiminnallisuudet

Pelin päätoiminnallisuuksiin kuuluu pelin luonti, peliin liittyminen sekä yksittäisen vuoron pelaaminen.

Pelin luonti tapahtuu seuraavasti:

<img src="kuvat/createSequence.jpg" width="400">

Vastaavasti pelin liittyminen tapahtuu näin:

<img src="kuvat/joinSequence.jpg" width="400">

Molempien kaavioiden lopussa CusotmNetworkManager:ista taaksepäin ei lähde minkään laista tietoa, sillä LoadScene() poistaa kaiken scenen sisällön paitsi CustomNetworkManager:in.

Vuoron pelaaminen:

<img src="kuvat/turnSequence.jpg" width="400">

Lopussa kuitenkaan Player ei kerro TexasHoldEm:illä, että se on valmis vaan TexasHoldEm odottaa loopissa, kunnes pelaaja on valmis.

## Käyttöliittymä


## Sovelluslogiikka

Luokkakaavio

<img src="kuvat/luokkakaavio.jpg" width="400">
