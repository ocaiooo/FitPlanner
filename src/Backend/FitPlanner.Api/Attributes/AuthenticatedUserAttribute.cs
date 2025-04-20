using FitPlanner.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FitPlanner.Api.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter)) { }
}