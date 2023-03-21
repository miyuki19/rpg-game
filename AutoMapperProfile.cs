using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_game.DTOs.Character;
using rpg_game.DTOs.Fight;
using rpg_game.DTOs.Skill;
using rpg_game.DTOs.Weapon;

namespace rpg_game
{
    public class AutoMapperProfile
    {
        public MapperConfiguration Configuration { get; private set; }
        public AutoMapperProfile()
        {
            Configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Character, CharacterResponseDTO>();
                cfg.CreateMap<CharacterRequestDTO, Character>();
                cfg.CreateMap<Weapon, WeaponGettingDTO>();
                cfg.CreateMap<Skill, SkillGettingDTO>();
                cfg.CreateMap<Character, HighScoreDTO>();
            });
        }
    }
}