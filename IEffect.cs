namespace AudioEffects
{
    // Also required to override Equals and GetHashCode for each effect!
    public interface IEffect : IEquatable<IEffect>
    {
        float ApplyEffect(float frame);
    }
}
