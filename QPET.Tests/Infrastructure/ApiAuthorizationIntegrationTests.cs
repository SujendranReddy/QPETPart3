using Microsoft.AspNetCore.Mvc.Testing;
using QPET.Tests.Infrastructure;
using System.Net;

namespace QPET.Tests.Integration
{
    public class ApiAuthorizationIntegrationTests
        : IClassFixture<
            CustomWebApplicationFactory>
    {
        private readonly HttpClient
            _client;

        public ApiAuthorizationIntegrationTests(
            CustomWebApplicationFactory
                factory)
        {
            _client =
                factory.CreateClient(
                    new WebApplicationFactoryClientOptions
                    {
                        AllowAutoRedirect =
                            false,

                        BaseAddress =
                            new Uri(
                                "https://localhost"
                            )
                    }
                );
        }

        [Fact]
        public async Task EnquiriesApi_Unauthenticated_Returns401()
        {
            var response =
                await _client.GetAsync(
                    "/api/enquiries"
                );

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode
            );
        }

        [Fact]
        public async Task AdminProducts_Unauthenticated_RedirectsToLogin()
        {
            var response =
                await _client.GetAsync(
                    "/Admin/Products"
                );

            Assert.Equal(
                HttpStatusCode.Redirect,
                response.StatusCode
            );

            Assert.NotNull(
                response.Headers.Location
            );

            Assert.Contains(
                "/Admin/Login",
                response.Headers
                    .Location!
                    .ToString()
            );
        }

        [Fact]
        public async Task ProductDeleteApi_Unauthenticated_Returns401()
        {
            var request =
                new HttpRequestMessage(
                    HttpMethod.Delete,
                    "/api/products/1"
                );

            var response =
                await _client.SendAsync(
                    request
                );

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode
            );
        }

        [Fact]
        public async Task ReviewDeleteApi_Unauthenticated_Returns401()
        {
            var request =
                new HttpRequestMessage(
                    HttpMethod.Delete,
                    "/api/reviews/1"
                );

            var response =
                await _client.SendAsync(
                    request
                );

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode
            );
        }
    }
}