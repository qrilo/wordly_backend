using Bogus;
using Wordly.Application.Models.Auth;
using Wordly.Core.Models.Entities;

namespace Wordly.Tests.Common;

public sealed class DataGenerator
{
    private readonly Faker _faker = new();

    public UserRegisterRequest GivenUserRegisterRequest()
    {
        var person = new Person();

        return new UserRegisterRequest()
        {
            Email = person.Email,
            Name = person.FirstName,
            Password = GivenPassword(),
        };
    }

    public User GivenUser()
    {
        var person = new Person();
        return new User(person.FirstName, person.Email, GivenPassword());
    }

    private string GivenPassword()
    {
        return _faker.Random.String2(length: 3).ToUpperInvariant() + _faker.Random.Replace("###????@");
    }
}