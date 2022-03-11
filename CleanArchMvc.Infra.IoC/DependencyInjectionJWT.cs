using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMvc.Infra.IoC
{
    public static class DependencyInjectionJWT
    {
        public static IServiceCollection AddInfrastructureJWT(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Informar o tipo de autenticação -= JWT bearer
            //Definir modelo de desafio, jwt bearer
            services.AddAuthentication(opt =>
            {
                //ESquema padrao de authentication
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //Habilita a autenticacao JWT usando esquema e desafios definidos
            //Validar o token
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience =  true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //Valores válidos:
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                    //tempo padrado do ske´w é 5 minutos, entao se eu zerar ele, só fica valando
                    //Os 10 minutos que eu defini no controller
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}
