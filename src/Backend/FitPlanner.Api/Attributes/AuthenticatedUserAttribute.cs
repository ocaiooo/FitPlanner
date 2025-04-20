using FitPlanner.Api.Filters;
using FitPlanner.Communication.Responses;
using FitPlanner.Domain.Respositories.User;
using FitPlanner.Domain.Security.Tokens;
using FitPlanner.Exceptions;
using FitPlanner.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace FitPlanner.Api.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter)) { }
}