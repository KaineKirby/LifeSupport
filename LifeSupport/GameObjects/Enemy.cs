using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace LifeSupport.GameObjects
{
    public abstract class Enemy : Actor {

        protected Player player;
        protected Room currentRoom;

        // This is the length of each tile
        public const int SquareTileLength = 30;

        // This stores the path from the enemy to the player
        IList<Position> path;


        public Enemy(Player p, Vector2 position, int width, int height, int rotation, Texture2D sprite,  Room room, 
            float moveSpeed, float health, float damage, float range, float shotSpeed, float rateOfFire) : base(position, width, height, rotation, sprite, room, moveSpeed, health, damage, range, shotSpeed, rateOfFire) {
            this.player = p;
            this.currentRoom = room ;

        }
        
        //draw the current pathing path for debugging
        public void DrawPath(SpriteBatch spriteBatch) {
            if (path == null) 
                return ;
            
            for (int i = 0 ; i < path.Count ; i++) {
                spriteBatch.Draw(sprite, new Vector2(CurrentRoom.StartX+path[i].Y*30, CurrentRoom.StartY+path[i].X*30), new Rectangle(0, 0, 32, 32), Color.Black, Rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
        }

        protected virtual void InfluenceDirection(Vector2 direction, GameTime gameTime) {
            this.MoveDirection = ((MoveDirection * (.2f) / (float)gameTime.ElapsedGameTime.TotalSeconds) + direction)/30 ;
            if (!MoveDirection.Equals(Vector2.Zero))
                MoveDirection.Normalize() ;
        }

        public override void UpdatePosition(GameTime gameTime) {

            //use the path finding algorithm by RoyT library to determine the current move direction vector we need, then call base which handles movement for us

            //get the enemy and player positions
            Position enemy = this.GetGridPosition() ;
            Position player = this.player.GetGridPosition() ;

            path = CurrentRoom.gridTiles.GetPath(enemy, player) ;

         
            //the calculated path is too short (next to player)
            if (path.Count < 2) {
                this.MoveDirection = Vector2.Zero ;
                return ;
            }

            //there are 8 scenarios for the next place to go

            //need to move up and to the left
            if (path[0].X > path[1].X && path[0].Y > path[1].Y) {
                InfluenceDirection(new Vector2(-1, -1), gameTime) ;
            }
            //up and to the right
            else if (path[0].X > path[1].X && path[0].Y < path[1].Y) {
                InfluenceDirection(new Vector2(1, -1), gameTime) ;

            }
            //down and to the right
            else if (path[0].X < path[1].X && path[0].Y < path[1].Y) {
                InfluenceDirection(new Vector2(1, 1), gameTime) ;

            }
            //down and to the left
            else if (path[0].X < path[1].X && path[0].Y > path[1].Y) {
                InfluenceDirection(new Vector2(-1, 1), gameTime) ;

            }
            //need to move up
            else if (path[0].X > path[1].X) {
                InfluenceDirection(new Vector2(0, -1), gameTime) ;
            }
            //move down
            else if (path[0].X < path[1].X) {
                InfluenceDirection(new Vector2(0, 1), gameTime) ;
            }
            //move left
            else if (path[0].Y > path[1].Y) {
                InfluenceDirection(new Vector2(-1, 0), gameTime) ;
            }
            //move right
            else if (path[0].Y < path[1].Y) {
                InfluenceDirection(new Vector2(1, 0), gameTime) ;
            }

            base.UpdatePosition(gameTime) ;


        }

        public bool HasLineOfSight() {
            if (path == null)
                return false;
            Position enemy = this.GetGridPosition();
            Position player = this.player.GetGridPosition();
            Grid emptyGrid = new Grid(36, 64);
            IList<Position> straightPath = emptyGrid.GetPath(enemy, player);
            for(int i = 0; i < straightPath.Count && i < path.Count; i++)
            {
                if (straightPath[i] != path[i])
                    return false;
            }
            return true;

        }

    }
}
