using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.Utilities {

    public class FrameCounter {

        private Game game ;

        public FrameCounter(Game game) {
            this.game = game ;

        }

        public long TotalFrames { get ; private set ; }
        public float TotalSeconds { get ; private set ; }
        public float AverageFramesPerSecond { get ; private set ; }
        public float CurrentFramesPerSecond { get ; private set ; }

        public const int MAXIMUM_SAMPLES = 100 ;

        private Queue<float> _sampleBuffer = new Queue<float>() ;

        public bool Update(float deltaTime) {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MAXIMUM_SAMPLES) {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            } 
            else {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
            return true ;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {

            //rather awkward, but we update this every frame so we might as well tie the two together
            Update((float)gameTime.ElapsedGameTime.TotalSeconds) ;
            string fps = string.Format("FPS: {0}", (int)this.AverageFramesPerSecond) ;
            spriteBatch.DrawString(game.Content.Load<SpriteFont>("fonts/default_ui"), fps, new Vector2(0, 0), Color.White) ;
        }
    }
}
