using BoardGameTracker.Shared.DataTransferObjects;
using System.ComponentModel.DataAnnotations;

namespace BoardGameTracker.Web
{
    public class AuthenticationApiClient(HttpClient pHttpClient)
    {
        public async Task<UserDto?> GetUserAsync(string pEmail, CancellationToken pCancellationToken = default)
        {
            var user = await pHttpClient.GetFromJsonAsync<UserDto>($"/api/user/{pEmail}", pCancellationToken);
            return user;
        }
        public async Task<bool> RegisterUserAsync(UserDto pUser, CancellationToken pCancellationToken = default)
        {
            var response = await pHttpClient.PostAsJsonAsync("/api/user", pUser, pCancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
