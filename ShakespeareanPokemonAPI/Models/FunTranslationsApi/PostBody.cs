//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.FunTranslationsApi
{
    //I'm not good at naming things.
    public class PostBody
    {
        public PostBody(string text)
        {
            this.text = text;
        }

        public string text { get; set; }
    }
}
