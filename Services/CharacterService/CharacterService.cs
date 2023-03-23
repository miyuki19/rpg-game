using System.Security.Claims;
using rpg_game.DTOs.Character;

namespace rpg_game.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._mapper = mapper;
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.
            FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string GetUserRole() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role)!;

        private bool isAdmin()
        {
            return GetUserRole().Equals("Admin");
        }

        public async Task<ServiceResponse<List<CharacterResponseDTO>>> AddCharacter(CharacterRequestDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<CharacterResponseDTO>>();
            int userId = GetUserId();
            var character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters
                .Where(c => c.User!.Id == userId)
                .Select(c => _mapper.Map<CharacterResponseDTO>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterResponseDTO>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<CharacterResponseDTO>>();
            var dbCharacters =
                isAdmin() ?
                    await _context.Characters
                        .Include(c => c.Weapon)
                        .Include(c => c.Skills)
                        .ToListAsync() :
                    await _context.Characters
                        .Include(c => c.Weapon)
                        .Include(c => c.Skills)
                        .Where(c => c.User!.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<CharacterResponseDTO>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<CharacterResponseDTO>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<CharacterResponseDTO>();
            var character = isAdmin() ?
                    await _context.Characters
                        .Include(c => c.Weapon)
                        .Include(c => c.Skills)
                        .FirstOrDefaultAsync(c => c.Id == id) :
                    await _context.Characters
                        .Include(c => c.Weapon)
                        .Include(c => c.Skills)
                        .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<CharacterResponseDTO>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterResponseDTO>>> RemoveCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<CharacterResponseDTO>>();

            try
            {
                int userId = GetUserId();
                var character = isAdmin() ?
                    await _context.Characters.FirstOrDefaultAsync(c => c.Id == id) :
                    await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == userId);
                if (character is null)
                {
                    throw new Exception($"Character with Id '{id}' not found");
                }

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters
                    .Where(c => c.User!.Id == userId)
                    .Select(c => _mapper.Map<CharacterResponseDTO>(c)).ToListAsync();
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
                var character = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (character is null || character.User!.Id != GetUserId())
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

        public async Task<ServiceResponse<CharacterResponseDTO>> AddCharacterSkill(CharacterSkillAddingDTO newCharacterSkill)
        {
            var serviceResponse = new ServiceResponse<CharacterResponseDTO>();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
                        c.User!.Id == GetUserId());
                if (character is null)
                {
                    throw new Exception("Character not found");
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (character is null)
                {
                    throw new Exception("Skill not found");
                }

                character.Skills!.Add(skill!);
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