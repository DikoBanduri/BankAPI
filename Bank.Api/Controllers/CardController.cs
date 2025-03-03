using Bank.DTO;
using Bank.Model;
using Bank.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CardController : ControllerBase
{
    private readonly ICardService _cardService;
    private readonly ILogger<CardController> _logger;

    public CardController(ICardService cardService, ILogger<CardController> logger)
    {
        _cardService = cardService;
        _logger = logger;
    }

    [HttpGet]
    public Task<IQueryable<Card>> GetCards()
    {
        var cards = _cardService.GetCards();
        return cards ?? throw new ArgumentNullException($"No Cards can be loaded!{nameof(cards)}");
    }

    [HttpGet("{id:int}")]
    public async Task<Card> GetCardAsync(int id)
    {
        var card = await _cardService.GetCardAsync(id);
        return card ?? throw new ArgumentNullException(nameof(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCardAsync(CardModel cardModel)
    {
        if (cardModel == null)
            return BadRequest("Invalid card data");

        Card card = new()
        {
            Owner = cardModel.Owner,
            Type = cardModel.Type,
            Number = cardModel.Number,
            Cvc = cardModel.Cvc,
            ExpirationDate = cardModel.ExpirationDate,
            AccountId = cardModel.AccountId
        };

        await _cardService.CreateCardAsync(card);
        return Ok(card);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCard(int id, [FromBody] CardModel cardModel)
    {
        if (cardModel == null)
            return BadRequest("Invalid card data");

        var existingCard = await _cardService.GetCardAsync(id);
        if (existingCard == null)
            return NotFound($"Card with ID {id} not found.");

        existingCard.Owner = cardModel.Owner;
        existingCard.Type = cardModel.Type;
        existingCard.Number = cardModel.Number;
        existingCard.Cvc = cardModel.Cvc;
        existingCard.ExpirationDate = cardModel.ExpirationDate;
        existingCard.AccountId = cardModel.AccountId;

        _cardService.UpdateCard(existingCard);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCardAsync(int id)
    {
        var card = await _cardService.GetCardAsync(id);
        if (card == null)
            return NotFound($"Card with ID {id} not found");

        await _cardService.DeleteCardAsync(id);
        return NoContent();
    }
}
