namespace rpg_game.DTOs.Weapon
{
    public class WeaponAddingDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
        public int CharacterId { get; set; }
    }
}