using DomainLayer.Models.Booking_Transaction;
using DomainLayer.RepositoryInterface;
using ServiceAbstraction;

namespace ServiceImplementation
{
    public class TransactionService : ITransactionService
    {
        private readonly IGenericRepository<Transaction, int> _transactionRepo;

        public TransactionService(IGenericRepository<Transaction, int> transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }

        public async Task CreateAsync(Transaction transaction)
        {
            await _transactionRepo.Add(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _transactionRepo.GetAllAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _transactionRepo.GetByIdAsync(id);
        }


    }

}
