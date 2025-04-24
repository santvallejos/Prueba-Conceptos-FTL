using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Pruebas_Conceptos_MVC_FTG
{
    public class JwtAuthService
    {
        // Genera una clave aleatoria de 256 bits (32 bytes)
        private readonly byte[] _key = new byte[32];  // 32 bytes = 256 bits

        public JwtAuthService()
        {
            // Genera la clave utilizando RandomNumberGenerator
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(_key);
            }
        }

        public SymmetricSecurityKey GetSigningKey()
        {
            // Retorna la clave generada para la firma
            return new SymmetricSecurityKey(_key);
        }
    }
}
