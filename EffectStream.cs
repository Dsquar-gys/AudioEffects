using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace AudioEffects
{
    public class EffectStream : ISampleProvider
    {
        /// <summary>
        /// Stream that the effects would be applied to
        /// </summary>
        public WaveFileReader SourceStream { get; private set; }
        /// <summary>
        /// List of effects to be applied
        /// </summary>
        private List<IEffect> Effects { get; }

        #region Constructor

        public EffectStream() => Effects = new();

        #endregion
        #region Public Methods

        /// <summary>
        /// To add some filter or effect to the effect pipeline
        /// </summary>
        /// <param name="filter"></param>
        public void Register(IEffect filter) => Effects.Add(filter);
        /// <summary>
        /// To remove the equal filter from the pipeline
        /// </summary>
        /// <param name="filter"></param>
        public void Remove(IEffect filter) => Effects.Remove(filter);
        /// <summary>
        /// Clears the pipeline
        /// </summary>
        public void ClearEffects() => Effects.Clear();
        /// <summary>
        /// Set the Source stream for effect stream
        /// </summary>
        /// <param name="src"></param>
        public void SetSource(WaveFileReader src) => SourceStream = src;

        #endregion
        #region Inherited Properties from SourceStream
        
        public WaveFormat WaveFormat { get => SourceStream.WaveFormat; }

        #endregion

        public int Read(float[] buffer, int offset, int count)
        {
            float[] frame;

            // Frame counter for current Read iteration
            int frameCounter = 0;

            // While frame counter < max amount of frames for current Read iteration &&
            // frames are not over
            while (frameCounter < count / SourceStream.WaveFormat.Channels &&
                  (frame = SourceStream.ReadNextSampleFrame()) != null)
            {
                // Applying effects to the frame
                for (int i = 0; i < frame.Length; i++)
                    foreach (var effect in Effects)
                        frame[i] = effect.ApplyEffect(frame[i]);

                // Save frame samples to the buffer
                for (int i = 0; i < frame.Length; i++)
                    buffer[frameCounter * frame.Length + i] = frame[i];

                // Move counter to the next frame
                frameCounter++;
            }

            // Counting amount of read samples
            int read = frameCounter * SourceStream.WaveFormat.Channels;

            // Returns amount of read samples
            return read;
        }
    }
}
