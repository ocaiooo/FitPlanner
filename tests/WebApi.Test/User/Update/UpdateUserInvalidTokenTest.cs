﻿using System.Net;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidTokenTest : FitPlannerClassFixture
{
    private const string Method = "user";
    
    public UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(Method, request, token: "tokenInvalid");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(Method, request, token: string.Empty);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(Method, request, token);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}