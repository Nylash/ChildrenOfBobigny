using UnityEngine;

public class ProjectileSpell : Spell
{
    private Rigidbody _rb;
    private Vector3 _direction;
    private Data_Spell_Projectile _spellData;
    private Vector3 _birthPlace;
    private float _timerTrajectory;

    public override void Init(Data_Spell_Projectile spellData)
    {
        _spellData = spellData;

        Instantiate(_spellData.ProjectileObject, transform);
        _direction = new Vector3(PlayerAimManager.Instance.AimDirection.x, 0, PlayerAimManager.Instance.AimDirection.y);
        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.velocity = _spellData.Speed * Time.deltaTime * _direction;

        _birthPlace = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(_birthPlace, transform.position) > _spellData.Range)
        {
            KillProjectile();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<BasicEnemy>().TakeDamage(_spellData.Damage);
            KillProjectile();
        }
    }

    private void KillProjectile()
    {
        Destroy(gameObject);
    }
}