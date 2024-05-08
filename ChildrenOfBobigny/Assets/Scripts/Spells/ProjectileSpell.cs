public class ProjectileSpell : Spell
{
    public override void Init(Data_Spell_Projectile spellData)
    {
        _spellData = spellData;
        print(_spellData.SpellName);
    }
}
