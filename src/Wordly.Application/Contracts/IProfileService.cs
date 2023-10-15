using System.Threading.Tasks;
using Wordly.Application.Models.Profile;

namespace Wordly.Application.Contracts;

public interface IProfileService
{
    Task<CurrentUserProfileResponse> GetCurrentProfile();
}