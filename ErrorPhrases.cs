using System.Collections.Generic;

namespace Odin
{
    public class ErrorPhrases
    {
        private static List<string>? phrases;
        private static int now = 0;

        public static string RandErrorPhrase()
        {
            phrases = new List<string>
            {
                "No te preocupes, Morty. Es solo un error. Lo arreglaremos después.",
                "¡Ay, Rick, esto está fuera de control! ¡Estamos en problemas serios!",
                "¡Esto es solo un pequeño contratiempo, Morty!",
                "¡Rick, esto no va a funcionar! ¡Estamos en problemas!",
                "¡Esto es el apocalipsis, Morty! ¡La locura está por todas partes!",
                "¡Esto no está bien, Rick! ¡No entiendo nada!",
                "¡He creado un monstruo, Morty!",
                "¡Ay, Rick, creo que esto está completamente roto!",
                "¡Todo está mal, Morty! ¡Todo está mal!",
                "¡Esto es muy confuso, Rick! ¡No sé qué hacer!",
                "¡No estoy aquí para resolver todos tus problemas, Morty!",
                "¡Rick, creo que hemos metido la pata esta vez!",
                "¡Es un caos absoluto, Morty!",
                "¡Oh no, esto no se ve bien, Rick!"
            };
            return phrases[now++ % 14];
        }
    }
}
