using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.HUD {

    class MiniMap {

        private Rectangle bounds ;
        private Vector2 position ;
        private Level level ;
        private List<HUDImage> map ;

        public MiniMap(Vector2 position, Rectangle boundaries, Level level) {
            this.position = position ;
            this.bounds = boundaries ;
            this.level = level ;
            map = new List<HUDImage>() ;
        }

        public void Update() {
            map = new List<HUDImage>() ;
            foreach (Room room in level.Rooms) {
                if (room == level.activeRoom)
                    map.Add(new HUDImage(Assets.Instance.activeRoom, (position + new Vector2((room.coordinate.X-level.activeRoom.coordinate.X)*32, (room.coordinate.Y-level.activeRoom.coordinate.Y)*32)))) ;
                else if (room == level.ChallengeRoom)
                    map.Add(new HUDImage(Assets.Instance.challengeRoom, (position + new Vector2((room.coordinate.X-level.activeRoom.coordinate.X)*32, (room.coordinate.Y-level.activeRoom.coordinate.Y)*32)))) ;
                else if (room.IsBeaten)
                    map.Add(new HUDImage(Assets.Instance.beatenRoom, (position + new Vector2((room.coordinate.X-level.activeRoom.coordinate.X)*32, (room.coordinate.Y-level.activeRoom.coordinate.Y)*32)))) ;
                else
                    map.Add(new HUDImage(Assets.Instance.nonBeatenRoom, (position + new Vector2((room.coordinate.X-level.activeRoom.coordinate.X)*32, (room.coordinate.Y-level.activeRoom.coordinate.Y)*32)))) ;
            }

        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (HUDImage img in map) {
                img.Draw(spriteBatch) ;
            }
        }

    }
}
