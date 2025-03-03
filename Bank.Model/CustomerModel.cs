using Bank.DTO;

namespace Bank.Model;

public record CustomerModel(string FirstName, string LastName, Gender Gender, string Address, string PersonalNumber, int? CityId, int? UserId);
