using UnityEngine;

public class ShieldSpell : Spell
{
    private Data_Spell_Shield _spell_data;
    private float _birthTime;

    public override void Init(Data_Spell_Shield data)
    {
        _spell_data = data;

        if(gameObject.layer == _spell_data.PlayerSpellLayer)
        {
            gameObject.layer = _spell_data.PlayerShieldLayer;
        }
        else if (gameObject.layer == _spell_data.EnemySpellLayer)
        {
            gameObject.layer = _spell_data.EnemyShieldLayer;
        }
        else
        {
            Debug.LogError("Shield spell launch without an appropriate layer " + gameObject.name + " (layer : " +  gameObject.layer + ")");
        }
        SetLayers(gameObject, gameObject.layer);

        Instantiate(_spell_data.ShieldObject, transform);
        //apply offset and direction for rotation

        _birthTime = Time.time;
        _initDone = true;
    }

    private void Update()
    {
        if (!_initDone)
            return;

        if( Time.time > _birthTime + _spell_data.Lifetime)
        {
            DestroySpell();
        }
    }

    private void DestroySpell()
    {
        Destroy(gameObject);
    }
}
