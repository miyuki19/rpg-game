using rpg_game.DTOs.Character;
using rpg_game.DTOs.Weapon;

namespace rpg_game.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<CharacterResponseDTO>> AddWeapon(WeaponAddingDTO newWeapon);
    }
}