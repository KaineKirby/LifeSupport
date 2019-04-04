using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.GameObjects {
    abstract class Enemy : Actor {

        private Player player;
        private Room currentRoom;
        private int health;

        public Enemy(Player p, Vector2 position, int width, int height, int rotation, Texture2D sprite,  Room room, float moveSpeed) : base(position, width, height, rotation, sprite, room, moveSpeed) {
            this.player = p;
            this.currentRoom = room;
        }

        public override void UpdatePosition(GameTime gameTime)
        {
        //    Console.WriteLine(player.Position.Y + " " + this.Position.Y) ;
            if (player.Position.Y > this.Position.Y) // If the player is below the enemy, the enenmy will move down
            {
                UpdateDirection(new Vector2(0, 1));
                base.UpdatePosition(gameTime);
            }
            if(player.Position.Y < this.Position.Y) // If the player is above the enemy, the enenmy will move up
            {
                UpdateDirection(new Vector2(0, -1));
                base.UpdatePosition(gameTime);
            }

            if (player.Position.X > this.Position.X) // If the player is to the right, the enemy will move right
            {
                UpdateDirection(new Vector2(1, 0));
                base.UpdatePosition(gameTime);
            }
            if (player.Position.X < this.Position.X) // If the player is to the left, the enemy will move left
            {
                UpdateDirection(new Vector2(-1, 0));
                base.UpdatePosition(gameTime);
            }
            
        }


        public Player setFollowPlayer(Player p)
        {
            this.player = p;
            return player;
        }


        public void OnHit(int damgage)
        {

        }

    }
}
