using Flurl.Http;
using Wordly.Application.Models.Auth;

namespace Wordly.IntegrationTests.Testing;

public sealed record AuthorizedClient(IFlurlClient FlurlClient, UserRegisterRequest UserInfo, AuthResponse AuthInfo);