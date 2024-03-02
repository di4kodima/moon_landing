using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fisics : MonoBehaviour 
{
    float m;
    float f;       
    float jetM;
    float jetV;  
    float StartH;
    vect G; 
    float LandV;
    float Fps;

    vect accel;
    float trust;    
    vect v;      
    float h;
    float a;   

    Timer timer;
}

public struct vect
{
    public double x, y, z;
    public double modul()
    { return Math.Sqrt(x * x + y * y + z * z); }
    public double Mod
    {
        get { return Math.Sqrt(x * x + y * y + z * z); }
    }
    public vect(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public static vect operator +(vect a, vect b)
    {
        vect c = new vect();
        c.x = a.x + b.x;
        c.y = a.y + b.y;
        return c;
    }
    public static double operator *(vect a, vect b)
    {
        return a.x * b.x + a.y * b.y;
    }
    public static vect operator *(double c, vect a)
    {
        vect h = new vect();
        h.x = a.x * c;
        h.y = a.y * c;
        return h;
    }
    public static vect operator *(vect a, double c)
    {
        vect h = new vect();
        h.x = a.x * c;
        h.y = a.y * c;
        return h;
    }
    public static vect operator /(vect a, double c)
    {
        vect h = new vect();
        h.x = a.x / c;
        h.y = a.y / c;
        return h;
    }
    public static vect operator -(vect a, vect b)
    {
        vect c = new vect();
        c.x = a.x - b.x;
        c.y = a.y - b.y;
        return c;
    }
    public static vect operator &(vect a, vect b)
    {
        vect c = new vect();
        c.x = a.y * b.z - a.z * b.y;
        c.y = a.z * b.x - a.x * b.z;
        c.z = a.x * b.y - a.y * b.x;
        return c;
    }
}