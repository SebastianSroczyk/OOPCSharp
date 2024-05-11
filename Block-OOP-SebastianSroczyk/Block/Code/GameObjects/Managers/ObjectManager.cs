using Block.Code.GameObjects.Inventory;
using Block.Code.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Block.Code.GameObjects.Enemy;
using System.Xml.Serialization;

namespace Block.Code.GameObjects.Managers
{
    public class ObjectManager
    {
        private GameStateManager stateManager;
        private MonsterGen monsterGen;


        private List<GameObject> monsterObjectPool = new List<GameObject>();
        private List<GameObject> potionObjectPool = new List<GameObject>();
        private List<GameObject> weaponObjectPool = new List<GameObject>();

        private List<Vector2> monsterPoolPos = new List<Vector2>();
        private List<Vector2> potionPoolPos = new List<Vector2>();
        private List<Vector2> weaponPoolPos = new List<Vector2>();

        public List<GameObject> MonsterObjectPool { get { return monsterObjectPool; } set { monsterObjectPool = value; } }
        public List<GameObject> PotionObjectPool { get { return potionObjectPool; } set { potionObjectPool = value; } }
        public List<GameObject> WeaponObjectPool { get { return weaponObjectPool; } set { weaponObjectPool = value; } }
        
        public List<Vector2> MonsterPoolPos { get { return monsterPoolPos; } private set { monsterPoolPos = value; } }
        public List<Vector2> PotionPoolPos { get { return potionPoolPos; } private set { potionPoolPos = value; } }
        public List<Vector2> WeaponPoolPos { get { return weaponPoolPos; } private set { weaponPoolPos = value; } }


        



    }


    

    
}
