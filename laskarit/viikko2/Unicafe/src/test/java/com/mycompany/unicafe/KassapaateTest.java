/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mycompany.unicafe;

import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author porrasm
 */
public class KassapaateTest {
    
    Kassapaate p;
    
    public KassapaateTest() {
    }
    
    @Before
    public void setUp() {
        p = new Kassapaate();
    }
    
    @Test
    public void oikeaAlustus() {
        
        assertEquals(100000, p.kassassaRahaa());
        assertEquals(0, p.maukkaitaLounaitaMyyty());
        assertEquals(0, p.edullisiaLounaitaMyyty());
        
    }
    
    @Test
    public void kateisOsto() {
        
        int rahaa = p.kassassaRahaa();
        
        assertEquals(0, p.syoEdullisesti(240));
        rahaa += 240;
        assertEquals(1, p.syoEdullisesti(241));
        rahaa += 240;
        assertEquals(239, p.syoEdullisesti(239));
        
        assertEquals(rahaa, p.kassassaRahaa());
        
        assertEquals(0, p.syoMaukkaasti(400));
        rahaa += 400;
        assertEquals(1, p.syoMaukkaasti(401));
        rahaa += 400;
        assertEquals(200, p.syoMaukkaasti(200));
        
        assertEquals(rahaa, p.kassassaRahaa());
        
        assertEquals(2, p.edullisiaLounaitaMyyty());
        assertEquals(2, p.maukkaitaLounaitaMyyty());
        
    }

    @Test
    public void korttiOsto() {
        
        Maksukortti k = new Maksukortti(1000);
        int rahaa = p.kassassaRahaa();
        int saldo = k.saldo();
        
        if (!p.syoEdullisesti(k)) {
            fail("Edullinen osto ei menyt läpi.");
        }
        
        saldo -= 240;
        assertEquals(saldo, k.saldo());
        
        if (!p.syoMaukkaasti(k)) {
            fail("Maukas osto ei mennyt läpi.");
        }
        
        saldo -= 400;
        assertEquals(saldo, k.saldo());
        
        assertEquals(rahaa, p.kassassaRahaa());
        
        
        k = new Maksukortti(100);
        saldo = 100;
        
        if (p.syoEdullisesti(k)) {
            fail("Edullinen meni läpi vaikka sen ei pitänyt mennä.");
        }
        
        assertEquals(100, k.saldo());
        
        if (p.syoMaukkaasti(k)) {
            fail("Maukas osto meni läpi vaikka sen ei pitänyt mennä.");
        }
        
        assertEquals(100, k.saldo());
        
        assertEquals(1, p.edullisiaLounaitaMyyty());
        assertEquals(1, p.maukkaitaLounaitaMyyty());
        
    }
    
    @Test
    public void korttiLataus() {

        Maksukortti k = new Maksukortti(1000);
        int saldo = 1000;
        int kassa = p.kassassaRahaa();
        
        p.lataaRahaaKortille(k, 1000);
        kassa += 1000;
        saldo += 1000;
        
        assertEquals(saldo, k.saldo());
        assertEquals(kassa, p.kassassaRahaa());
        
        p.lataaRahaaKortille(k, -100);
        
        assertEquals(saldo, k.saldo());
        assertEquals(kassa, p.kassassaRahaa());
        
    }

}
