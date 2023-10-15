using FluentValidation;
using Wordly.Application.Models.Auth;

namespace Wordly.Application.Contracts;

public interface IAuthValidatorsAggregate
{
    public IValidator<SignInRequest> SignInValidator { get; }
    public IValidator<RefreshTokenRequest> RefreshTokenValidator { get; }
    public IValidator<UserRegisterRequest> UserRegisterValidator { get; }
}