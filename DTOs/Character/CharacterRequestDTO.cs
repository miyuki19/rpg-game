namespace rpg_game.DTOs.Character
{
    public class CharacterRequestDTO
    {
        public string Name { get; set; } = "MoChiLy";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
    }
}