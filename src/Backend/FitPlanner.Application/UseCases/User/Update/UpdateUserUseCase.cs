using FitPlanner.Communication.Requests;
using FitPlanner.Domain.Repositories.User;
using FitPlanner.Domain.Respositories;
using FitPlanner.Domain.Respositories.User;
using FitPlanner.Domain.Services.LoggedUser;
using FitPlanner.Exceptions;
using FitPlanner.Exceptions.ExceptionsBase;
using FluentValidation;

namespace FitPlanner.Application.UseCases.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUserReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.User();

        await Validate(request, loggedUser.Email);
        
        var user = await _repository.GetById(loggedUser.Id);
        
        user.Name = request.Name;
        user.Email = request.Email;

        _repository.Update(user);
        
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();
        
        var result = await validator.ValidateAsync(request);

        if (!currentEmail.Equals(request.Email))
        {
            var userExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
            
            if (userExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
