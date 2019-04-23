using LifeSupport.Config;
using LifeSupport.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.Augments {

    public class Augmentation
    {

        //all of the statistical increases/decreases
        /*
         * A negative is a statistical decrease
         * A positive is a statistical increase
         * 
         * Different stats require different applications, but the is handled in the player class
         * 
         */

        public Texture2D augmentImage;
        public int index;
        public Vector2 position;
        public Rectangle rect;

        //statistics 
        public float Damage;
        public float Range;
        public float ShotSpeed;
        public float RateOfFire;
        public float MoveSpeed;

        public Augmentation(float damage, float range, float shotSpeed, float rateOfFire, float moveSpeed) {
            this.augmentImage = Assets.Instance.augmentationLarge ;
            this.rect = new Rectangle((int)position.X, (int)position.Y, augmentImage.Width, augmentImage.Height);
            this.Damage = damage;
            this.Range = range;
            this.ShotSpeed = shotSpeed;
            this.RateOfFire = rateOfFire;
            this.MoveSpeed = moveSpeed;
        }

        //C# allows the overriding of binary operators like addition and subtraction which will be handy here
        public static Augmentation operator +(Augmentation a1, Augmentation a2)
        {
            return new Augmentation(a1.Damage + a2.Damage, a1.Range + a2.Range, a1.ShotSpeed + a2.ShotSpeed, a1.RateOfFire + a2.RateOfFire, a1.MoveSpeed + a2.MoveSpeed);
        }

        public static Augmentation operator -(Augmentation a1, Augmentation a2) {
            return new Augmentation(a1.Damage - a2.Damage, a1.Range - a2.Range, a1.ShotSpeed - a2.ShotSpeed, a1.RateOfFire - a2.RateOfFire, a1.MoveSpeed - a2.MoveSpeed);
        }

        public static Augmentation operator *(int a1, Augmentation a2) {
             return new Augmentation(a1 * a2.Damage, a1 *  a2.Range, a1 * a2.ShotSpeed, a1 * a2.RateOfFire, a1 * a2.MoveSpeed);
        }

        public void DrawWithScale(SpriteBatch spriteBatch, float scale) {
            spriteBatch.Draw(augmentImage, position, rect, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, .05f);
        }

    }
}