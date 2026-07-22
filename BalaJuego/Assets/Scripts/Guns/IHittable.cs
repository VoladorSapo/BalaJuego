public interface IHittable
{
    public bool getHit(IProyectile proyectile);
    public void Damage(int damage);
    public void Die();
}