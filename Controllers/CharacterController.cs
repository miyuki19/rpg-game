using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rpg_game.DTOs.Character;

namespace rpg_game.Controllers
{
    [Authorize(Roles = "Normal, Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _chacterService;

        public CharacterController(ICharacterService chacterService)
        {
            this._chacterService = chacterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<CharacterResponseDTO>>>> Get()
        {
            return Ok(await _chacterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<CharacterResponseDTO>>> GetSingle(int id)
        {
            return Ok(await _chacterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CharacterResponseDTO>>>> AddCharacter(CharacterRequestDTO newCharacter)
        {
            return Ok(await _chacterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<CharacterResponseDTO>>> UpdateCharacter(int id, CharacterRequestDTO updateCharacter)
        {
            return Ok(await _chacterService.UpdateCharacter(id, updateCharacter));
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<List<CharacterResponseDTO>>>> RemoveCharacter(int id)
        {
            return Ok(await _chacterService.RemoveCharacter(id));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<CharacterResponseDTO>>> AddCharacterSkill(CharacterSkillAddingDTO newCharacterSkill)
        {
            return Ok(await _chacterService.AddCharacterSkill(newCharacterSkill));
        }
    }
}