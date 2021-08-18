namespace Domain.Managers
{
    public class BaseManager
    {
        protected const char TABS = '\t';
        protected const char SPACE = '\b';
        protected const char SEMICOLON = ';';
    }

    public enum Delimiters
    {
        Tabs,
        Semicolon,
        Space
    }
}
