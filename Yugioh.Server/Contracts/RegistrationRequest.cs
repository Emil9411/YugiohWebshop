using System.ComponentModel.DataAnnotations;

namespace Yugioh.Server.Contracts
{
    public record RegistrationRequest([Required] string Email,
                                      [Required] string Username,
                                      [Required] string Password);
}
