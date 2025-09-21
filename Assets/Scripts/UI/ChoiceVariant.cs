public struct ChoiceVariant
{
    public string name;
    public object value;
    public ChoiceVariant(string name, object value) 
        => (this.name, this.value) = (name, value);
}
