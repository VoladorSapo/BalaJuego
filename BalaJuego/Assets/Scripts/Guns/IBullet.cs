
using UnityEngine;

public interface IBullet
{
    public void InstantiateBullet(GameObject shooter, float angle);

    public int getDamage();
}
