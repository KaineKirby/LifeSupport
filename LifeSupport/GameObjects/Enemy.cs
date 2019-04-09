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


        private int health;

        // Begin chasing player if they are close
        private bool playerFound;

        // This is the length of each tile
        public const int SquareTileLength = 30;

        // This stores the path from the enemy to the player
        IList<Position> path;


        public Enemy(Player p, Vector2 position, int width, int height, int rotation, Texture2D sprite,  Room room, float moveSpeed) : base(position, width, height, rotation, sprite, room, moveSpeed) {
            this.player = p;
            this.currentRoom = room;
        }


        public override void UpdatePosition(GameTime gameTime)
        {

            Position currentEnemyPostion = ToPositionFromVector2(this.Position);
            Position begin = new Position(currentEnemyPostion.Y / SquareTileLength, currentEnemyPostion.X / SquareTileLength);

            Position currentPlayerPostion = ToPositionFromVector2(this.player.Position);
            Position end = new Position(currentPlayerPostion.Y / SquareTileLength, currentPlayerPostion.X / SquareTileLength);

            // If the enemy x coord is within 20 tiles of the player x coord (going from left to right)
            // and the enemy x coord is within 15 tiles of the player y coord (going from top to bottom)
            // begin chasing player
            if ( (Math.Abs(end.Y - begin.Y) <= 20 && Math.Abs(end.X - begin.X) <= 15))
            {
                playerFound = true;
            }
            if(playerFound)
            {

                path = currentRoom.gridTiles.GetPath(begin, end);

                for (int i = 0; i < path.Count; i++)
                {
                    Position updatedEnemyPosition = new Position();
                    updatedEnemyPosition = path[i];

                    Position actualEnemyPosition = new Position(updatedEnemyPosition.Y * SquareTileLength, updatedEnemyPosition.X * SquareTileLength);
                    Position enemyDistanceFromLastPosition = actualEnemyPosition - ToPositionFromVector2(this.Position);
                    Vector2 enemyDirection = ToVector2FromPosition(enemyDistanceFromLastPosition);

                    UpdateDirection(enemyDirection);

                    if (!this.hasCollided)
                    {
                        base.UpdatePosition(gameTime);
                    }

                    // Call this if the enemy collides with a wall
                    // This helps the enemy navigate around corners and doors a bit smoother
                    else
                    {

                        // Moving Right-Up (needs to go up)
                        if (this.hasCollided == true && this.MoveDirection.X > 0 && this.MoveDirection.Y < 0)
                        {
                            UpdateDirection(new Vector2(0, -1));
                            base.UpdatePosition(gameTime);


                            // Moving Right-Up (needs to go right)
                            if (this.hasCollided == true)
                            {
                                UpdateDirection(new Vector2(1, 0));
                                base.UpdatePosition(gameTime);
                            }
                        }

                        // Moving Left-Up (needs to go up)
                        if (this.hasCollided == true && this.MoveDirection.X < 0 && this.MoveDirection.Y < 0)
                        {
                            UpdateDirection(new Vector2(0, -1));
                            base.UpdatePosition(gameTime);

                            // Moving Left-Up (needs to go left)
                            if (this.hasCollided == true)
                            {
                                UpdateDirection(new Vector2(-1, 0));
                                base.UpdatePosition(gameTime);
                            }
                        }



                        // Moving Right-Down (needs to go down)
                        if (this.hasCollided == true && this.MoveDirection.X > 0 && this.MoveDirection.Y > 0)
                        {
                            UpdateDirection(new Vector2(0, 1));
                            base.UpdatePosition(gameTime);

                            //  Moving Right-Down (needs to go right)
                            if (this.hasCollided == true)
                            {
                                UpdateDirection(new Vector2(1, 0));
                                base.UpdatePosition(gameTime);
                            }
                        }

                        // Moving Left-Down (needs to go down)
                        if (this.hasCollided == true && this.MoveDirection.X < 0 && this.MoveDirection.Y > 0)
                        {
                            UpdateDirection(new Vector2(0, 1));
                            base.UpdatePosition(gameTime);

                            // Moving Left-Down (needs to go left)
                            if (this.hasCollided == true)
                            {
                                UpdateDirection(new Vector2(-1, 0));
                                base.UpdatePosition(gameTime);
                            }
                        }
                    }

                }
            }
            else { };
        }



        public Vector2 ToVector2FromPoint(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public Vector2 ToVector2FromPosition(Position position)
        {
            return new Vector2(position.X, position.Y);
        }

        public Position ToPositionFromVector2(Vector2 vector)
        {
            return new Position((int)vector.X, (int)vector.Y);
        }

        public Point ToPointFromPosition(Position position)
        {
            return new Point(position.X, position.Y);
        }
            

        public Point ToPointFromVector2(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }



        public void OnHit(int damage)
        {

        }



        /*
        public override void UpdatePosition(GameTime gameTime)
        {

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
            
        }*/


    }
}
