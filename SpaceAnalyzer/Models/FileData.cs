namespace SpaceAnalyzer.Models
{
    public class FileData
    {
        private string _path;
        private string _type;
        private double _size;
        private DateTime _createdDate;

        public FileData (string path, string type, double size, DateTime createdDate)
        {
            if (path == null)
            { 
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Тип не может быть пустым", nameof(type));
            }

            if (size < 0)
            { 
                throw new ArgumentOutOfRangeException(nameof(size), "Размер файла не может быть меьнше 0");
            }

            _path = path;
            _type = type;
            _size = size;
            _createdDate = createdDate;
        }

        public string GetPath() => _path;
        public string GetType() => _type;
        public double GetSize() => _size;
        public DateTime GetCreatedDate() => _createdDate;


        //вычисляемое свойство (форматирует размер файла в более удобную форму для чтения)
        public string sizeTemplate 
        {
            get 
            {
                string[] sizeUnit = { "bytes", "Kb", "Mb", "Gb"};
                double lenth = _size;
                int order = 0;

                while (lenth >= 1024 && order < sizeUnit.Length - 1) 
                { 
                    order++;
                    lenth /= 1024;
                }
                return $"{lenth:0.##} {sizeUnit[order]}";
            }  
        }
    }
}
