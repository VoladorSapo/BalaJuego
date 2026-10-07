public interface IHittable
{
    public bool getHit(IEffectSource proyectile);
    public void Damage(int damage);
    public void Die();
}