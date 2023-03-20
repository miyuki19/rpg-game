using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_game.DTOs.Character
{
    public class CharacterSkillAddingDTO
    {
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
    }
}