# Vaatimusmäärittely pokeri pelille

## Sovelluksen tarkoitus

Sovelluksen tarkoituksena on antaa pelaajille hauska online pelikokemus. Peli on Texas Hold 'em joka on yksi maailman tunnetuimpia korttipelejä.

## Sovelluksen tyyppi

Sovellus toteutetaan Unity pelinä ja mahdollisena erillisenä nettisovelluksena.

## Pelaajien käyttäjät

Tarkoituksena on, että pelaaja voi tehdä oman käyttäjän ja liittyä tällä netissä oleviin pokeripöytiin jossa hän voi pelata muita käyttäjiä vastaan. Käyttäjän tiedot tallennetaan joko paikallisesti tai johonkin nettisovellukseen.

## Käyttöliittymä

Käyttöliittymän tulee olle selkeä ja sen pitäsi sisältää ainakin seuraavat osat:

### Kirjautumisruutu

- käyttäjänimen ja salasanan kirjoitus
- kirjatuminen sisään ja ulos

### Päävalikko

- pelin aloitus, lopetus, linkit kirjautumis ruutuun

### Peliruutu

- kaikki pelitoiminnot kuten panostus, foldaus yms.
- valikko josta pääsee päävalikkoon ja/tai saa auki asetus ikkunan

### Asetukset

- asetuksissa käyttäjän tulisi pystyä säätämään esim. äänenvoimakkuutta ja resoluutiota

## Perusversion tarjoama toiminnallisuus

HUOM: Pelillä ei tarkoiteta tässä tapauksessa itse sovellusta vaan 'pöytää' eli yhtä netissä auki olevaa peliä.

### Ennen kirjautumista

Käyttäjälle annetaan mahdollisuus joko kirjautumaan jo luodulle käyttäjälle tai luomaan uusi käyttäjätunnus.

### Kirjautumisen jälkeen

Käyttäjä pääsee päävalikkoon josta hän voi liittyä peliin tai luoda uuden pelin.

### Pelin toiminnallisuus

Pelin tulisi toteuttaa perinteisen Texas Hold 'emin toiminnallisuus. Käyttäjällä on tietty määrä rahaa/pelivaluuttaa, jolla hän voi pelata. Jos käyttäjän valuutta loppuu kesken, tämä poistetaan pelistä.

## Jatkokehitysideoita

Ideoita ja muita mahdollisia toiminnallisuuksia, jos aikaa riittää:

- Herokusovellus joka pitää kirjaa käyttäjistä sekä heidän tiedoistaan ja valuuttamäärästä. Tälläinen sovellus mahdollistaisi sen, että voitaisiin luoda 'leaderboard', joka listaisi menestyneimmät pelaajat. Myös muut pelaajat pystyisivät tällöin etsimään toisia pelaajia. Tälläinen sovellus kuitenkin voisi viedä jopa yhtä paljon aikaa kuin itse pelikin, joten sen prioriteetti ei ole suuri.
- online huoneet, joihin voi asettaa salasanan ja minimi buy inin
- yksinkertainen tekoäly, jotta voi pelata yksinpeliä
- scoreboard
- chat tai joku muu keskustelutapa
- ensisijaisesti pelistä tulee kaksiulotteinen, mutta jos aikaa riittää kolmiulotteinen fysiikka pohjainen pelitoiminnallisuus olisi hauska lisä peliin
