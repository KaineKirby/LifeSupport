using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Augments;
using LifeSupport.Config ;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

/*
* Player Class (Singleton)
* This is the class that represents the player
*/

namespace LifeSupport.GameObjects {

   public class Player : Actor {
        //the controller instance since the player will be manipulated with controls
        private readonly Controller controller;

        private static readonly float startPlayerSpeed = 500f ;
        public static readonly float FloorTimer = 600f ;

        //animation stuff
        private Texture2D playerLegs ;
        private int animFrame ; //the current frame of animation
        private float timer ; //the time between animation frames
        private float time ; //the current time since last animation frame
        private float legRotation ;
        private Vector2 legOrigin ;

        private static float InvincibleMaxTime = 1.5f ;
        private float InvincibleTime ;

        //the money the player has
        public int Money ;
        //whether or not the player has the card to unlock the challenge room
        public bool HasCard ;

        //An array of augmentations that the player current holds
        public List<Augmentation> Augments ;

        //the time the player has left before they die (run out of O2)
        public float OxygenTime ;

        //the player's flashlight
        private Spotlight light ;


        //will probably be constant
        public Player(PenumbraComponent penumdra) : base(new Vector2(100, 100), penumdra, 32, 32, 0, Assets.Instance.player, null, startPlayerSpeed, 3f, 1f, 1000f, 1000f, 1f) {

            this.controller = Controller.Instance;
            this.GunBarrelPosition = new Point(960, 540) ;

            //animation
            this.animFrame = 0 ;
            this.timer = .05f ;
            this.time = 0;
            this.playerLegs = Assets.Instance.playerLegs ;
            this.legOrigin = new Vector2(Width/2, Height/2) ;
            this.legRotation = 0 ;

            this.InvincibleTime = 0f ;

            this.Money = 0 ;
            this.HasCard = false ;
            this.OxygenTime = FloorTimer ;

            this.Augments = new List<Augmentation>(8);
            for (int i = 0; i < Augments.Capacity; i++)
            {
                Augments.Add(null);
            }

            this.light = new Spotlight {
                Position = this.Position,
                Intensity = 2f,
                Rotation = this.Rotation,
                Scale = new Vector2(2000f),
                ShadowType = ShadowType.Occluded
            };

            penumbra.Lights.Add(light) ;

            AddAugment(AugmentationStation.GenerateAugment(10), 0) ;

        }


        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(playerLegs, Position, new Rectangle(animFrame*Width, 0, Width, Height), Color.White, legRotation, legOrigin, 1f, SpriteEffects.None, 0);
            base.Draw(spriteBatch) ;
        }

        //use the controller class to update the positions
        public override void UpdatePosition(GameTime gameTime) {

            if (Cursor.Instance.IsLeftMouseDown())
                Shoot(Cursor.Instance.GetDirection(GunBarrelPosition), Assets.Instance.playerShot) ;

            //on the various vectors
            if (controller.IsMovingUp() && controller.IsMovingRight()) {
                UpdateDirection(new Vector2(1, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingUp() && controller.IsMovingLeft()) {
                UpdateDirection(new Vector2(-1, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingDown() && controller.IsMovingRight()) {
                UpdateDirection(new Vector2(1, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingDown() && controller.IsMovingLeft()) {
                UpdateDirection(new Vector2(-1, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingUp()) {
                UpdateDirection(new Vector2(0, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingDown()) {
                UpdateDirection(new Vector2(0, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingLeft()) {
                UpdateDirection(new Vector2(-1, 0)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingRight()) {
                UpdateDirection(new Vector2(1, 0)) ;
                base.UpdatePosition(gameTime) ;
            }
            //if not make the player not move by giving a zero vector
            else {
                UpdateDirection(new Vector2(0, 0)) ;
                base.UpdatePosition(gameTime) ;
            }

            //for animation
            if (!MoveDirection.Equals(Vector2.Zero)) {
                this.time += (float)gameTime.ElapsedGameTime.TotalSeconds ;
                this.legRotation = (float)(Math.Atan(MoveDirection.Y/MoveDirection.X)) ;
            }
            else {
                animFrame = 8 ;
            }

            //for I frames
            if (InvincibleTime > 0f)
                InvincibleTime -= (float)gameTime.ElapsedGameTime.TotalSeconds ;
                
            //timer between frames
            if (time >= timer) {
                time = 0 ;
                animFrame = (animFrame+1)%(playerLegs.Width/(Width)) ;
            }

            //rotating the player relative to the mouse
            Vector2 mouseDir = Cursor.Instance.GetDirection(new Point(960, 540)) ;
            this.Rotation = (float)(Math.Atan(mouseDir.Y/mouseDir.X)) ;
            if (mouseDir.X < 0f)
                this.Rotation -= (float)Math.PI ;

            //for updating the oxygen timer
            this.OxygenTime -= (float)gameTime.ElapsedGameTime.TotalSeconds ;
            if (this.OxygenTime <= 0)
                OnDeath() ;

            light.Rotation = this.Rotation ;
            light.Position = this.Position ;

        }

        public override void OnHit(Projectile proj) {
            //if the player is able to get hit again
            if (InvincibleTime <= 0f) {
                InvincibleTime = InvincibleMaxTime ;
                Console.WriteLine(this + " hit for " + proj.Damage + " damage") ;
                this.Health -= proj.Damage ;
                Assets.Instance.playerHit.Play((float)Settings.Instance.SfxVolume/100, 0f, 0f) ;
                //kill it if its health is below 0
                if (this.Health <= 0) {
                    //display a game over somehow
                    OnDeath() ;
                }
            }
        }

        //when the player dies
        public void OnDeath() {
            Console.WriteLine("Player Died") ;
        }

        //remove an augment from the augmentations list and apply changes
        public void RemoveAugment(Augmentation augment) {
            this.Augments.Remove(augment) ;
            ResetStats() ;
            UpdateStats() ;
        }

        //add an augment to the augmentations list and apply changes
        public void AddAugment(Augmentation augment, int spot)
        {
            if (spot < 0 || spot > 8)
            {
                return;
            }
            else
            {
                this.Augments.Insert(spot, augment);
                augment.index = spot;
                //         augment.position = new Vector2(1200, 250);
                ResetStats();
                UpdateStats();
            }
        }


        public int SearchForNextAvailableSpot()
        {
            int spot;
            int i = 0;
            if (Augments[i] == null)
            {
                spot = 0;
                return spot;
            }
            else
            {
                while (this.Augments[i] != null && i < Augments.Count)
                {
                    i++;
                }
                if (i < 8)
                {
                    spot = i;
                    return spot;
                }
                else
                {
                    return Augments.Capacity;
                }
            }
        }

        //reset the stats of the player to their base so we can apply augments
        private void ResetStats() {
            this.Damage = 1f ;
            this.Range = 1000f ;
            this.ShotSpeed = 1000f ;
            this.RateOfFire = 1f ;
            this.MoveSpeed = 500f ;
        }

        //update the stats based on the master augment
        private void UpdateStats()
        {
            foreach (Augmentation a in Augments)
            {
                if (a != null)
                {
                    this.Damage += a.Damage;
                    this.Range += this.Range * a.Range;
                    this.ShotSpeed += this.ShotSpeed * a.ShotSpeed;
                    this.RateOfFire *= (1-a.RateOfFire);
                    this.MoveSpeed += this.MoveSpeed * a.MoveSpeed;
                }
            }
        }
    }
}
