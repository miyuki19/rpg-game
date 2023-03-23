using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rpg_game.DTOs.Fight;

namespace rpg_game.Controllers
{
    [Authorize(Roles = "Normal, Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;
        public FightController(IFightService fightService)
        {
            this._fightService = fightService;

        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> WeaponAttack(WeaponAttackDTO attack)
        {
            return Ok(await _fightService.WeaponAttack(attack));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> SkillAttack(SkillAttackDTO attack)
        {
            return Ok(await _fightService.SkillAttack(attack));
        }

        [HttpPost("AutoFight")]
        public async Task<ActionResult<ServiceResponse<AutoFightResultDTO>>> AutoFight(AutoFightRequestDTO autoFight)
        {
            return Ok(await _fightService.AutoFight(autoFight));
        }

        [HttpGet("HighScore")]
        public async Task<ActionResult<ServiceResponse<List<HighScoreDTO>>>> GetHighScore()
        {
            return Ok(await _fightService.GetHighScore());
        }
    }
}