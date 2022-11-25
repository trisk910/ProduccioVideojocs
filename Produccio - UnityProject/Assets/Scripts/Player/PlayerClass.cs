using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass
{
    private float hp;
    private float speed;
    private float jump;


    public PlayerClass(float hp, float speed, float jump)
    {
        this.hp = hp;
        this.speed = speed;
        this.jump = jump;
    }

    public void setHp(float hp) { this.hp = hp; }
    public float getHp() { return this.hp; }
    public void setSpeed(float speed) { this.speed = speed; }
    public float getSpeed() { return this.speed; }

}
