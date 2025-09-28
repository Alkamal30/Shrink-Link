using ShrinkLink.LinkService.Domain.Services;

namespace ShrinkLink.LinkService.Infrastructure.Services;

public class ShortCodeService : IShortCodeService
{
    private static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static readonly int Base = Alphabet.Length;

    public string GenerateFromId(long id)
    {
        if (id == 0)
        {
            return Alphabet[0].ToString();
        }

        string result = "";
        long number = id;
        
        while (number > 0)
        {
            result = Alphabet[(int)(number % Base)] + result;
            number /= Base;
        }

        return result;
    }
}
