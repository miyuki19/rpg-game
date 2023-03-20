using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_game.DTOs.Fight
{
    public class AutoFightRequestDTO
    {
        public List<int> CharacterIds { get; set; } = new List<int>();
    }
}