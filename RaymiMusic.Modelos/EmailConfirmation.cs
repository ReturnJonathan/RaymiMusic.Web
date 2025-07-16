namespace RaymiMusic.Modelos
{
    public class EmailConfirmation
    {
        public int Id { get; set; }
        public Guid UsuarioId { get; set; }           // antes era int
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public bool IsConfirmed { get; set; }
        public ConfirmationPurpose Purpose { get; set; } // nuevo
    }
}
