using System.ComponentModel.DataAnnotations;

namespace Yugioh.Server.Services.AuthServices.Requests
{
    public record RegistrationRequest([Required] string Email,
                                      [Required] string Username,
                                      [Required] string Password);
}
