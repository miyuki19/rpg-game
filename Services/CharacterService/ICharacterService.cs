using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_game.DTOs.Character;

namespace rpg_game.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<CharacterResponseDTO>>> GetAllCharacters();
        Task<ServiceResponse<CharacterResponseDTO>> GetCharacterById(int id);
        Task<ServiceResponse<List<CharacterResponseDTO>>> AddCharacter(CharacterRequestDTO newCharacter);
        Task<ServiceResponse<CharacterResponseDTO>> UpdateCharacter(int id, CharacterRequestDTO updatedCharacter);
        Task<ServiceResponse<List<CharacterResponseDTO>>> RemoveCharacter(int id);
        Task<ServiceResponse<CharacterResponseDTO>> AddCharacterSkill(CharacterSkillAddingDTO newCharacterSkill);
    }
}