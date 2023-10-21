namespace SIGEES.Web.Common
{
    public class JQGridButton
    {
        public string Arguments;
        public string Function;
        public string Image;
        public string Title;

        public JQGridButton(string title, string image, string function, string arguments) 
        {
            Arguments = arguments;
            Function = function;
            Image = image;
            Title = title;
        }
    }
}
