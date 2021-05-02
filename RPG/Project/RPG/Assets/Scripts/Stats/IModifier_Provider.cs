using System.Collections.Generic; 

namespace RPG.Stat
{
    public interface IModifier_Provider
    {
        IEnumerable<float> Get_Additive_Modifiers(Stats stat);
        IEnumerable<float> Get_Percentage_Modifiers(Stats stat);
    }
}