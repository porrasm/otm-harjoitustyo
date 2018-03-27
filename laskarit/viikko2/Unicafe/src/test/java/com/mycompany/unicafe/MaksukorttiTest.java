package com.mycompany.unicafe;

import static org.junit.Assert.*;
import org.junit.Before;
import org.junit.Test;

public class MaksukorttiTest {

    Maksukortti kortti;

    @Before
    public void setUp() {
        kortti = new Maksukortti(10);
    }

    @Test
    public void luotuKorttiOlemassa() {
        assertTrue(kortti!=null);      
    }
    
    @Test
    public void saldoOikeinAlussa() {
        
        assertEquals(10, kortti.saldo());
        
    }
    
    @Test
    public void lataaminenToimii() {
        
        kortti.lataaRahaa(5);
        assertEquals(15, kortti.saldo());
        kortti.lataaRahaa(-5);
        //assertEquals(15, kortti.saldo());
        
        
    }
    
    @Test
    public void saldoVaheneeOikein() {
        
        kortti.lataaRahaa(990);
        
        Kassapaate p = new Kassapaate();
        
        if (!p.syoEdullisesti(kortti)) {
            fail();
        }
        assertEquals(760, kortti.saldo());
        if (!p.syoMaukkaasti(kortti)) {
            fail();
        }
        assertEquals(360, kortti.saldo());
        
        if (!kortti.otaRahaa(360)) {
            fail();
        }
        
        assertEquals(0, kortti.saldo());
        
        if (kortti.otaRahaa(10)) {
            fail();
        }
        
        if (p.syoEdullisesti(kortti) || p.syoMaukkaasti(kortti)) {
            fail();
        }
        
    }
}
