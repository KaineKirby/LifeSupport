﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config ;
using LifeSupport.Levels;
using LifeSupport.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*
* Player Class (Singleton)
* This is the class that represents the player
*/

namespace LifeSupport.GameObjects {

    class Player : Actor {

        //the controller instance since the player will be manipulated with controls
        private readonly Controller controller ;

    //    public Texture2D playerProjectileTexture;
   //     public float playerProjectileDelay;
        public List<PlayerProjectiles> playerProjectilesList;
        public Vector2 playerPosition;
     //   public float bulletDelay = 1000;
        public float bulletTimeSeconds = 0f;
        public float bulletTimeMilliSeconds = 0f;
        public Boolean firstShot = true;
        public PlayerProjectiles newProjectile = new PlayerProjectiles();
        public Texture2D projectileTexture;
        

        //will probably be constant
        public Player(Game game, Room startingRoom) : base(100, 100, 32, 32, 0, "img/player/player", game, startingRoom, 200f) {

            this.controller = Controller.Instance ;

        //    playerProjectileDelay = bulletDelay;
            playerPosition = new Vector2(XPos, YPos);
            playerProjectilesList = new List<PlayerProjectiles>();
            projectileTexture = newProjectile.setSprite(game, "square");
        }




        public void DrawPlayerProjectiles(SpriteBatch spriteBatch)
        {
            foreach (PlayerProjectiles pp in playerProjectilesList)
                pp.DrawPlayerProjectile(spriteBatch);
        }
        
        //use the controller class to update the positions
        public new void UpdatePosition(GameTime gameTime) {

          
            MouseState mouseState = Mouse.GetState();


            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                
                Shoot(gameTime);
            }

            UpdateProjectiles();

            //on the various vectors
            if (controller.IsKeyDown(controller.MoveUp) && controller.IsKeyDown(controller.MoveRight)) {
                UpdateDirection(new Vector2(1, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveUp) && controller.IsKeyDown(controller.MoveLeft)) {
                UpdateDirection(new Vector2(-1, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveDown) && controller.IsKeyDown(controller.MoveRight)) {
                UpdateDirection(new Vector2(1, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveDown) && controller.IsKeyDown(controller.MoveLeft)) {
                UpdateDirection(new Vector2(-1, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveUp)) {
                UpdateDirection(new Vector2(0, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveDown)) {
                UpdateDirection(new Vector2(0, 1)) ;
                base.UpdatePosition(gameTime) ;
            }  
            else if (controller.IsKeyDown(controller.MoveLeft)) {
                UpdateDirection(new Vector2(-1, 0)) ;
                base.UpdatePosition(gameTime) ;
            }  
            else if (controller.IsKeyDown(controller.MoveRight)) {
                UpdateDirection(new Vector2(1, 0)) ;
                base.UpdatePosition(gameTime) ;
            }

        }


        public void Shoot(GameTime gameTime)
        {
            //   if (bulletTime >= 0)
            //     playerProjectileDelay--;



            bulletTimeSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            bulletTimeMilliSeconds += (float)gameTime.ElapsedGameTime.Milliseconds;


            if (bulletTimeSeconds >= 1f /2 || firstShot == true) {
                firstShot = false;
                PlayerProjectiles newProjectile = new PlayerProjectiles();
                  newProjectile.sprite = projectileTexture;
                  newProjectile.projectilePosition = new Vector2(base.XPos, base.YPos);

                  newProjectile.isVisible = true;

                 if (playerProjectilesList.Count() < 6) { 
                    playerProjectilesList.Add(newProjectile);
                 }
                }

            if (bulletTimeSeconds >= 1f /2)
            {
                bulletTimeMilliSeconds -= bulletTimeMilliSeconds;
                bulletTimeSeconds -= bulletTimeSeconds;
            }


            //     if (playerProjectileDelay == 0)
            //         playerProjectileDelay = bulletDelay;
        }



        public void UpdateProjectiles()
        {
            foreach (PlayerProjectiles pp in playerProjectilesList)
            {
                pp.projectilePosition.Y = pp.projectilePosition.Y - pp.projectileSpeed;

                if (pp.projectilePosition.Y <= 0)
                    pp.isVisible = false;
            }

            for(int i = 0; i < playerProjectilesList.Count; i++)
            {
                if(!playerProjectilesList[i].isVisible)
                {
                    playerProjectilesList.RemoveAt(i);
                    i--;
                }
            }

        }





    }

}

