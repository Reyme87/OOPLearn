using AuthService.Application.Interfaces;
using AuthService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly IAppDbContext _dbContext;

        public UserRepository(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AsNoTracking()
                                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }
    }
}
