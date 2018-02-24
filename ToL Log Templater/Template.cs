namespace ToL_Log_Templater
{
    public class Template
    {
        public string Filename { get; set; }
        public string Name { get; set; }
        public string Page1 { get; set; }
        public string Page2 { get; set; }

        public Template(string filename, string name, string page1, string page2)
        {
            Filename = filename;
            Name = name;
            Page1 = page1;
            Page2 = page2;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
