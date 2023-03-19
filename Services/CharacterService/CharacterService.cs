using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using rpg_game.DTOs.Character;

namespace rpg_game.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            this._mapper = mapper;
            this._context = context;
        }

        public async Task<ServiceResponse<List<CharacterResponseDTO>>> AddCharacter(CharacterRequestDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<CharacterResponseDTO>>();
            var character = _mapper.Map<Character>(newCharacter);

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<CharacterResponseDTO>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterResponseDTO>>> GetAllCharacters(int userId, string userRole)
        {
            var serviceResponse = new ServiceResponse<List<CharacterResponseDTO>>();
            var dbCharacters =
                userRole.Equals("Admin") ?
                await _context.Characters.ToListAsync() :
                await _context.Characters.Where(c => c.User!.Id == userId).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<CharacterResponseDTO>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<CharacterResponseDTO>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<CharacterResponseDTO>();
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<CharacterResponseDTO>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterResponseDTO>>> RemoveCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<CharacterResponseDTO>>();

            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (character is null)
                {
                    throw new Exception($"Character with Id '{id}' not found");
                }

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<CharacterResponseDTO>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<CharacterResponseDTO>> UpdateCharacter(int id, CharacterRequestDTO updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<CharacterResponseDTO>();

            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (character is null)
                {
                    throw new Exception($"Character with Id '{id}' not found");
                }

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                //_context.Characters.Update(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<CharacterResponseDTO>(character);

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}