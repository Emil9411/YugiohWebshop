using System.ComponentModel.DataAnnotations;

namespace Yugioh.Server.Services.AuthServices.Requests
{
    public record UpdatePersonalDataRequest([Required] string Email,
                                            [Required] string FirstName,
                                            [Required] string LastName,
                                            [Required] string Country,
                                            [Required] string City,
                                            [Required] string Address,
                                            [Required] string PostalCode,
                                            [Required] string PhoneNumber);
}
