using System;
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

    class Player : Actor
    {

        //the controller instance since the player will be manipulated with controls
        private readonly Controller controller;


        /* All of the following variables are used to set up the projectiles. 
           First a list of projectiles of created.
           Second, a projectile is created called newProjectile to set up a texture within the texture.
           Third, a texture is defined to get the projectile sprite.
           Fourth, the shooting time tracker keeps time within the game (always running) and resets when a shot is made.
           Fifth, the reload time between shots is to prevent from all projectiles from shooting at once (using time)*/
        public List<PlayerProjectiles> playerProjectilesList;
        public PlayerProjectiles newProjectile = new PlayerProjectiles();
        public Texture2D projectileTexture;
        public float shootingTimeTracker = 0f;
        public float reloadTimeBetweenShots = .5f;


        /* The player object position (x,y) */
        public Vector2 PlayerPosition;

        /* A mouse object is instantiated using the MouseControl.cs class */
        public MouseControl MouseCursor;

        /* A room objects is set up to help with collision detection between player projectiles and other game objects */
        private Room Room;




        //constructor
        public Player(Game game, Room startingRoom) : base(100, 100, 32, 32, 0, "img/player/player", game, startingRoom, 200f)
        {

            this.controller = Controller.Instance;

            playerProjectilesList = new List<PlayerProjectiles>();
            projectileTexture = newProjectile.setSprite(game, "square");

            PlayerPosition = new Vector2(XPos, YPos);
            MouseCursor = new MouseControl(game);
            this.Room = startingRoom;
        }


        /* Call this method in MainGame.cs. This function draws the bullets as they are fired 
         It also calls DrawPlayerProjectile(spriteBatch), which is a method within PlayerProjectile.cs
         That method draws one bullet.*/
        public void DrawAllPlayerProjectiles(SpriteBatch spriteBatch)
        {
            foreach (PlayerProjectiles bullet in playerProjectilesList)
                bullet.DrawPlayerProjectile(spriteBatch);
        }

        //use the controller class to update the positions
        public new void UpdatePosition(GameTime gameTime)
        {
            /*Update the player position and retrieve the (x,y) coordinate through a vector */
            PlayerPosition = new Vector2(base.XPos, base.YPos);

            /*Call shoot */
            Shoot(gameTime);

            /*Call Update projectiles */
            UpdateProjectiles(gameTime);
        

            //on the various vectors
            if (controller.IsKeyDown(controller.MoveUp) && controller.IsKeyDown(controller.MoveRight))
            {
                UpdateDirection(new Vector2(1, -1));
                base.UpdatePosition(gameTime);
            }
            else if (controller.IsKeyDown(controller.MoveUp) && controller.IsKeyDown(controller.MoveLeft))
            {
                UpdateDirection(new Vector2(-1, -1));
                base.UpdatePosition(gameTime);
            }
            else if (controller.IsKeyDown(controller.MoveDown) && controller.IsKeyDown(controller.MoveRight))
            {
                UpdateDirection(new Vector2(1, 1));
                base.UpdatePosition(gameTime);
            }
            else if (controller.IsKeyDown(controller.MoveDown) && controller.IsKeyDown(controller.MoveLeft))
            {
                UpdateDirection(new Vector2(-1, 1));
                base.UpdatePosition(gameTime);
            }
            else if (controller.IsKeyDown(controller.MoveUp))
            {
                UpdateDirection(new Vector2(0, -1));
                base.UpdatePosition(gameTime);
            }
            else if (controller.IsKeyDown(controller.MoveDown))
            {
                UpdateDirection(new Vector2(0, 1));
                base.UpdatePosition(gameTime);
            }
            else if (controller.IsKeyDown(controller.MoveLeft))
            {
                UpdateDirection(new Vector2(-1, 0));
                base.UpdatePosition(gameTime);
            }
            else if (controller.IsKeyDown(controller.MoveRight))
            {
                UpdateDirection(new Vector2(1, 0));
                base.UpdatePosition(gameTime);
            }

        }



        /*Shoot is a function which does the following:
         * 1: Create a mouse state object to get the coordinates of the mouse (built in monogame)
         * 2: Update the MouseCursor object (instantiated in Player.cs) using Update (a function defined in MouseControl.cs)
         * 3: Continnualy add the total time since the last shot was made (this is reset when the player left clicks)
         * 4: If the player clicks (or holds down the left mouse key),  and half a second passed since the last shot then..
         *      5:Create a new projectile object (PlayerProjectile.cs)
         *      6:Assign that new projectile with a texture (defined in the constructor)
         *      7:Assign the starting position of the projectile to the current location of the player
         *      8:Assign the projectile to visible
         *      9:Assign projectile's direction (using a function defined in Player.cs)
         *      10: If there are less than 6 player projectiles on screen, shoot another (maximum of 6 per window currently)
         *      11:Reset the shootingTimeTracker to prevent a rapid shooting rate
         *      */
        public void Shoot(GameTime gameTime)
        {

            MouseState mouseState = Mouse.GetState();
            MouseCursor.Update(gameTime);
            shootingTimeTracker += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (mouseState.LeftButton == ButtonState.Pressed) {

                if (shootingTimeTracker >= reloadTimeBetweenShots){
                    PlayerProjectiles newProjectile = new PlayerProjectiles();
                    newProjectile.sprite = projectileTexture;
                    newProjectile.ProjectilePosition = new Vector2(base.XPos, base.YPos);
                    newProjectile.isVisible = true;
                    newProjectile.ProjectileDirection = getProjectileDirection();

                    if (playerProjectilesList.Count() < 6) {
                        playerProjectilesList.Add(newProjectile);
                    }
                }

                if (shootingTimeTracker >= reloadTimeBetweenShots){
                    shootingTimeTracker -= shootingTimeTracker;
                }
            }
        }


        /*The primary function of UpdateProjectiles is to update the position of the bulllet fired after shooting it, checking to see if the bullet
         * collides with the edge of the screen or an object, and to remove the bullet from the list if it does collide. */

        public void UpdateProjectiles(GameTime gameTime)
        {

            foreach (PlayerProjectiles bullet in playerProjectilesList) {

                /*Update the position of the current bullet fired */
                bullet.ProjectilePosition.X += bullet.ProjectileDirection.X * bullet.projectileSpeed ;
                bullet.ProjectilePosition.Y += bullet.ProjectileDirection.Y * bullet.projectileSpeed;

                /* Check for collisions with game window */
                bullet.collisionBox = new Rectangle((int)bullet.ProjectilePosition.X, (int)bullet.ProjectilePosition.Y, ((int)projectileTexture.Width * (int)bullet.scale), ((int)projectileTexture.Height * (int)bullet.scale));
                if (bullet.collisionBox.Y <= 0 || bullet.collisionBox.Y >= Settings.Instance.Height || bullet.collisionBox.X <= 0 || bullet.collisionBox.X >= Settings.Instance.Width){
                    bullet.isVisible = false;
                }

                /* Check for collisions with game objects */
                foreach (GameObject obj in Room.Objects) {
                    if (bullet.collisionBox.X < obj.XPos + obj.Width && bullet.collisionBox.X > obj.XPos &&
                       bullet.collisionBox.Y < obj.YPos + obj.Height && bullet.collisionBox.Y  > obj.YPos){
                        bullet.isVisible = false;
                    }
                }

            }

            /* If there was a collision, remove the bullet from the player projectiles list */
            for (int i = 0; i < playerProjectilesList.Count; i++) {
                if (!playerProjectilesList[i].isVisible) {
                    playerProjectilesList.RemoveAt(i);
                    i--;
                }
            }

        }


        /* This function returns a vector that calculates the direction of each individual bullet within the player projectile list. It is assigned to each
         * projectile's direction field in the shoot function. */
        public Vector2 getProjectileDirection()
        {
            Vector2 BulletDirection;

            /* The bullet direction is the center of the crosshair subtracted by the current player position */
            BulletDirection = (MouseCursor.MousePosition  + (new Vector2(MouseCursor.mouseImageCenterX, MouseCursor.mouseImageCenterY))) - PlayerPosition;
            BulletDirection.Normalize();
            if(BulletDirection != Vector2.Zero){
                BulletDirection.Normalize();
                return BulletDirection;
            }
            return BulletDirection;

        }




    }
}

