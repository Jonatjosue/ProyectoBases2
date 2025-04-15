namespace proyectoMMSBS2.DTOs
{
    public class DtoPrincipales
    {
    }


    public class roleBd {
        public int roleId { get; set; }
        public string? roleNombre { get; set; }
    }
    public class validarLogin {
        public required string Usuario { get; set; }
        public required int idRole { get; set; }
        public required string parametro1 { get; set; }
    }
    public class usuarioJwt { 
        public required string Username { get; set; }
        public required string Role { get; set;  }
    }
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60;
        public string Issuer { get; set; } = "MyApp";
        public string Audience { get; set; } = "MyUsers";
    }

}
