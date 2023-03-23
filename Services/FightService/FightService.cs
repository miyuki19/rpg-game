using rpg_game.DTOs.Fight;

namespace rpg_game.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<AutoFightResultDTO>> AutoFight(AutoFightRequestDTO autoFight)
        {
            var serviceResponse = new ServiceResponse<AutoFightResultDTO>
            {
                Data = new AutoFightResultDTO()
            };

            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => autoFight.CharacterIds.Contains(c.Id))
                    .ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        int oIndex = new Random().Next(opponents.Count);
                        var opponent = opponents[oIndex];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon && attacker.Weapon is not null)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else if (!useWeapon && (attacker.Skills is not null && attacker.Skills.Any()))
                        {
                            int index = new Random().Next(attacker.Skills.Count);
                            var skill = attacker.Skills[index];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        else
                        {
                            serviceResponse.Data.Log.Add($"{attacker.Name} wasn't able to attack...");
                            continue;
                        }

                        serviceResponse.Data.Log
                            .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage");

                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            serviceResponse.Data.Log.Add($"{opponent.Name} has been defeated!");
                            serviceResponse.Data.Log.Add($"{attacker.Name} won with {attacker.HitPoints} HP left!");
                            break;
                        }
                    }
                }

                //reset HP for next fight
                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<AttackResultDTO>> SkillAttack(SkillAttackDTO attack)
        {
            var serviceResponse = new ServiceResponse<AttackResultDTO>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == attack.AttackerId);
                var opponent = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == attack.OpponentId);

                if (attacker is null || opponent is null)
                {
                    throw new Exception("Attacker or Opponent not found");
                }
                if (attacker.Skills is null)
                {
                    throw new Exception("No skill, cannot fight...");
                }

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == attack.SkillId);
                if (skill is null)
                {
                    throw new Exception($"{attacker.Name} doesn't know this skill, cannot fight");
                }

                int damage = DoSkillAttack(attacker, opponent, skill);
                if (opponent.HitPoints <= 0)
                {
                    serviceResponse.Message = $"{opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();
                serviceResponse.Data = new AttackResultDTO
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<AttackResultDTO>> WeaponAttack(WeaponAttackDTO attack)
        {
            var serviceResponse = new ServiceResponse<AttackResultDTO>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == attack.AttackerId);
                var opponent = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == attack.OpponentId);

                if (attacker is null || opponent is null)
                {
                    throw new Exception("Attacker or Opponent not found");
                }
                if (attacker.Weapon is null)
                {
                    throw new Exception("No weapon, cannot fight...");
                }

                int damage = DoWeaponAttack(attacker, opponent);
                if (opponent.HitPoints <= 0)
                {
                    serviceResponse.Message = $"{opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();
                serviceResponse.Data = new AttackResultDTO
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            if (attacker.Weapon is null)
            {
                throw new Exception("Attacker has no weapon...");
            }
            int damage = attacker.Weapon.Damage +
                                                new Random().Next(attacker.Strength) -
                                                new Random().Next(opponent.Defeats);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage +
                                new Random().Next(attacker.Intelligence) -
                                new Random().Next(opponent.Defeats);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<List<HighScoreDTO>>> GetHighScore()
        {


            var characters = await _context.Characters
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .ToListAsync();

            var serviceResponse = new ServiceResponse<List<HighScoreDTO>>
            {
                Data = characters.Select(c => _mapper.Map<HighScoreDTO>(c)).ToList()
            };

            return serviceResponse;
        }
    }
}