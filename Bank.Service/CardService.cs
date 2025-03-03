using Bank.DTO;
using Bank.Service.Interfaces.Repository;
using Bank.Service.Interfaces.Services;

namespace Bank.Service;

public sealed class CardService : ICardService
{
    private readonly IUnitOfWork _unitOfWork;

    public CardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<Card> GetCard(int cardId)
    {
        Card card = _unitOfWork.CreditCardRepository.Get(cardId);
        return Task.FromResult(card) ?? throw new InvalidDataException("The cardId couldn't not be found in within the data.");
    }

    public Task<IQueryable<Card>> GetCards()
    {
        var cards = _unitOfWork.CreditCardRepository.Set();
        return Task.FromResult(cards) ?? throw new InvalidDataException("The cardId couldn't not be found in within the data.");
    }

    public void CreateCard(Card card)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));
        _unitOfWork.CreditCardRepository.Insert(card);
        _unitOfWork.SaveChanges();
    }

    public void UpdateCard(Card card)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));
        _unitOfWork.CreditCardRepository.Update(card);
        _unitOfWork.SaveChanges();
    }

    public void DeleteCard(int cardId)
    {
        Card card = _unitOfWork.CreditCardRepository.Get(cardId) ?? throw new ArgumentNullException($"The {cardId} does not exist.");
        card.IsDelete = true;
        _unitOfWork.CreditCardRepository.Update(card);
        _unitOfWork.SaveChanges();
    }

    public async Task<Card> GetCardAsync(int cardId)
    {
        try
        {
            return await _unitOfWork.CreditCardRepository.GetAsync(cardId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidDataException($"CardId could not be found within data {ex}.");
        }
    }

    public async Task CreateCardAsync(Card card)
    {
        if(card == null)
            throw new ArgumentNullException(nameof(card));

        await _unitOfWork.CreditCardRepository.InsertAsync(card);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCardAsync(Card card)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));
        _unitOfWork.CreditCardRepository.Update(card);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCardAsync(int id)
    {
        Card card = await _unitOfWork.CreditCardRepository.GetAsync(id)
            ?? throw new ArgumentNullException($"The card with ID:{id} doesn't exists");
        card.IsDelete = true;
        await _unitOfWork.CreditCardRepository.InsertOrUpdateAsync(card);
        await _unitOfWork.SaveChangesAsync();
    }
}
