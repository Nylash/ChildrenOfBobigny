using UnityEngine;

public class ProjectileSpell : Spell
{
    private Rigidbody _rb;
    private Data_Spell_Projectile _spellData;
    private Vector3 _birthPlace;
    private bool _initDone;

    public override void Init(Data_Spell_Projectile spellData)
    {
        _spellData = spellData;

        Instantiate(_spellData.ProjectileObject, transform);

        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.velocity = _spellData.Speed * Time.deltaTime * Direction;

        _birthPlace = transform.position;

        _initDone = true;
    }

    private void Update()
    {
        if (!_initDone)
            return;

        if (Vector3.Distance(_birthPlace, transform.position) > _spellData.Range)
        {
            KillProjectile();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Unit"))
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