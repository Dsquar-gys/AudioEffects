namespace AudioEffects.Effects
{
    public class VolumeMultiply : IEffect
    {
        #region Private Members

        private float multiplyVolume;

        #endregion
        #region Constructor

        public VolumeMultiply(float multiply) => multiplyVolume = multiply;

        #endregion
        #region Public Methods

        public float ApplyEffect(float sample) => sample * multiplyVolume;

        public bool Equals(IEffect? other)
        {
            var volumeMultiply = other as VolumeMultiply;
            return volumeMultiply != null && volumeMultiply.multiplyVolume == multiplyVolume;
        }

        #endregion
        #region Override Methods

        public override bool Equals(object? obj) => Equals(obj as VolumeMultiply);
        public override int GetHashCode() => multiplyVolume.GetHashCode();

        #endregion
    }
}
