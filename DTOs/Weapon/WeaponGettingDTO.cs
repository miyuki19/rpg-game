using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_game.DTOs.Weapon
{
    public class WeaponGettingDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
    }
}