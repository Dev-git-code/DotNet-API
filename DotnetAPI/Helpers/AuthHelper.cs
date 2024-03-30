    using System.Data;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using Dapper;
    using DotNetAPI.Data;
    using DotNetAPI.Dtos;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using Microsoft.Data.SqlClient;
    using Microsoft.IdentityModel.Tokens;

    namespace DotNetAPI.Helpers
    {
        public class AuthHelper
        {
            private readonly IConfiguration _config;
            private readonly DataContextDapper _dapper;
            public AuthHelper(IConfiguration config)
            {
                _dapper = new DataContextDapper(config);
                _config = config;
            }
            public byte[] GetPasswordHash(string password, byte[] passwordSalt)
            {

                string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                                                    Convert.ToBase64String(passwordSalt);

                return KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 1000000,
                    numBytesRequested: 256 / 8
                );
            }

            public string CreateToken(int userId)
            {
                Claim[] claims = new Claim[]{
                    new Claim("userId",userId.ToString())
                };

                string? tokenKeyString = _config.GetSection("Appsettings:TokenKey").Value;

                SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                                                        tokenKeyString != null ? tokenKeyString : ""));

                SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = credentials,
                    Expires = DateTime.Now.AddDays(1)
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken token = tokenHandler.CreateToken(descriptor);
                return tokenHandler.WriteToken(token);
            }

            public bool SetPassword(UserForLoginDto userForSetPassword)
            {
                byte[] passwordSalt = new byte[128 / 8];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetNonZeroBytes(passwordSalt);
                }

                byte[] passwordHash = GetPasswordHash(userForSetPassword.Password, passwordSalt);

                string sqlAddAuth = @"EXEC TutorialAppSchema.spRegistration_Upsert
                                            @Email = @EmailParameter, 
                                            @PasswordHash = @PasswordHashParameter, 
                                            @PasswordSalt = @PasswordSaltParameter";

                // List<SqlParameter> sqlParameters = new List<SqlParameter>();
                // SqlParameter emailParameter = new SqlParameter("@EmailParameter", SqlDbType.NVarChar)
                // { Value = userForSetPassword.Email };
                // sqlParameters.Add(emailParameter);

                // using DynamicParameters instead of the above list of SqlParameter
                DynamicParameters sqlParameters = new DynamicParameters();
                sqlParameters.Add("@EmailParameter", userForSetPassword.Email, DbType.String);
                sqlParameters.Add("@PasswordHashParameter", passwordHash, DbType.Binary);
                sqlParameters.Add("@PasswordSaltParameter", passwordSalt, DbType.Binary);

                return _dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters);

            }
        }

    }