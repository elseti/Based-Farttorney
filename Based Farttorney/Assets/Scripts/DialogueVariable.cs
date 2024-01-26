public class DialogueVariable<T>
{
    private string _key;
    private T _value;
    
    public DialogueVariable(string key, T value)
    {
        _key = key;
        _value = value;
    }

    public void SetKey(string key)
    {
        _key = key;
    }

    public string GetKey()
    {
        return _key;
    }

    public void SetValue(T value)
    {
        _value = value;
    }

    public T GetValue()
    {
        return _value;
    }
}