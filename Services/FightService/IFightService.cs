using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_game.DTOs.Fight;

namespace rpg_game.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDTO>> WeaponAttack(WeaponAttackDTO attack);
        Task<ServiceResponse<AttackResultDTO>> SkillAttack(SkillAttackDTO attack);
        Task<ServiceResponse<AutoFightResultDTO>> AutoFight(AutoFightRequestDTO autoFight);
        Task<ServiceResponse<List<HighScoreDTO>>> GetHighScore();
    }
}