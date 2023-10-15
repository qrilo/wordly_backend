using FluentValidation;
using Wordly.Application.Contracts;
using Wordly.Application.Models.Auth;

namespace Wordly.Application.Validators.Auth;

public sealed class AuthValidatorsAggregate : IAuthValidatorsAggregate
{
    public AuthValidatorsAggregate(
        IValidator<SignInRequest> signInValidator,
        IValidator<RefreshTokenRequest> refreshTokenValidator,
        IValidator<UserRegisterRequest> userRegisterValidator)
    {
        SignInValidator = signInValidator;
        RefreshTokenValidator = refreshTokenValidator;
        UserRegisterValidator = userRegisterValidator;
    }

    public IValidator<SignInRequest> SignInValidator { get; }
    public IValidator<RefreshTokenRequest> RefreshTokenValidator { get; }
    public IValidator<UserRegisterRequest> UserRegisterValidator { get; }
}