using Microsoft.AspNetCore.Identity;
using TicketDesk.Domain.Entities;

namespace TicketDesk.BL;

public class PasswordService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(User user, string plainPassword) => _hasher.HashPassword(user, plainPassword);

    public bool Verify(User user, string hash, string plainPassword) =>
        _hasher.VerifyHashedPassword(user, hash, plainPassword) == PasswordVerificationResult.Success;
}