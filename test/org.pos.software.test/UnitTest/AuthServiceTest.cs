using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using org.pos.software.Application.Services;
using org.pos.software.Configuration;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Utils;

namespace org.pos.software.test.UnitTest
{
    public class AuthServiceTest
    {

        // Estructura de nombre para metodos de test
        // Convencion: Metodo_Escenario_ResultadoEsperado

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // ARRANGE: Preparacion del entorno

            /*
             crea mocks de ambas dependencias para no depender de la base de datos real
             logra que el entorno sea controlado y rapido
             moq: permite definir que devuelve un metodo de la interfaz cuando se llama con ciertos parametros
             */
            var mockUserRepo = new Mock<IUserRepository>();
            var mockRoleRepo = new Mock<IRoleRepository>();

            // Crea el rol con permisos
            /*
             En AuthService.Login, se crean Claims basados en user.Role.Name y
             user.Role.Permissions. Si esto fuera null, el test fallaria
             
             Consejo: Siempre se debe inicializar todas las propiedades que se usan
             en el metodo bajo prueba

             */
            var role = new Role("PRESUPUESTISTA", new[] { "CREATE", "READ" });

            // Crea el usuario
            /*
             con todas sus propiedades que son obligatorias
             se hace uso del builder para crear un usuario con muchos parametros de forma clara
             Id es necesario para el claim "id" en JWT
             Salt y Hash son usados en la validacion de contraseña (PasswordUtils.VerifyPassword)
             
             Consejo: Siempre asegurarse de que el objeto de prueba tenga todos los campos
             requeridos por el metodo bajo prueba

             */
            var testUser = User.Builder()
                .Id("usr-test-001")
                .Email("gomezdiegoelias1@gmail.com")
                .Dni(12345678)
                .FirstName("Diego")
                .Role(role)
                .Status(Status.ACTIVE)
                .Salt("salt123")
                .Hash(PasswordUtils.HashPasswordWithSalt("password", "salt123"))
                .Build();

            // Configura el comportamiento del mock
            /*
             ¿Que hace?: Dice "cuando alguin llame FindByEmail con ese email, devuelve testUser".
             ¿Por que?: Asi AuthService cree que esta llamando a la base de datos real, pero
             en realidad se esta controlando la respuesta
             
             Buenas practicas:
               Configurar solo lo que el test necesita
               Evitar mocks innecesarios que compliquen el test
             */
            mockUserRepo.Setup(r => r.FindByEmail("gomezdiegoelias1@gmail.com"))
                .ReturnsAsync(testUser);

            // Configura el JWT
            /*
             ¿Por que?: AuthService necesita JwtConfigDto para generar tokens
             Buenas practicas
               No usar secretos reales en test
               Mantener valores consistentes y cortos para pruebas
             */
            var jwtConfig = new JwtConfigDto
            {
                Secret = "SuperClaveSecretaParaTesting1234",
                ExpirationMinutes = 60,
                Issuer = "TestIssuer",
                Audience = "TestAudience"
            };

            // Crea el servicio
            /*
             ¿Que hace?: Instancia el AuthService pasando los mocks y la configuracion
             ¿Por que?: Es el sistema bajo prueba (SUT, System Under Test)
             */
            var service = new AuthService(mockUserRepo.Object, mockRoleRepo.Object, jwtConfig);

            // Prepara la peticion
            /*
             ¿Que hace?: Simula la peticion de login que se enviara desde un cliente
             
             Consejo: Siempre se debe de crear objetos de entrada claros y consistentes
             con lo que espera el metodo
             */
            var loginRequest = new LoginRequest("gomezdiegoelias1@gmail.com", "password");

            // ACTION: Ejecucion de metodos

            // ejecuta el metodo login
            /*
             ¿Que hace?: Llama al metodo que estamos testeando
             
             Buenas practicas
               Solo un Act por test
               Mantener el test enfocado en unico escenario
            */
            var response = await service.Login(loginRequest);

            // ASSERT: Validacion de resultados

            /*
             ¿Que hace?:
              1. Verifica que la respuesta no sea null
              2. Verifica que el AccessToken no este vacio
             ¿Por que?: Estas son las condiciones minimsa que nos indican que el login funciono

             Consejo:
               Podes agrear mas asserts si queres validad claims, expiracion, etc.
               Mantener los asserts simples y claros
             */
            Assert.NotNull(response);
            Assert.False(string.IsNullOrEmpty(response.AccessToken));

        }

        [Fact]
        public void Login_InvalidPassword_ThrowsUnauthorizedException()
        {
            // Arrange
            // Act
            // Assert
        }

        [Fact]
        public async Task Register_CreatedNewUser_ReturnsOk()
        {

            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var mockRoleRepo = new Mock<IRoleRepository>();

            var role = new Role("PRESUPUESTISTA", new[] { "CREATE", "READ" });

            //var registerRequest = new RegisterRequest(
            //        46924236,
            //        "gomezdiegoelias1@gmail.com",
            //        "123456",
            //        "Diego"
            //    );

            var registerRequest = new RegisterRequest(
                Dni: 12345678,
                Email: "gomezdiegoelias1@gmail.com",
                Password: "123456",
                FirstName: "Diego"
            );

            mockUserRepo.Setup(r => r.FindByEmail(registerRequest.Email))
                .ReturnsAsync((User?)null);

            mockUserRepo.Setup(r => r.FindByDni(registerRequest.Dni))
                .ReturnsAsync((User?)null);

            mockRoleRepo.Setup(r => r.FindByName(role.Name))
                .ReturnsAsync(new RoleEntity
                {
                    Name = role.Name,
                });

            var jwtConfig = new JwtConfigDto { Secret = "key", ExpirationMinutes = 60, Issuer = "test", Audience = "test" };
            var service = new AuthService(mockUserRepo.Object, mockRoleRepo.Object, jwtConfig);

            // Act
            var response = await service.Register(registerRequest);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("User created successfully", response.Message);

        }

    }
}
