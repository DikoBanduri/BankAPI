using Bank.DTO;
using Bank.Service.Interfaces.Repository;
using Bank.Service.Interfaces.Services;

namespace Bank.Service;

public sealed class CityService : ICityService
{
    private readonly IUnitOfWork _unitOfWork;

    public CityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Task<City> GetCity(int cityId)
    {
        City city = _unitOfWork.CityRepository.Get(cityId) ?? throw new InvalidDataException("CityId could not be found");
        return Task.FromResult(city) ?? throw new InvalidDataException("The AccountId could not be found");
    }

    public Task<IQueryable<City>> GetCities()
    {
        var cities = _unitOfWork.CityRepository.Set() ?? throw new InvalidDataException("Cities could not be loaded");
        return Task.FromResult(cities) ?? throw new InvalidDataException("The AccountId could not be found");
    }

    public void AddCity(City city)
    {
        if (city == null) throw new ArgumentNullException(nameof(city));
        _unitOfWork.CityRepository.Insert(city);
        _unitOfWork.SaveChanges();
    }
    public void UpdateCity(City city)
    {
        if (city == null) throw new ArgumentNullException(nameof(city));
        _unitOfWork.CityRepository.Update(city);
        _unitOfWork.SaveChanges();
    }

    public void DeleteCity(int cityId)
    {
        City city = _unitOfWork.CityRepository.Get(cityId) ?? throw new InvalidDataException("CityId Could not be found");
        city.IsDelete = true;
        _unitOfWork.CityRepository.Update(city);
        _unitOfWork.SaveChanges();
    }

    public async Task<City> GetCityAsync(int cityId)
    {
        try
        {
            return await _unitOfWork.CityRepository.GetAsync(cityId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidDataException($"The CityId could not be found {ex}");
        }
    }

    public async Task AddCityAsync(City city)
    {
        if (city == null) throw new ArgumentNullException(nameof(city));
        await _unitOfWork.CityRepository.InsertAsync(city);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCityAsync(int cityId)
    {
        City city = await _unitOfWork.CityRepository.GetAsync(cityId) ?? throw new InvalidDataException("CityId Could not be found");
        city.IsDelete = true;
        await _unitOfWork.CityRepository.InsertOrUpdateAsync(city);
        await _unitOfWork.SaveChangesAsync();
    }
}
