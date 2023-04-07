using Sirenix.OdinInspector;

namespace Game
{
    public abstract class Soul : SerializedScriptableObject, ISoul
    {
        IPrimaryFragment ISoul.PrimaryFragement { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        ISecondaryFragment ISoul.SecondaryFragement { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        void ISoul.Bind(IPlayer player, bool isPrimary)
        {
            throw new System.NotImplementedException();
        }

        void ISoul.Unbind()
        {
            throw new System.NotImplementedException();
        }
    }
}
