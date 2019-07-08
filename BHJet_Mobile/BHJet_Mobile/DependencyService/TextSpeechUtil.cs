using System.Linq;
using Xamarin.Essentials;

namespace BHJet_Mobile.DependencyService
{
    public static class TextSpeechUtil
    {
        public static async void ExecutarVoz(string textoFala)
        {
            try
            {
                var locales = await TextToSpeech.GetLocalesAsync();
                var settings = new SpeechOptions()
                {
                    Volume = .75f,
                    Pitch = 1.0f,
                    Locale = locales.Where(l => l?.Country != null && l.Country?.ToUpper() == "BR").FirstOrDefault()
                };
                TextToSpeech.SpeakAsync(textoFala, settings);
            }
            catch
            {
            }
        }
    }
}
