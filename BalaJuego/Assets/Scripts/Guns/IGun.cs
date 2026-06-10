using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun : IEquipable
{
    public void addBullets(int bul);
    public void setBullets(int bul);

    public bool shoot();

    public Animator getAnim();
    public int getBullets();

    public void restart();

    public void setShooting(bool _shoot);

}
