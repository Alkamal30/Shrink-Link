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

        var result = string.Empty;
        var number = id;
        
        while (number > 0)
        {
            result = Alphabet[(int)(number % Base)] + result;
            number /= Base;
        }

        return result;
    }

    public long ConvertToId(string shortCode)
    {
        long id = 0;
        long currentBase = 1;

        foreach (var character in shortCode.Reverse())
        {
            var characterIndex = Alphabet.IndexOf(character);
            id += currentBase * characterIndex;
            currentBase *= Base;
        }

        return id;
    }
}
