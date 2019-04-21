﻿using LifeSupport.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.Augments {

    public class Augmentation {

        //all of the statistical increases/decreases
        /*
         * A negative is a statistical decrease
         * A positive is a statistical increase
         * 
         * Different stats require different applications, but the is handled in the player class
         * 
         */ 

        //statistics 
        public float Damage ;
        public float Range ;
        public float ShotSpeed ;
        public float RateOfFire ;
        public float MoveSpeed ;

        public Augmentation(float damage, float range, float shotSpeed, float rateOfFire, float moveSpeed) {
            this.Damage = damage ;
            this.Range = range ;
            this.ShotSpeed = shotSpeed ;
            this.RateOfFire = rateOfFire ;
            this.MoveSpeed = moveSpeed ;
        }

        //C# allows the overriding of binary operators like addition and subtraction which will be handy here
        public static Augmentation operator+ (Augmentation a1, Augmentation a2) {
            return new Augmentation(a1.Damage+a2.Damage, a1.Range+a2.Range, a1.ShotSpeed+a2.ShotSpeed, a1.RateOfFire+a2.RateOfFire, a1.MoveSpeed+a2.MoveSpeed) ;
        }
        public static Augmentation operator- (Augmentation a1, Augmentation a2) {
            return new Augmentation(a1.Damage-a2.Damage, a1.Range-a2.Range, a1.ShotSpeed-a2.ShotSpeed, a1.RateOfFire-a2.RateOfFire, a1.MoveSpeed-a2.MoveSpeed) ;
        }

    }
}
