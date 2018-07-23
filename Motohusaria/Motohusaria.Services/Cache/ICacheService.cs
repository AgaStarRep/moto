using System;

namespace Motohusaria.Services
{
   /// <summary>
    /// Serwis do obsługi cache
    /// </summary>
    public interface ICacheService
{
    /// <summary>
    /// Pobiera dane z key pod danym kluczem
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Klucz z pod którego odczytywane są dane</param>
    /// <returns>Dane z sesji</returns>
    T Get<T>(string key);

    /// <summary>
    /// Usuwa z sesji dane o podanym kluczu
    /// </summary>
    /// <param name="key">Klucz pod którym zapisywane są dane</param>
    void DeleteKey(string key);

    /// <summary>
    /// Usuwa z sesji dane o podanym kluczu
    /// </summary>
    /// <param name="key">Klucz pod którym zapisywane są dane</param>
    void DeleteKey(CacheKey key);

    /// <summary>
    /// Zapisuje do sesji dane o podanym kluczu
    /// </summary>
    /// <typeparam name="T">Tymp zwracanych danych</typeparam>
    /// <param name="key">Klucz</param>
    /// <param name="data">Dane do zapisu</param>
    /// <param name="cacheTime">Czas przechowywania w cache w minutach</param>
    void Store<T>(string key, T data, int cacheTime);

    /// <summary>
    /// Zwraca obiekt z sesji o podanym kluczu
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    object this[string key] { get; }

    /// <summary>
    /// Czyści wszystkie klucze z cache które zaczynają sie danym kluczem
    /// </summary>
    /// <param name="key">Klucz</param>
    void DeleteKeysStartingWith(string key);
}
}
