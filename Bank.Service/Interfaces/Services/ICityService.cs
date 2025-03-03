using Bank.DTO;

namespace Bank.Service.Interfaces.Services;

public interface ICityService
{
    Task<City> GetCity(int cityId);
    Task<IQueryable<City>> GetCities();
    void AddCity(City city);
    void UpdateCity(City city);
    void DeleteCity(int cityId);

    Task<City> GetCityAsync(int cityId);
    Task AddCityAsync(City city);
    Task DeleteCityAsync(int cityId);
}
