using LeagueSharp;
using SharpDX;

namespace RAREGangplank.Interfaces
{

    internal interface ISpell
    {

        double GetSpellDamage(Obj_AI_Base target);

        bool CastSpell(Vector2 pos);

        bool CastSpell(Obj_AI_Base target);


    }
}
