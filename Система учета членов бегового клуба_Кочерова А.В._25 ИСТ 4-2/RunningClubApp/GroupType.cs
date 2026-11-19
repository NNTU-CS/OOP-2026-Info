using System;

namespace RunningClubApp
{
    public class GroupType
    {
        private bool _isMain;
        private string _name;

        public GroupType(string name, bool isMain = false)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _isMain = isMain;
        }

        public bool IsMain
        {
            get => _isMain;
            set => _isMain = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override string ToString() => _name;

        public override bool Equals(object obj)
        {
            GroupType other = obj as GroupType; // Приведение типов: сущность GroupType должна быть объектом

            if (other == null)
                return false;

            if (_name != other._name)
                return false;

            if (_isMain != other._isMain)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode() ^ _isMain.GetHashCode();
        }
    }
}