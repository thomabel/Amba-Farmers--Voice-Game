/// <summary>
/// Stores a reference to a FloatVariable scriptable object.
/// Can also use a constant value.
/// </summary>
[System.Serializable]
public class FloatReference
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;

    public float Value
    {
        get
        {
            return UseConstant ? 
                ConstantValue :
                Variable.Value;
        }
    }
}
