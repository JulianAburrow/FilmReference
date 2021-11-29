namespace FilmReference.FrontEnd.Config
{
    public class ConfigValues
    {
        public class StringValues
        {
            public const string FilmReferenceContext = "FilmReferenceContext";
            // Pages
            public const string MainIndexPage = "/Index";
            public const string DirectorIndexPage = "/DirectorPages/Index";
            public const string FilmIndexPage = "/FilmPages/Index";
            public const string GenreIndexPage = "/GenrePages/Index";
            public const string PersonIndexPage = "/PersonPages/Index";
            public const string StudioIndexPage = "/StudioPages/Index";
            // Picture Errors
            public const string FilmPicture = "Film.Picture";
            public const string PersonPicture = "Person.Picture";
            public const string StudioPicture = "Studio.Picture";
        }

        public class PleaseSelect
        {
            public const int Int = -1;
            public const string Text = "Please select";
        }

        public class All
        {
            public const int Int = 0;
            public const string Text = "All";
        }
    }
}
