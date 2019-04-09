using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoyT.AStar;

namespace LifeSupport.GameObjects
{
  public  class AlienDog : Enemy
    {

        private int health;

        public AlienDog(Player p, Vector2 position, Room room, float speed) : base(p, position, 30, 30, 0, Assets.Instance.alienDog, room, speed) {
          
        }

    }
}
