using Microsoft.AspNetCore.Authorization;
using QPET.Controllers;
using QPET.Controllers.Api;
using System.Reflection;

namespace QPET.Tests.Security
{
    public class AuthorizationMetadataTests
    {
        [Fact]
        public void AdminController_RequiresAdminRole()
        {
            var attribute =
                Assert.Single(
                    typeof(AdminController)
                        .GetCustomAttributes<
                            AuthorizeAttribute>()
                );

            Assert.Equal(
                "Admin",
                attribute.Roles
            );

            Assert.Equal(
                "QPETAdmin",
                attribute
                    .AuthenticationSchemes
            );
        }

        [Fact]
        public void ProductWriteEndpoints_RequireAdminRole()
        {
            AssertAdminProtected(
                typeof(
                    ProductsApiController
                ),
                "CreateProduct"
            );

            AssertAdminProtected(
                typeof(
                    ProductsApiController
                ),
                "UpdateProduct"
            );

            AssertAdminProtected(
                typeof(
                    ProductsApiController
                ),
                "DeleteProduct"
            );
        }

        [Fact]
        public void ReviewDeleteEndpoint_RequiresAdminRole()
        {
            AssertAdminProtected(
                typeof(
                    ReviewsApiController
                ),
                "DeleteReview"
            );
        }

        [Fact]
        public void EnquiryAdminEndpoints_RequireAdminRole()
        {
            AssertAdminProtected(
                typeof(
                    EnquiriesApiController
                ),
                "GetEnquiries"
            );

            AssertAdminProtected(
                typeof(
                    EnquiriesApiController
                ),
                "GetEnquiry"
            );

            AssertAdminProtected(
                typeof(
                    EnquiriesApiController
                ),
                "UpdateEnquiryStatus"
            );
        }

        private static void
            AssertAdminProtected(
                Type controllerType,
                string methodName)
        {
            var method =
                controllerType.GetMethod(
                    methodName
                );

            Assert.NotNull(method);

            var attribute =
                Assert.Single(
                    method!
                        .GetCustomAttributes<
                            AuthorizeAttribute>()
                );

            Assert.Equal(
                "Admin",
                attribute.Roles
            );

            Assert.Equal(
                "QPETAdmin",
                attribute
                    .AuthenticationSchemes
            );
        }
    }
}