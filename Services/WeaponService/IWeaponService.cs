using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_game.DTOs.Character;
using rpg_game.DTOs.Weapon;

namespace rpg_game.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<CharacterResponseDTO>> AddWeapon(WeaponAddingDTO newWeapon);
    }
}