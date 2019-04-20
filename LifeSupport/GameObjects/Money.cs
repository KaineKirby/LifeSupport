using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.GameObjects {
    class Money : Drop {

        private int amount ;

        public Money(Vector2 position, Player player, Room room, int amount) : base(position, 30, 30, Assets.Instance.money, player, room) {
            this.amount = amount ;
        }

        public override void OnPickup() {
            player.Money += amount ;
            
            Console.WriteLine("Player picked up money: " + player.Money) ;

            base.OnPickup() ;
        }
    }
}
