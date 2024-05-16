using UnityEngine;

public class ProjectileSpell : Spell
{
    private Data_Spell_Projectile _spellData;
    private Rigidbody _rb;
    private Vector3 _birthPlace;

    public override void Init(Data_Spell_Projectile data)
    {
        _spellData = data;

        Instantiate(_spellData.ProjectileObject, transform);

        SetLayers(gameObject, gameObject.layer);

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
            DestroySpell();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Unit":
                collision.gameObject.GetComponent<BasicEnemy>().TakeDamage(_spellData.Damage);
                DestroySpell();
                break;
            case "Player":
                PlayerHealthManager.Instance.TakeDamage(_spellData.Damage);
                DestroySpell();
                break;
            case "Shield":
                DestroySpell();
                break;
            default:
                break;
        }
    }

    public void DestroySpell()
    {
        Destroy(gameObject);
    }
}