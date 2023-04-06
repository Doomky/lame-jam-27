using System.Collections.Generic;

public abstract class ModifierHolder<TModifier, TModifiable> where TModifier : Modifier<TModifiable>
{
    protected HashSet<int> _keys = new();

    protected ModifierResource _resource = new();

    protected TModifiable _modifiable = default(TModifiable);

    public ModifierHolder()
    {

    }

    public void Bind(TModifiable modifiable)
    {
        this._modifiable = modifiable;
    }

    public void Unbind()
    {
        this._modifiable = default(TModifiable);
    }

    private bool CanAddModifierHolder(int key)
    {
        return !_keys.Contains(key);
    }

    public void Add(TModifier modifier)
    {
        if (modifier != null && this.CanAddModifierHolder(modifier.Key))
        {
            this._keys.Add(modifier.Key);
            this.AddModifier(modifier);
        }
    }

    protected abstract void AddModifier(TModifier modifier);
}
