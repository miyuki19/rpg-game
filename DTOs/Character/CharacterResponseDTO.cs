using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_game.DTOs.Skill;
using rpg_game.DTOs.Weapon;

namespace rpg_game.DTOs.Character
{
    public class CharacterResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "MoChiLy";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
        public WeaponGettingDTO? Weapon { get; set; }
        public List<SkillGettingDTO>? Skills { get; set; }
    }
}