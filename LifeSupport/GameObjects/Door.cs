using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.GameObjects {
    interface Door {

        //we require doors to be able to open and close
        void OpenDoor() ;
        void CloseDoor() ;

    }
}
