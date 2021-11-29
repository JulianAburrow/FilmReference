namespace FilmReference.FrontEnd.Classes.Helpers
{
    public class ReplacementHelper
    {
        public string ReplaceForRadioButton(string name)
        {
            // We need to make these replacements or the values won't 
            // match when creating RadioButtons - and the postback won't
            // filter. No idea why "." gets replaced with a "z" - just
            // an arbitrary value I guess...
            return name.Replace(" ", "_").Replace(".", "z");
        }

        public string ShowCorrectRecordText(int recordCount)
        {
            // Just to be really pedantic so that we don't get
            // "1 records found" above our tables
            return recordCount == 1 ? "" : "s";
        }
    }
}
