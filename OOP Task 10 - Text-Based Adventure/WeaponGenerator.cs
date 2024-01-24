using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class WeaponGenerator
    {
        private string[] itemName = { "Sword", "Stick", "Pan", "Gun", "Cup", "Toilet Paper" };
        private string[] description = { "\nIt does sword things...", "\nIts sticky.", "\nIts a frying Pan", "\nIt shoots", "\nIts just a Cup", "\n What can I say... make sure to wipe..." };
        public WeaponGenerator()
        {

        }
        public Weapon GenerateWeapons()
        {
            Random random = new Random();

            int arrayNumber = random.Next(itemName.Length);

            string Name = itemName[arrayNumber];

            string Description = description[arrayNumber];

            int HitPoints = random.Next(10,20);

            return new Weapon(Name, Description, HitPoints);
        }
    } 
}
