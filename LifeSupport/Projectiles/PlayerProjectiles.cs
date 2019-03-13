using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LifeSupport.Projectiles
{

    /* This class creates a projectile object. The projectiles will be instantiated within a list in the Player.cs class */
    public class PlayerProjectiles
    {
        public Rectangle collisionBox;
        public Texture2D sprite;
        public Vector2 ProjectilePosition;
        public Vector2 ProjectileDirection;
        public bool isVisible;
        public float projectileSpeed;
        public float scale;

        public PlayerProjectiles()
        {
            projectileSpeed = .2f;
            isVisible = false;
            scale = .01f;
        }

        public void DrawPlayerProjectile(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, ProjectilePosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }


        public Texture2D setSprite(Game game, String SpritePath)
        {
            this.sprite = game.Content.Load<Texture2D>(SpritePath);
            return sprite;
        }

        
            




    }
}
