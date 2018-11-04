using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SignalRServer.API.Security
{
    public class JwtFactory
    {
        public string GenerateEncodedToken(string username)
        {

            var utcNow = DateTime.UtcNow;
            var symmetricKeyAsBase64 = "this is a custom Secret key for authnetication, Hell YEAH!!!";
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var claims = new[]
              {
                 new Claim(JwtRegisteredClaimNames.Sub, username)
             };
            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.Add(TimeSpan.FromMinutes(1)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}