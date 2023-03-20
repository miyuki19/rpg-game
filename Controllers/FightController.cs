using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}