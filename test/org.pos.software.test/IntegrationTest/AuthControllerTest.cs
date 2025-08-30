using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software;

namespace org.pos.software.test.IntegrationTest
{

    /*
     Test de integración para el AuthController.
     Estos tests utilizan la clase WebApplicationFactory para levantar la API real en memoria
     y ejecutar llamadas HTTP como si fueran clientes externos.
     IMPORTANTE: En este caso se conecta a la BASE DE DATOS REAL (MySQL),
     por lo tanto los datos deben existir en la DB (ej: el usuario usado en Login).
    */ 
    public class AuthControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient _client;

        /*
         El constructor recibe un WebApplicationFactory que levanta el Program de la API.
         Esto permite simular peticiones HTTP reales contra la aplicación.
        */
        public AuthControllerTest(WebApplicationFactory<Program> factory)
        {
            // Se crea un HttpClient que hace las peticiones a la API en memoria.
            _client = factory.CreateClient();
        }

        /*
         Caso de integración: login con credenciales válidas.
         Se espera que la API responda 200 OK y devuelva un token de acceso.
         IMPORTANTE: El usuario y password deben existir en la DB real.
        */
        [Fact]
        public async Task Login_WhitValidCredentials_ReturnsOkAndToken()
        {

            // Arrange (Preparación del escenario)
            // Se crea el request con un email y password que existen en la DB real.
            var loginRequest = new LoginRequest("gomezdiegoelias1@gmail.com", "123456");

            // Act (Ejecución de la acción)
            // Se hace la llamada POST al endpoint real de la API.
            var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

            // Assert (Comprobaciones)
            // Se espera que la respuesta sea 200 OK.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Se deserializa el contenido a un AuthResponse.
            var content = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Se valida que la respuesta no sea nula y que el token no esté vacío.
            Assert.NotNull(content);
            Assert.False(string.IsNullOrEmpty(content.AccessToken));
        
        }

        /*
         Caso de integración: login con password inválido.
         Se espera que la API responda con 401 Unauthorized.
        */
        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            // Act
            // Assert
        }

    }
}
