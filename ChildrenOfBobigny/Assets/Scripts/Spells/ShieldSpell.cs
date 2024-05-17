using UnityEngine;
using static Utilities;

public class ShieldSpell : Spell
{
    private Data_Spell_Shield _spell_data;
    private float _birthTime;

    public override void Init(Data_Spell_Shield data)
    {
        _spell_data = data;

        //Adjust layer to shield from spell
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

        GameObject shield = Instantiate(_spell_data.ShieldObject, transform);
        shield.transform.localPosition += _spell_data.PositionOffset;
        transform.Rotate(Vector3.up, Vector3.SignedAngle(transform.forward, Direction, Vector3.up));

        _birthTime = Time.time;

        SetChildsLayers(gameObject, gameObject.layer);
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
