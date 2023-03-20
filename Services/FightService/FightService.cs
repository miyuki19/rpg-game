using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_game.DTOs.Fight;

namespace rpg_game.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        public FightService(DataContext context)
        {
            this._context = context;
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

                int damage = skill.Damage +
                                    new Random().Next(attacker.Intelligence) -
                                    new Random().Next(opponent.Defeats);
                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }
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

                int damage = attacker.Weapon.Damage +
                                    new Random().Next(attacker.Strength) -
                                    new Random().Next(opponent.Defeats);
                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }
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
    }
}