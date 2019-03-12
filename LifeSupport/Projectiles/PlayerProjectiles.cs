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
    public class PlayerProjectiles
    {
        public Rectangle collisionBox;
        public Texture2D sprite;
        public Vector2 projectileOrigin;
        public Vector2 projectilePosition;
        public Vector2 projectileDirection;
        public bool isVisible;
        public float projectileSpeed;
        public float scale = .05f;

        public PlayerProjectiles()
        {
            projectileSpeed = .2f;
            isVisible = false;
        }

        public void DrawPlayerProjectile(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, projectilePosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        

        /*
        public Texture2D getSprite()
        {
            return sprite;
        }
        */
        public Texture2D setSprite(Game game, String SpritePath)
        {
            this.sprite = game.Content.Load<Texture2D>(SpritePath);
            return sprite;
        }
            
        /*
        public float getProjectileSpeed()
        {
            return projectileSpeed;
        }

        public void setProjectileSpeed(Game game,float speed)
        {
            projectileSpeed = speed;
        }
            */
       




    }
}
