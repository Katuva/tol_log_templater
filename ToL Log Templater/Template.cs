using System.ComponentModel;

namespace ToL_Log_Templater
{
    public class Template: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _filename { get; set; }
        private string _name { get; set; }
        private string _page1 { get; set; }
        private string _page2 { get; set; }

        public Template() { }

        public Template(string filename, string name, string page1, string page2)
        {
            Filename = filename;
            Name = name;
            Page1 = page1;
            Page2 = page2;
        }

        public string Filename
        {
            get
            {
                return _filename;
            }

            set
            {
                if (value == _filename)
                    return;

                _filename = value;
                NotifyPropertyChanged("Filename");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (value == _name)
                    return;

                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string Page1
        {
            get
            {
                return _page1;
            }

            set
            {
                if (value == _page1)
                    return;

                _page1 = value;
                NotifyPropertyChanged("Page1");
            }
        }

        public string Page2
        {
            get
            {
                return _page2;
            }

            set
            {
                if (value == _page2)
                    return;

                _page2 = value;
                NotifyPropertyChanged("Page2");
            }
        }

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
