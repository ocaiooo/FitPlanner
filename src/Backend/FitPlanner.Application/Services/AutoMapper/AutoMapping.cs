﻿using AutoMapper;
using FitPlanner.Communication.Requests;

namespace FitPlanner.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest =>  dest.Password, opt => opt.Ignore());
    }

    private void DomainToResponse()
    {
        throw new NotImplementedException();
    }
}