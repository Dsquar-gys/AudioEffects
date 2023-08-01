namespace AudioEffects.Effects
{
    public class Echo : IEffect
    {
        #region Private Members

        private int index = 0;
        private List<float> samples;

        #endregion
        #region Public Properties

        public int EchoDelay { get; private set; }
        public double EchoFactor { get; private set; }
        public int EchoRepetitions { get; private set; }

        #endregion
        #region Constructor

        public Echo(double seconds = 0.5, double factor = 0.5, int repetitions = 1)
        {
            EchoDelay = (int)(seconds * 44100);
            EchoFactor = factor;
            EchoRepetitions = repetitions;

            samples = new List<float>();

            for (int i = 0; i < EchoDelay * (repetitions + 1); i++)
                samples.Add(0f);
        }

        #endregion
        #region Public Methods

        public float ApplyEffect(float sample)
        {
            for (int repetition = 1; repetition <= EchoRepetitions; repetition++)
                samples[index + repetition * EchoDelay] += sample * (float)Math.Pow(EchoFactor, repetition);

            index++;
            if (index == EchoDelay)
            {
                index = 0;
                samples.RemoveRange(0, EchoDelay);
                for (int i = 0; i < EchoDelay; i++)
                    samples.Add(0f);
            }

            return Math.Min(1, Math.Max(-1, (sample + samples[index])));
        }

        public bool Equals(IEffect? other)
        {
            var echo = other as Echo;
            return echo != null &&
                   echo.EchoDelay == EchoDelay &&
                   echo.EchoFactor == EchoFactor &&
                   echo.EchoRepetitions == EchoRepetitions;
        }

        #endregion
        #region Override Methods

        public override bool Equals(object? obj) => Equals(obj as Echo);
        public override int GetHashCode() => EchoDelay * (int)EchoFactor * EchoRepetitions;

        #endregion
    }
}
