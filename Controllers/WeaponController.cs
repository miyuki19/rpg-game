using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rpg_game.DTOs.Character;
using rpg_game.DTOs.Weapon;

namespace rpg_game.Controllers
{
    [Authorize(Roles = "Normal, Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;
        public WeaponController(IWeaponService weaponService)
        {
            this._weaponService = weaponService;

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<CharacterResponseDTO>>> AddWeapon(WeaponAddingDTO newWeapon)
        {
            return Ok(await _weaponService.AddWeapon(newWeapon));
        }
    }
}